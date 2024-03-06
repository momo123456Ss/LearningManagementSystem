using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly Cloudinary _cloudinary;
        public TestController(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
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
                if (file.ContentType != "application/pdf")
                    return BadRequest("Invalid file format. Only PDF files are allowed.");

                // Initialize Cloudinary upload parameters
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream()),
                    Folder = "pdf_folder" // Specify the folder in Cloudinary where you want to store the PDF files
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
    }
}
