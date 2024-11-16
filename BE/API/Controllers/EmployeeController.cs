using BLL.Interface;
using DTO.Employee;
using DTO.Responses;
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

        [HttpDelete]
        public IActionResult Delete(string accountName)
        {
            BaseResponseModel result = employee.Delete(accountName);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public IActionResult Put(EmployeeRequestPutModule req)
        {
            BaseResponseModel result = employee.Put(req);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public IActionResult Post(EmployeeRequestModule req)
        {
            BaseResponseModel result = employee.Post(req);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        } 

        [HttpGet]
        public IActionResult Get(int? EmployeeTypeID, int? DepartmentID, int pageNumber = 1, int pageSize = 8)
        {
            BaseResponseModel result = employee.Get(EmployeeTypeID, DepartmentID, pageNumber, pageSize);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
