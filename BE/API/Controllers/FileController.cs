using DTO.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly string _targetFolder;

        public FileController()
        {
            // Đường dẫn thư mục lưu file vào thư mục Products của FE
            _targetFolder = Path.Combine("C:\\Users\\ADMIN\\Desktop\\ProjectCuNhan-WebsiteVina-\\FE\\src\\assets\\Products");
            //_targetFolder = Path.Combine("C:\\Users\\ADMIN\\Desktop\\ProjectCuNhan-WebsiteVina-\\FE\\public\\Products");

            // Tạo thư mục nếu chưa tồn tại
            if (!Directory.Exists(_targetFolder))
            {
                Directory.CreateDirectory(_targetFolder);
            }
        }

        [HttpPost("upload")]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new BaseResponseModel()
                {
                    IsSuccess = false,
                    Message = "No file was uploaded."
                });
            }

            // Đường dẫn đầy đủ để lưu file
            var filePath = Path.Combine(_targetFolder, file.FileName);

            // Lưu file vào thư mục
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new BaseResponseModel {
                Message = "File uploaded successfully!",
                Data = file.FileName,
                IsSuccess = true
            });
        }

        [HttpGet]        
        public IActionResult GetImage(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) {
                return BadRequest("??? TÊN FILE ĐÂU PRO!"); 
            }

            var filePate = Path.Combine(_targetFolder, fileName);

            if (!System.IO.File.Exists(filePate))
            {
                return BadRequest("Không Tìm Thấy");
            }

            var image = System.IO.File.OpenRead(filePate);

            return File(image, "image/png");
        }
        
    }
}
