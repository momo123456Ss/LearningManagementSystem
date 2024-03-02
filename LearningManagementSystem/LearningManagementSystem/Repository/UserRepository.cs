using AutoMapper;
using CloudinaryDotNet;
using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.TokenModel;
using LearningManagementSystem.Models.UserModel;
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
        public UserRepository(LearningManagementSystemContext context, IConfiguration iConfiguration, Cloudinary cloudinary, IMapper mapper)
        {
            _context = context;
            _iConfiguration = iConfiguration;
            _cloudinary = cloudinary;
            _mapper = mapper;
        }
        //Private
        private async Task<TokenModel> GenerateToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_iConfiguration["JWT:Secret"]);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
                    new Claim(ClaimTypes.Role, await _context.UserRoles
                                .Where(r => r.RoleId == user.RoleId)
                                .Select(r => r.RoleName)
                                .FirstOrDefaultAsync()),

                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserId", user.UserId.ToString()),

                    //roles
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

        public Task<APIResponse> SignIn(SignInModel model)
        {
            throw new NotImplementedException();
        }

        public Task<APIResponse> SignUp(SignUpModel model)
        {
            throw new NotImplementedException();
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
    }
}
