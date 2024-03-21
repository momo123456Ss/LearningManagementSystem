using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.ExamAndTestQuestionModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;

using Newtonsoft.Json;
using System.Text;
using System.Xml;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using CloudinaryDotNet.Core;

namespace LearningManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly Cloudinary _cloudinary;
        private readonly IWebHostEnvironment _environment;
        private readonly LearningManagementSystemContext _context;
        private readonly IMapper _mapper;
        private readonly string _templatePath = @"E:\.NET CORE API\LearningManagementSystem\LearningManagementSystem\LearningManagementSystem\Uploads\Files\Template\mau-de-thi-trac-nghiem-v2.docx";

        public TestController(Cloudinary cloudinary, IWebHostEnvironment environment, LearningManagementSystemContext context, IMapper mapper)
        {
            _cloudinary = cloudinary;
            _environment = environment;
            _context = context;
            _mapper = mapper;
        }
        private string SanitizeXmlString(string xmlString)
        {
            // Loại bỏ các ký tự không hợp lệ từ chuỗi XML
            StringBuilder sb = new StringBuilder();
            foreach (char c in xmlString)
            {
                if (XmlConvert.IsXmlChar(c))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        private void GenerateWordFromTemplate(List<ExamAndTestQuestion1ViewModel> questions, string outputPath)
        {

            using (WordprocessingDocument doc = WordprocessingDocument.Create(outputPath, WordprocessingDocumentType.Document))
            {

                // Tạo body cho tài liệu
                doc.AddMainDocumentPart();
                doc.MainDocumentPart.Document = new Document();
                Body body = doc.MainDocumentPart.Document.AppendChild(new Body());

                // Đọc template và thay thế dữ liệu
                #region
                using (var templateStream = System.IO.File.OpenRead(_templatePath))
                {
                    body.InnerXml = new StreamReader(templateStream).ReadToEnd();

                    var danhSachCauHoiContentControl = body.Descendants<SdtElement>().FirstOrDefault(sdt => sdt.SdtProperties.GetFirstChild<Tag>().Val == "danh_sach_cau_hoi");
                    if (danhSachCauHoiContentControl != null)
                    {
                        var danhSachCauHoiContent = danhSachCauHoiContentControl.SdtProperties;
                        foreach (var question in questions)
                        {
                            string sanitizedQuestion = question.ExamAndTestQuestionContent;
                            Paragraph questionParagraph = new Paragraph(new Run(new Text(sanitizedQuestion)));
                            danhSachCauHoiContent.AppendChild(questionParagraph);

                            foreach (var answer in question.ExamAndTestAnswerss)
                            {
                                string sanitizedAnswer = answer.AnswerContent;
                                Paragraph answerParagraph = new Paragraph(new Run(new Text(sanitizedAnswer)));
                                danhSachCauHoiContent.AppendChild(answerParagraph);
                            }
                        }
                    }
                }
                #endregion
                #region
                //var templateStream = System.IO.File.OpenRead(_templatePath);
                //body.InnerXml = new StreamReader(templateStream).ReadToEnd();

                //var danhSachCauHoiContentControl = body.Descendants<SdtElement>().FirstOrDefault(sdt => sdt.SdtProperties.GetFirstChild<Tag>().Val == "[danh_sach_cau_hoi]");
                //if (danhSachCauHoiContentControl != null)
                //{
                //    var danhSachCauHoiContent = danhSachCauHoiContentControl.SdtProperties;
                //    foreach (var question in questions)
                //    {
                //        string sanitizedQuestion = question.ExamAndTestQuestionContent;
                //        Paragraph questionParagraph = new Paragraph(new Run(new Text(sanitizedQuestion)));
                //        danhSachCauHoiContent.AppendChild(questionParagraph);

                //        foreach (var answer in question.ExamAndTestAnswerss)
                //        {
                //            string sanitizedAnswer = answer.AnswerContent;
                //            Paragraph answerParagraph = new Paragraph(new Run(new Text(sanitizedAnswer)));
                //            danhSachCauHoiContent.AppendChild(answerParagraph);
                //        }
                //    }
                //}

                //templateStream.Close();
                #endregion

                doc.Dispose();
            }

        }
        [HttpGet("CreateTest/{facultyId}/subject/{subjectId}")]
        public IActionResult CreateTest(string facultyId, string subjectId)
        {
            try
            {
                var faculty = _context.Facultys.Find(Guid.Parse(facultyId));
                var subject = _context.Subjects.Find(Guid.Parse(subjectId));
                var question = _context.ExamAndTestQuestionss
                    .Include(a => a.ExamAndTestAnswerss)
                    .Where(f => f.FacultyId.Equals(faculty.FacultyId))
                    .Where(f => f.SubjectId.Equals(subject.SubjectId))
                    .ToList();
                // Đảo ngẫu nhiên thứ tự của danh sách câu hỏi
                var random = new Random();
                var randomizedQuestions = question.OrderBy(x => random.Next()).ToList();
                // Lấy 10 câu hỏi đầu tiên
                var selectedQuestions = randomizedQuestions.Take(10).ToList();
                string jsonData = JsonConvert.SerializeObject(_mapper.Map<List<ExamAndTestQuestion1ViewModel>>(selectedQuestions));
                var data = JsonConvert.DeserializeObject<List<ExamAndTestQuestion1ViewModel>>(jsonData);

                string outputPath = @"E:\.NET CORE API\LearningManagementSystem\LearningManagementSystem\LearningManagementSystem\Uploads\Files\Template\Output\output_path_for_generated_file.docx";
                GenerateWordFromTemplate(data, outputPath);
                return PhysicalFile(outputPath, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "exam.docx");

                // Đảm bảo tệp đã được đóng trước khi đọc nó
                //byte[] fileBytes;
                //using (FileStream fileStream = System.IO.File.Open(outputPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                //{
                //    fileBytes = new byte[fileStream.Length];
                //    fileStream.Read(fileBytes, 0, (int)fileStream.Length);
                //    return File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "exam.docx");
                //}
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadPdf(IFormFile file)
        {
            try
            {
                // Check if the file exists
                if (file == null || file.Length == 0)
                    return BadRequest("No file uploaded.");

                // Check if the uploaded file is a PDF
                //if (file.ContentType != "application/pdf")
                //    return BadRequest("Invalid file format. Only PDF files are allowed.");

                // Initialize Cloudinary upload parameters
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream()),
                    Folder = "lecture_resoucer_folder" // Specify the folder in Cloudinary where you want to store the PDF files
                };

                // Upload the PDF file to Cloudinary
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                // Return the URL of the uploaded PDF file
                return Ok(new { PdfUrl = uploadResult.SecureUri.AbsoluteUri });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error uploading PDF file to Cloudinary: {ex.Message}");
            }
        }
        [HttpPost("uploadAllType")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            try
            {
                string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                string fileExtension = Path.GetExtension(file.FileName);

                // Kiểm tra phần mở rộng của tệp
                //List<string> allowedExtensions = new List<string> { ".mp4", ".docx", ".xlsx", ".pptx" };
                //if (!allowedExtensions.Contains(fileExtension))
                //    return BadRequest("File type not supported.");

                // Lưu file tạm thời
                string filePath = Path.Combine(_environment.ContentRootPath, "Uploads\\Files\\TempFiles", file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Upload file lên Cloudinary
                var uploadParams = new RawUploadParams()
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream()),
                    Folder = "lecture_resoucer_folder"
                };
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                // Xóa file tạm thời sau khi đã upload lên Cloudinary
                System.IO.File.Delete(filePath);

                return Ok(new { uploadResult.PublicId, uploadResult.Url });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
