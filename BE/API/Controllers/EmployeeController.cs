using BLL.Interface;
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
        public IActionResult Get(int? departmentID = null, int? employeeTypeID = null)
        {
            BaseResponseModel result = employee.Get(departmentID, employeeTypeID);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
