using BLL.Interface;
using DTO.CustomerType;
using DTO.Responses;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerTypeController : ControllerBase
    {
        private readonly ICustomerType customerType;

        public CustomerTypeController(ICustomerType customerType)
        {
            this.customerType = customerType;
        }

        [HttpGet]
        public IActionResult Get(int? customerID)
        {
            BaseResponseModel result = customerType.Get(customerID);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CustomerTypeRequestModule req)
        {
            BaseResponseModel result = customerType.Post(req);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public IActionResult Put([FromBody]CustomerTypeResponseModule req)
        {
            BaseResponseModel result = customerType.Put(req);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpDelete]
        public IActionResult Delete(int? customerID)
        {
            BaseResponseModel result = customerType.Delete(customerID);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
