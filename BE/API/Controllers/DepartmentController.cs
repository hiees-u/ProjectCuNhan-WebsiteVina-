using BLL.Interface;
using DTO.Department;
using DTO.Responses;
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
        public IActionResult Post([FromBody] string departmentName)
        {
            BaseResponseModel result = department.Post(departmentName);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        public IActionResult Get()
        {
            BaseResponseModel result = department.Get();

            return result.IsSuccess ? 
                Ok(result) :
                BadRequest(result);
        }

        [HttpPut]
        public IActionResult Put([FromBody] DepartmentRequestModule req)
        {
            BaseResponseModel result = department.Put(req);

            return result.IsSuccess ?
                Ok(result) :
                BadRequest(result);
        }

        [HttpDelete]
        public IActionResult Delete(int depID)
        {
            BaseResponseModel result = department.Delete(depID);

            return result.IsSuccess ?
                Ok(result) :
                BadRequest(result);
        }
    }
}
