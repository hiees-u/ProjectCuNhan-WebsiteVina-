using BLL.Interface;
using DTO.Department;
using DTO.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartment department;

        public DepartmentController(IDepartment department)
        {
            this.department = department;
        }

        [HttpPost]
        [Authorize(Roles = "Moderator")]
        public IActionResult Post([FromBody] string departmentName)
        {
            BaseResponseModel result = department.Post(departmentName);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [Authorize(Roles = "Moderator")]
        public IActionResult Get(int pageNumber, int pageSize)
        {
            BaseResponseModel result = department.Get(pageNumber, pageSize);

            return result.IsSuccess ? 
                Ok(result) :
                BadRequest(result);
        }

        [HttpPut]
        [Authorize(Roles = "Moderator")]
        public IActionResult Put([FromBody] DepartmentRequestModule req)
        {
            BaseResponseModel result = department.Put(req);

            return result.IsSuccess ?
                Ok(result) :
                BadRequest(result);
        }

        [HttpDelete]
        [Authorize(Roles = "Moderator")]
        public IActionResult Delete(int depID)
        {
            BaseResponseModel result = department.Delete(depID);

            return result.IsSuccess ?
                Ok(result) :
                BadRequest(result);
        }
    }
}
