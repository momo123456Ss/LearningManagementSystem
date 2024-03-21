using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.APIRespone;
using LearningManagementSystem.Models.ExamAndTestQuestionModel;
using LearningManagementSystem.Models.PaginatedList;
using LearningManagementSystem.Repository.InterfaceRepository;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using Break = DocumentFormat.OpenXml.Wordprocessing.Break;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;

namespace LearningManagementSystem.Repository
{
    public class EaTQuestionRepository : InterfaceEaTQuestionRepository
    {
        private readonly LearningManagementSystemContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Cloudinary _cloudinary;
        private static int PAGE_SIZE { get; set; } = 10;
        private readonly string _templatePath = @"E:\.NET CORE API\LearningManagementSystem\LearningManagementSystem\LearningManagementSystem\Uploads\Files\Template\mau-de-thi-trac-nghiem-v2.docx";

        public EaTQuestionRepository(LearningManagementSystemContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, Cloudinary cloudinary)
        {
            this._context = context;
            this._mapper = mapper;
            this._httpContextAccessor = httpContextAccessor;
            this._cloudinary = cloudinary;
        }
        //Private
        //Body
        #region
        private void ReplacePlaceholder(Body body, string placeholder, string value)
        {
            foreach (var text in body.Descendants<Text>())
            {
                if (text.Text.Contains(placeholder))
                {
                    text.Text = text.Text.Replace(placeholder, value);

                }
            }
        }
        #endregion
        //FOOTER
        #region
        private void ReplacePlaceholder(IEnumerable<FooterPart> footerParts, string placeholder, string value)
        {
            foreach (var footerPart in footerParts)
            {
                ReplacePlaceholderInFooter(footerPart, placeholder, value);
            }
        }
        private void ReplacePlaceholderInFooter(FooterPart footerPart, string placeholder, string value)
        {
            var body = footerPart.Footer.Descendants<Text>().FirstOrDefault(t => t.Text.Contains(placeholder));
            if (body != null)
            {
                body.Text = body.Text.Replace(placeholder, value);
            }
        }
        #endregion
        private void AddQuestions(Body body, string placeholder, List<ExamAndTestQuestion1ViewModel> questions)
        {
            #region
            foreach (var para in body.Descendants<Text>())
            {
                if (para.Text.Contains(placeholder))
                {
                    para.InsertBeforeSelf(new Break());
                    int index = 1;
                    foreach (var question in questions)
                    {
                        para.InsertBeforeSelf(new Break());
                        //Paragraph paragraph = new Paragraph(new Run(new Text($"Câu {index}: {question.ExamAndTestQuestionContent}")));
                        Text newTextQuestion = new Text($"Câu {index} - tier {question.Tier}: {question.ExamAndTestQuestionContent}");
                        para.InsertBeforeSelf(newTextQuestion);
                        para.InsertBeforeSelf(new Break());
                        //body.Append(paragraph);

                        foreach (var answer in question.ExamAndTestAnswerss)
                        {
                            //Paragraph answerParagraph = new Paragraph(new Run(new Text(answer.AnswerContent)));
                            Text newTextAnswer = new Text(answer.AnswerContent);
                            para.InsertBeforeSelf(newTextAnswer);
                            para.InsertBeforeSelf(new Break());
                            //body.Append(answerParagraph);
                        }

                        index++;
                    }
                    para.Text = para.Text.Replace(placeholder, "");
                }
            }
            #endregion
        }
        //GET
        #region
        public async Task<ExamAndTestQuestion1ViewModel> GetById(string id)
        {
            var user = await _context.Users.Include(role => role.UserRole)
              .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));
            if (!user.UserRole.ExamsAndTestsSee)
            {
                return null;
            }
            var question = await _context.ExamAndTestQuestionss
                .Include(a => a.ExamAndTestAnswerss)
                .FirstOrDefaultAsync(q => q.EaTQuestionId.Equals(int.Parse(id)));
            return _mapper.Map<ExamAndTestQuestion1ViewModel>(question);
        }
        public async Task<List<ExamAndTestQuestionViewModel>> GetAll(string? searchString, string? facultyId, string? subjectId, int? tier, int page = 1)
        {
            var user = await _context.Users.Include(role => role.UserRole)
              .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));
            if (!user.UserRole.ExamsAndTestsSee)
            {
                return null;
            }

            var allQuestion = _context.ExamAndTestQuestionss
                .Include(s => s.SubjectNavigation).ThenInclude(u => u.UserNavigation)
                .Include(f => f.FacultyNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                allQuestion = allQuestion.Where(f =>
                    f.ExamAndTestQuestionCode.ToLower().Contains(searchString) ||
                    f.ExamAndTestQuestionContent.ToLower().Contains(searchString)
                );
            }
            allQuestion = allQuestion.OrderBy(sb => sb.ExamAndTestQuestionCode);

            if (!string.IsNullOrEmpty(facultyId))
            {
                allQuestion = allQuestion.Where(f => f.FacultyId.Equals(Guid.Parse(facultyId)));
            }
            if (!string.IsNullOrEmpty(subjectId))
            {
                allQuestion = allQuestion.Where(f => f.SubjectId.Equals(Guid.Parse(subjectId)));
            }
            if (tier.HasValue && tier != null)
            {
                allQuestion = allQuestion.Where(f => f.Tier.Equals(tier));
            }


            if (page < 1)
            {
                return _mapper.Map<List<ExamAndTestQuestionViewModel>>(allQuestion);
            }
            var result = PaginatedList<ExamAndTestQuestions>.Create(allQuestion, page, PAGE_SIZE);
            return _mapper.Map<List<ExamAndTestQuestionViewModel>>(result);
        }
        #endregion
        //POST
        #region
        public async Task<APIResponse> CreateQuestions(List<ExamAndTestQuestionCreateModel> models)
        {
            var user = await _context.Users.Include(role => role.UserRole)
              .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));
            if (!user.UserRole.ExamsAndTestsCreateNew)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "User dont have permission."
                };
            }
            foreach (var model in models)
            {
                // Check if SubjectId and FacultyId exist in the database
                var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.SubjectId.Equals(model.SubjectId));
                var faculty = await _context.Facultys.FirstOrDefaultAsync(f => f.FacultyId.Equals(model.FacultyId));

                if (subject == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = $"Invalid SubjectId for question: {model.ExamAndTestQuestionContent}"
                    };
                }

                if (faculty == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = $"Invalid FacultyId for question: {model.ExamAndTestQuestionContent}"
                    };
                }

                // Map ExamAndTestQuestionCreateModel to ExamAndTestQuestion entity
                var newQuestion = _mapper.Map<ExamAndTestQuestions>(model);
                newQuestion.ExamAndTestQuestionCode = $"MCH{_context.ExamAndTestQuestionss.Count() + 1:000}";
                await _context.AddAsync(newQuestion);
                newQuestion.CreatedDate = DateTime.Now;
                await _context.SaveChangesAsync();

                // Map and add answers to the question
                if (model.ExamAndTestAnswerCreateModels != null && model.ExamAndTestAnswerCreateModels.Any())
                {
                    foreach (var answerModel in model.ExamAndTestAnswerCreateModels)
                    {
                        var answer = new ExamAndTestAnswers
                        {
                            AnswerContent = answerModel.AnswerContent,
                            isAnswer = answerModel.isAnswer,
                            EaTQuestionId = newQuestion.EaTQuestionId
                        };

                        // Add answer to the context
                        await _context.ExamAndTestAnswerss.AddAsync(answer);
                    }
                }
            }
            await _context.SaveChangesAsync();
            return new APIResponse
            {
                Success = true,
                Message = "Questions created successfully."
            };
        }
        public async Task<APIResponse> CreateQuestion(ExamAndTestQuestionCreateModel model)
        {
            var user = await _context.Users.Include(role => role.UserRole)
              .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));
            if (!user.UserRole.ExamsAndTestsCreateNew)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "User dont have permission."
                };
            }
            // Check if SubjectId and FacultyId exist in the database
            var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.SubjectId.Equals(model.SubjectId));
            var faculty = await _context.Facultys.FirstOrDefaultAsync(f => f.FacultyId.Equals(model.FacultyId));

            if (subject == null)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Invalid SubjectId"
                };
            }

            if (faculty == null)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Invalid FacultyId"
                };
            }

            // Map ExamAndTestQuestionCreateModel to ExamAndTestQuestion entity
            var newQuestion = _mapper.Map<ExamAndTestQuestions>(model);
            newQuestion.ExamAndTestQuestionCode = $"MCH{_context.ExamAndTestQuestionss.Count() + 1:000}";
            newQuestion.CreatedDate = DateTime.Now;
            await _context.AddAsync(newQuestion);
            await _context.SaveChangesAsync();
            // Map and add answers to the question
            if (model.ExamAndTestAnswerCreateModels != null && model.ExamAndTestAnswerCreateModels.Any())
            {
                foreach (var answerModel in model.ExamAndTestAnswerCreateModels)
                {
                    var answer = new ExamAndTestAnswers
                    {
                        AnswerContent = answerModel.AnswerContent,
                        isAnswer = answerModel.isAnswer,
                        EaTQuestionId = newQuestion.EaTQuestionId
                    };

                    // Add answer to the context
                    await _context.ExamAndTestAnswerss.AddAsync(answer);
                }

                await _context.SaveChangesAsync();
            }
            return new APIResponse
            {
                Success = true,
                Message = "Created success."
            };
        }
        public async Task<byte[]> GenerateExamDocument(string facultyId, string subjectId, string made, string hinhthuc, string thoigianthi, int easy, int normal, int difficult)
        {
            byte[] result;
            var faculty = await _context.Facultys.FirstOrDefaultAsync(f => f.FacultyId.Equals(Guid.Parse(facultyId)));
            var subject = await _context.Subjects.FirstOrDefaultAsync(f => f.SubjectId.Equals(Guid.Parse(subjectId)));
            var question = await _context.ExamAndTestQuestionss
                    .Include(a => a.ExamAndTestAnswerss)
                    .Where(f => f.FacultyId.Equals(faculty.FacultyId))
                    .Where(f => f.SubjectId.Equals(subject.SubjectId))
                    .ToListAsync();
            
            var selectedEasyQuestions = question.Where(q => q.Tier == 1).Take(easy).ToList();
            var selectedNormalQuestions = question.Where(q => q.Tier == 2).Take(normal).ToList();
            var selectedDifficultQuestions = question.Where(q => q.Tier == 3).Take(difficult).ToList();
            var allQuestions = selectedEasyQuestions.Concat(selectedNormalQuestions).Concat(selectedDifficultQuestions).ToList();

            var random = new Random();
            var randomizedQuestions = allQuestions.OrderBy(x => random.Next()).ToList();

            using (MemoryStream stream = new MemoryStream())
            {
                //File.Copy(_templatePath, stream);
                // Mở tệp .dotx và sao chép dữ liệu vào MemoryStream
                using (FileStream fileStream = new FileStream(_templatePath, FileMode.Open))
                {
                    fileStream.CopyTo(stream);
                }
                // Đặt vị trí của MemoryStream về đầu
                stream.Position = 0;
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(stream, true))
                {
                    var body = wordDocument.MainDocumentPart.Document.Body;
                    var footer = wordDocument.MainDocumentPart.FooterParts;
                    // Thay thế các placeholder
                    ReplacePlaceholder(body, "<<khoa>>", faculty.FacultyName);
                    ReplacePlaceholder(body, "<<monthi>>", subject.SubjectName);
                    ReplacePlaceholder(body, "<<hinhthuc>>", hinhthuc);
                    ReplacePlaceholder(body, "<<thoigian>>", thoigianthi);
                    ReplacePlaceholder(body, "<<made>>", made);
                    //footer
                    ReplacePlaceholder(footer, "<<made>>", made);
                    // Thêm danh sách câu hỏi
                    AddQuestions(body, "<<danhsachcauhoi>>", _mapper.Map<List<ExamAndTestQuestion1ViewModel>>(randomizedQuestions));

                    wordDocument.MainDocumentPart.Document.Save();
                }

                result = stream.ToArray();
            }
            return result;
        }
        public async Task<APIResponse> GenerateExamExcel(string tendethi,string facultyId, string subjectId, string hinhthuc, string thoigianthi, int easy, int normal, int difficult)
        {
            var faculty = await _context.Facultys.FirstOrDefaultAsync(f => f.FacultyId.Equals(Guid.Parse(facultyId)));
            var subject = await _context.Subjects.FirstOrDefaultAsync(f => f.SubjectId.Equals(Guid.Parse(subjectId)));
            var question = await _context.ExamAndTestQuestionss
                    .Include(a => a.ExamAndTestAnswerss)
                    .Where(f => f.FacultyId.Equals(faculty.FacultyId))
                    .Where(f => f.SubjectId.Equals(subject.SubjectId))
                    .ToListAsync();

            var selectedEasyQuestions = question.Where(q => q.Tier == 1).Take(easy).ToList();
            var selectedNormalQuestions = question.Where(q => q.Tier == 2).Take(normal).ToList();
            var selectedDifficultQuestions = question.Where(q => q.Tier == 3).Take(difficult).ToList();
            var allQuestions = selectedEasyQuestions.Concat(selectedNormalQuestions).Concat(selectedDifficultQuestions).ToList();

            var random = new Random();
            var randomizedQuestions = allQuestions.OrderBy(x => random.Next()).ToList();
            //Excel
            // Tạo một MemoryStream để lưu trữ dữ liệu của tệp Excel
            using (MemoryStream memStream = new MemoryStream())
            {
                // Khởi tạo một SpreadsheetDocument mới
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(memStream, SpreadsheetDocumentType.Workbook))
                {
                    // Tạo một WorkbookPart
                    WorkbookPart workbookPart = document.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();

                    // Tạo một WorksheetPart
                    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    worksheetPart.Worksheet = new Worksheet(new SheetData());

                    // Tạo một Sheets để định nghĩa sheet trong Workbook
                    Sheets sheets = document.WorkbookPart.Workbook.AppendChild(new Sheets());
                    Sheet sheet = new Sheet() { Id = document.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "QuestionData" };
                    sheets.Append(sheet);

                    // Lấy ra SheetData từ WorksheetPart
                    SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
                    //Hàng chủ
                    // Tạo một hàng tiêu đề
                    Row headerRow = new Row();

                    // Thêm các cột tiêu đề
                    Cell cellCodeHeader = new Cell() { DataType = CellValues.String, CellValue = new CellValue("Mã câu hỏi") };
                    headerRow.Append(cellCodeHeader);

                    Cell cellTierHeader = new Cell() { DataType = CellValues.String, CellValue = new CellValue("Cấp độ") };
                    headerRow.Append(cellTierHeader);

                    Cell cellContentHeader = new Cell() { DataType = CellValues.String, CellValue = new CellValue("Câu hỏi") };
                    headerRow.Append(cellContentHeader);

                    Cell cellAnswerAHeader = new Cell() { DataType = CellValues.String, CellValue = new CellValue("Đáp án A") };
                    headerRow.Append(cellAnswerAHeader);

                    Cell cellAnswerBHeader = new Cell() { DataType = CellValues.String, CellValue = new CellValue("Đáp án B") };
                    headerRow.Append(cellAnswerBHeader);

                    Cell cellAnswerCHeader = new Cell() { DataType = CellValues.String, CellValue = new CellValue("Đáp án C") };
                    headerRow.Append(cellAnswerCHeader);

                    Cell cellAnswerDHeader = new Cell() { DataType = CellValues.String, CellValue = new CellValue("Đáp án D") };
                    headerRow.Append(cellAnswerDHeader);

                    // Thêm hàng tiêu đề vào SheetData
                    sheetData.Append(headerRow);
                    // Điền dữ liệu vào tệp Excel
                    foreach (var questionObj in randomizedQuestions)
                    {
                        // Tạo một hàng mới
                        Row row = new Row();
                        Cell cellCode = new Cell() { DataType = CellValues.String, CellValue = new CellValue(questionObj.ExamAndTestQuestionCode) };
                        row.Append(cellCode);
                        Cell cellTier = new Cell() { DataType = CellValues.String, CellValue = new CellValue(questionObj.Tier) };
                        row.Append(cellTier);
                        // Điền nội dung câu hỏi vào cột đầu tiên
                        Cell cell = new Cell() { DataType = CellValues.String, CellValue = new CellValue(questionObj.ExamAndTestQuestionContent) };
                        row.Append(cell);

                        // Điền nội dung đáp án vào các cột còn lại
                        foreach (var answer in questionObj.ExamAndTestAnswerss)
                        {
                            cell = new Cell() { DataType = CellValues.String, CellValue = new CellValue(answer.AnswerContent) };
                            row.Append(cell);
                        }

                        // Thêm hàng vào SheetData
                        sheetData.Append(row);
                    }
                }

                // Đặt vị trí đầu của MemoryStream về đầu
                memStream.Seek(0, SeekOrigin.Begin);
                long fileSize = memStream.Length;

                // Upload tệp Excel lên Cloudinary
                var uploadParams = new RawUploadParams
                {
                    File = new FileDescription("question_data.xlsx", memStream),
                    Folder = "resources_of_lms_folder/exam_and_test_folder"
                };
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                // Save file info to database
                var fileInfo = new ExamAndTest
                {
                    SubjectId = Guid.Parse(subjectId),
                    FacultyId = Guid.Parse(facultyId),
                    Time = thoigianthi,
                    ExamForm = hinhthuc,
                    Status = "Lưu nháp",
                    CreatedDate = DateTime.Now,
                    //file info
                    FileType = ".xlsx",
                    FileName = tendethi,
                    FileSize = $"{(double)fileSize / (1024 * 1024):0.#####} MB",
                    FileUrl = uploadResult.SecureUri.ToString(),
                    FileViewUrl = String.Concat("https://docs.google.com/viewer?url=", uploadResult.SecureUri.ToString())
                };

                _context.ExamAndTestS.Add(fileInfo);
                await _context.SaveChangesAsync();
                return new APIResponse
                {
                    Success = true,
                    Message = $"Upload file success.Link: {uploadResult.SecureUri.ToString()}"
                };
            }
        }
        #endregion
        //PUT
        #region
        public async Task<APIResponse> UpdateQuestion(string id, ExamAndTestQuestionUpdateModel model)
        {
            var user = await _context.Users.Include(role => role.UserRole)
              .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));
            if (!user.UserRole.ExamsAndTestsEdit)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "User dont have permission."
                };
            }
            var question = await _context.ExamAndTestQuestionss.FindAsync(int.Parse(id));
            if (question == null)
            {
                return new APIResponse { Success = false, Message = "Question not found" };
            }
            #region
            foreach (var property in typeof(ExamAndTestQuestionUpdateModel).GetProperties())
            {
                var modelValue = property.GetValue(model);
                if (modelValue != null)
                {
                    var questionProperty = typeof(ExamAndTestQuestions).GetProperty(property.Name);
                    if (questionProperty != null)
                    {
                        questionProperty.SetValue(question, modelValue);
                    }
                }
            }
            #endregion
            question.LastModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return new APIResponse { Success = true, Message = "Question updated successfully" };
        }
        #endregion
        //DELETE
        #region
        public async Task<APIResponse> DeleteQuestion(string id)
        {
            var user = await _context.Users.Include(role => role.UserRole)
              .FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));
            if (!user.UserRole.ExamsAndTestsDelete)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "User dont have permission."
                };
            }
            var question = await _context.ExamAndTestQuestionss.FindAsync(int.Parse(id));
            if (question == null)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Question not found."
                };
            }
            var allAnswer = await _context.ExamAndTestAnswerss
                .Where(q => q.EaTQuestionId.Equals(question.EaTQuestionId))
                .ToListAsync();
            _context.ExamAndTestAnswerss.RemoveRange(allAnswer);
            _context.ExamAndTestQuestionss.Remove(question);
            await _context.SaveChangesAsync();
            return new APIResponse
            {
                Success = false,
                Message = "Deleted."
            };
        }

       
        #endregion
    }
}
