using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.TokenModel;
using LearningManagementSystem.Models.UserModel;
using LearningManagementSystem.Models.UserRoleModels;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LearningManagementSystem.Repository
{
    public class UserRepository : InterfaceUserRepository
    {
        private readonly LearningManagementSystemContext _context;
        private readonly IConfiguration _iConfiguration;
        private readonly Cloudinary _cloudinary;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserRepository(LearningManagementSystemContext context, IConfiguration iConfiguration
            , Cloudinary cloudinary, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this._context = context;
            this._iConfiguration = iConfiguration;
            this._cloudinary = cloudinary;
            this._mapper = mapper;
            this._httpContextAccessor = httpContextAccessor;
        }
        //Private
        private async Task<TokenModel> GenerateToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_iConfiguration["JWT:Secret"]);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("UserId", user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    //roles
                    new Claim("RoleId", user.RoleId.ToString()),
                    new Claim(ClaimTypes.Role, await _context.UserRoles
                                .Where(r => r.RoleId == user.RoleId)
                                .Select(r => r.RoleName.ToLower())
                                .FirstOrDefaultAsync()),
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                Issuer = _iConfiguration["JWT:ValidIssuer"], // Issuer của token
                Audience = _iConfiguration["JWT:ValidAudience"], // Audience của token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256),

            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);

            var accessToken = jwtTokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();
            //Lưu DB
            var refreshTokenEntity = new RefreshToken
            {
                Id = Guid.NewGuid(),
                JwtId = token.Id,
                UserId = user.UserId,
                Token = refreshToken,
                IsUsed = false,
                IsRevoked = false,
                IssuedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddMinutes(30)
            };
            await _context.AddAsync(refreshTokenEntity);
            await _context.SaveChangesAsync();
            return new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }

        private string GenerateUserCode(string userType)
        {
            // Đếm số lượng người dùng cùng loại
            int userCount = _context.Users.Count(u => u.UserType == userType);

            // Tạo mã người dùng dựa trên loại người dùng và số lượng người dùng hiện tại
            return $"{userType}{userCount + 1:00}";
        }

        private async Task<bool> DeleteImageCloudinary(string imageCloudinaryUri)
        {
            // Lấy URL của hình ảnh hiện tại từ cơ sở dữ liệu hoặc từ bất kỳ nguồn nào khác
            string currentImageUrl = imageCloudinaryUri; // Giả sử user.Avatar là URL của hình ảnh hiện tại

            // Phân tích URL để lấy public ID của hình ảnh trên Cloudinary
            string publicId = string.Empty;
            if (!string.IsNullOrEmpty(currentImageUrl))
            {
                Uri uri = new Uri(currentImageUrl);
                string path = uri.AbsolutePath;
                // Cloudinary public ID là phần sau cùng của đường dẫn URL, loại bỏ phần đuôi mở rộng của tệp (ví dụ: .jpg, .png)
                publicId = Path.GetFileNameWithoutExtension(path);
            }

            // Sử dụng Cloudinary API để xóa hình ảnh sử dụng public ID
            if (!string.IsNullOrEmpty(publicId))
            {
                var deletionParams = new DeletionParams(publicId);
                var deletionResult = await _cloudinary.DestroyAsync(deletionParams);
                // Kiểm tra xem hình ảnh đã được xóa thành công hay không
                if (deletionResult.Result == "ok")
                {
                    // Xóa hình ảnh thành công
                    return true;
                }
                else
                {
                    // Xóa hình ảnh thất bại
                    return false;
                }
            }
            return false;

        }
        //Public
        public async Task<APIResponse> RenewToken(TokenModel model)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_iConfiguration["JWT:Secret"]);
            var tokenValidateParam = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false, // ko kiểm tra hết hạn
                ValidateIssuerSigningKey = true,


                ValidAudience = _iConfiguration["JWT:ValidAudience"],
                ValidIssuer = _iConfiguration["JWT:ValidIssuer"],
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
                ClockSkew = TimeSpan.Zero

            };

            //check 1: AccessToken valid format
            var tokenInVerification = jwtTokenHandler.ValidateToken(model.AccessToken,
                tokenValidateParam, out var validatedToken);

            //check 2: Check alg
            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase);
                if (!result)//false
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Invalid token"
                    };
                }
            }
            //3 Check accessToken expire?
            var jwtToken = jwtTokenHandler.ReadToken(model.AccessToken) as JwtSecurityToken;
            if (jwtToken == null)
            {
                // Token không hợp lệ
                return new APIResponse
                {
                    Success = false,
                    Message = "Invalid token"
                };
            }
            else
            {
                // Lấy thời gian hết hạn từ token
                var expirationTime = jwtToken.ValidTo;
                if (expirationTime > DateTime.UtcNow)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Access token has not yet expired"
                    };
                }
            }
            //check 3: Check accessToken expire?
            //var utcExpireDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(
            //    x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            //var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);
            //if (expireDate > DateTime.UtcNow)
            //{
            //    return new ApiResponse
            //    {
            //        Success = false,
            //        Message = "Access token has not yet expired"
            //    };
            //}

            //check 4: Check refreshtoken exist in DB
            var storedToken = _context.RefreshTokens.FirstOrDefault(x => x.Token == model.RefreshToken);
            if (storedToken == null)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Refresh token does not exist"
                };
            }

            //check 5: check refreshToken is used/revoked?
            if (storedToken.IsUsed)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Refresh token has been used"
                };
            }
            if (storedToken.IsRevoked)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Refresh token has been revoked"
                };
            }

            //check 6: AccessToken id == JwtId in RefreshToken
            var jti = tokenInVerification.Claims.FirstOrDefault(
                x => x.Type == JwtRegisteredClaimNames.Jti).Value;
            if (storedToken.JwtId != jti)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Token doesn't match"
                };
            }

            //Update token is used
            storedToken.IsRevoked = true;
            storedToken.IsUsed = true;
            _context.Update(storedToken);
            await _context.SaveChangesAsync();

            //create new token
            var user = await _context.Users.SingleOrDefaultAsync(nd => nd.UserId == storedToken.UserId);
            var token = await GenerateToken(user);

            return new APIResponse
            {
                Success = true,
                Message = "Renew token success",
                Data = token
            };
        }

        public async Task<APIResponse> SignIn(SignInModel model)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email.Equals(model.Email));

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Invalid username/password"
                };
            }
            else if (!user.IsActived)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Account is not activated"
                };
            }
            else
            {
                var token = await GenerateToken(user);
                return new APIResponse
                {
                    Success = true,
                    Message = "Authenticate success",
                    Data = token
                };
            }
        }

        public async Task<APIResponse> CreateLeadershipUser(UserModelAllType model)
        {
            var userPrincipal = _httpContextAccessor.HttpContext.User;
            var userEmail = userPrincipal.FindFirstValue(ClaimTypes.Email);
            var userCheck = await _context.Users.Include(role => role.UserRole).FirstOrDefaultAsync(u => u.Email == userEmail);
            if (userCheck.UserRole.UserAccountEdit)
            {
                // Kiểm tra xem người dùng đã tồn tại chưa
                if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Email exists"
                    };
                }
                string imageUrl;
                if (model.imageFile != null)
                {
                    // Tải lên ảnh lên Cloudinary
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(model.imageFile.FileName, model.imageFile.OpenReadStream()),
                        // Các tham số tải lên khác
                    };
                    var uploadResult = _cloudinary.Upload(uploadParams);

                    // Lấy URL của ảnh từ kết quả tải lên
                    imageUrl = uploadResult.SecureUrl.ToString();
                    if (imageUrl == null)
                    {
                        return new APIResponse
                        {
                            Success = false,
                            Message = "Upload image failed."
                        };
                    }
                }
                else
                {
                    imageUrl = "https://res.cloudinary.com/dicxcmntw/image/upload/v1709443071/awzwxwub3veotajal6lc.png";
                }
                // Mã hóa mật khẩu trước khi lưu vào cơ sở dữ liệu
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
                var newUser = _mapper.Map<User>(model);
                newUser.UserType = "QT";
                newUser.UserCode = GenerateUserCode("QT");
                newUser.Password = hashedPassword;
                newUser.Avatar = imageUrl;
                //Set thông báo là true khi khởi tạo Leadership (Loại thông báo Leadership)
                //Thông báo chung
                newUser.NotificationWhenUpdatingAccount = true;
                newUser.NotificationWhenChangingPassword = true;
                //Thông báo của Leadership
                #region
                newUser.LeadershipNotificationWhenYouMakeChangesInTheRoleList = true;
                newUser.LeadershipNotificationWhenYouMakeChangesInTheUserList = true;
                newUser.LeadershipNotificationWhenInstructorsSaveNewExamQuestionsIntoTheSystem = true;
                newUser.LeadershipNotificationWhenYouConfirmOrCancelTheTest = true;
                newUser.LeadershipNotificationWhenYouCreateOrChangeNamesOrDeletePrivateFiles = true;
                newUser.LeadershipNotificationWhenThereAreChangesInSubjectContent = true;
                newUser.LeadershipNotificationWhenThereAreChangesInSubjectManagement = true;
                #endregion
                await _context.AddAsync(newUser);
                await _context.SaveChangesAsync();
                return new APIResponse
                {
                    Success = true,
                    Message = "Sign Up Success."
                };
            }
            else
            {
                return new APIResponse { Success = false, Message = "The account does not have permission to create user" };
            }
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_iConfiguration["JWT:Secret"]);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
                ValidateIssuer = true,
                ValidIssuer = _iConfiguration["JWT:ValidIssuer"], // Issuer của token
                ValidateAudience = true,
                ValidAudience = _iConfiguration["JWT:ValidAudience"], // Audience của token
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            SecurityToken validatedToken;
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

            return principal;
        }

        public async Task<APIResponse> ActiveUser(string id)
        {
            var userPrincipal = _httpContextAccessor.HttpContext.User;
            var userEmail = userPrincipal.FindFirstValue(ClaimTypes.Email);
            var userCheck = await _context.Users.Include(role => role.UserRole).FirstOrDefaultAsync(u => u.Email == userEmail);
            if (userCheck.UserRole.UserAccountEdit)
            {
                try
                {
                    var user = await _context.Users.FindAsync(Guid.Parse(id));
                    if (!user.IsActived)
                    {
                        user.IsActived = true;
                    }
                    else
                    {
                        user.IsActived = false;
                    }
                    await _context.SaveChangesAsync();
                    return new APIResponse { Success = true, Message = "ActiveUser - DeactiveUser successfully" };
                }
                catch (Exception ex)
                {
                    return new APIResponse { Success = false, Message = $"Error ActiveUser: {ex.Message}" };
                }
            }
            else
            {
                return new APIResponse { Success = false, Message = "The account does not have permission to edit user information" };
            }
        }

        public async Task<APIResponse> UpdateUserPersonalInformation(string id, UserModelUpdateAllType model)
        {
            var userPrincipal = _httpContextAccessor.HttpContext.User;
            var userEmail = userPrincipal.FindFirstValue(ClaimTypes.Email);
            var userCheck = await _context.Users.Include(role => role.UserRole).FirstOrDefaultAsync(u => u.Email == userEmail);
            if (userCheck.UserRole.UserAccountEdit)
            {
                try
                {
                    var user = await _context.Users.FindAsync(Guid.Parse(id));

                    if (user == null)
                    {
                        return new APIResponse { Success = false, Message = "User not found" };
                    }
                    //_mapper.Map(model, userRole);

                    // Cập nhật từng trường một từ model vào userRole
                    #region
                    foreach (var property in typeof(UserModelUpdateAllType).GetProperties())
                    {
                        var modelValue = property.GetValue(model);
                        if (modelValue != null)
                        {
                            var userProperty = typeof(User).GetProperty(property.Name);
                            if (userProperty != null)
                            {
                                userProperty.SetValue(user, modelValue);
                            }
                        }
                    }
                    #endregion
                    await _context.SaveChangesAsync();

                    return new APIResponse { Success = true, Message = "User updated successfully" };
                }
                catch (Exception ex)
                {
                    return new APIResponse { Success = false, Message = $"Error updating User: {ex.Message}" };
                }
            }
            else
            {
                return new APIResponse { Success = false, Message = "The account does not have permission to edit user information" };
            }
        }

        public async Task<APIResponse> ChangeUserAvatar(UserModelChangeAvatar model)
        {
            var user = await _context.Users.Include(role => role.UserRole)
                .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));
            // Lấy URL của hình ảnh hiện tại từ cơ sở dữ liệu hoặc từ bất kỳ nguồn nào khác
            string currentImageUrl = user.Avatar; // Giả sử user.Avatar là URL của hình ảnh hiện tại

            if (currentImageUrl.Equals("https://res.cloudinary.com/dicxcmntw/image/upload/v1709443071/awzwxwub3veotajal6lc.png"))
            {
                string imageUrl;
                if (model.imageFile != null)
                {
                    // Tải lên ảnh lên Cloudinary
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(model.imageFile.FileName, model.imageFile.OpenReadStream()),
                        // Các tham số tải lên khác
                    };
                    var uploadResult = _cloudinary.Upload(uploadParams);

                    // Lấy URL của ảnh từ kết quả tải lên
                    imageUrl = uploadResult.SecureUrl.ToString();
                    if (imageUrl == null)
                    {
                        return new APIResponse
                        {
                            Success = false,
                            Message = "Upload image failed."
                        };
                    }
                }
                else
                {
                    return new APIResponse { Success = true, Message = "No photo found" };
                }
                user.Avatar = imageUrl;
                await _context.SaveChangesAsync();
                return new APIResponse { Success = true, Message = "ChangeUserAvatar successfully" };
            }
            else if(!currentImageUrl.Equals("https://res.cloudinary.com/dicxcmntw/image/upload/v1709443071/awzwxwub3veotajal6lc.png"))
            {
                // Phân tích URL để lấy public ID của hình ảnh trên Cloudinary
                string publicId = string.Empty;
                if (!string.IsNullOrEmpty(currentImageUrl))
                {
                    Uri uri = new Uri(currentImageUrl);
                    string path = uri.AbsolutePath;
                    // Cloudinary public ID là phần sau cùng của đường dẫn URL, loại bỏ phần đuôi mở rộng của tệp (ví dụ: .jpg, .png)
                    publicId = Path.GetFileNameWithoutExtension(path);
                }

                // Sử dụng Cloudinary API để xóa hình ảnh sử dụng public ID
                if (!string.IsNullOrEmpty(publicId))
                {
                    var deletionParams = new DeletionParams(publicId);
                    var deletionResult = await _cloudinary.DestroyAsync(deletionParams);
                    // Kiểm tra xem hình ảnh đã được xóa thành công hay không
                    if (deletionResult.Result == "ok")
                    {
                        // Xóa hình ảnh thành công
                        string imageUrl;
                        if (model.imageFile != null)
                        {
                            // Tải lên ảnh lên Cloudinary
                            var uploadParams = new ImageUploadParams()
                            {
                                File = new FileDescription(model.imageFile.FileName, model.imageFile.OpenReadStream()),
                                // Các tham số tải lên khác
                            };
                            var uploadResult = _cloudinary.Upload(uploadParams);

                            // Lấy URL của ảnh từ kết quả tải lên
                            imageUrl = uploadResult.SecureUrl.ToString();
                            if (imageUrl == null)
                            {
                                return new APIResponse
                                {
                                    Success = false,
                                    Message = "Upload image failed."
                                };
                            }
                        }
                        else
                        {
                            return new APIResponse { Success = true, Message = "No photo found" };
                        }
                        user.Avatar = imageUrl;
                        await _context.SaveChangesAsync();
                        return new APIResponse { Success = true, Message = "ChangeUserAvatar successfully" };
                    }
                    else
                    {
                        return new APIResponse { Success = false, Message = "Deleting current avatar failed" };
                    }
                }
            }
            return new APIResponse { Success = true, Message = "Something Error" };

        }
    }
}
