using BLL.Interface;
using DTO.Customer;
using DTO.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomer customer;

        public CustomerController(ICustomer customer)
        {
            this.customer = customer;
        }

        [HttpDelete]
        [Authorize(Roles = "Moderator")]
        public ActionResult Delete(string AccountName)
        {
            BaseResponseModel result = customer.Delete(AccountName);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        [Authorize(Roles = "Moderator")]
        public IActionResult Put(CustomerRequestModule req)
        {
            BaseResponseModel result = customer.Put(req);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [Authorize(Roles = "Moderator")]
        public IActionResult Get(
            int? TypeCustomerId,
            int pageNumber = 1,
            int pageSize = 8
        )
        {
            BaseResponseModel result = customer.Get(TypeCustomerId, pageNumber, pageSize);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
