using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace LearningManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly Cloudinary _cloudinary;
        private readonly IWebHostEnvironment _environment;

        public TestController(Cloudinary cloudinary, IWebHostEnvironment environment)
        {
            _cloudinary = cloudinary;
            _environment = environment;
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
