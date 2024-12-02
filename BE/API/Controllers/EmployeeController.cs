using BLL.Interface;
using DTO.Employee;
using DTO.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployee employee;

        public EmployeeController(IEmployee employee)
        {
            this.employee = employee;
        }

        [HttpGet]
        [Authorize(Roles = "Moderator")]
        public IActionResult Get(int pageNumber, int pageSize ,int? departmentID = null, int? employeeTypeID = null)
        {
            BaseResponseModel result = employee.Get(departmentID, employeeTypeID, pageNumber, pageSize);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        [Authorize(Roles = "Moderator")]
        public IActionResult Post(EmployeeRequestPostModule req)
        {
            BaseResponseModel result = employee.Post(req);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpDelete]
        [Authorize(Roles = "Moderator")]
        public IActionResult Delete(string accountName)
        {
            BaseResponseModel result = employee.Delete(accountName);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        [Authorize(Roles = "Moderator")]
        public IActionResult Put([FromBody] EmployeeRequestPutModule req)
        {
            BaseResponseModel result = employee.Put(req);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
