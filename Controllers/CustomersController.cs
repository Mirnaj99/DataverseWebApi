using DataverseWebApis.Helpers;
using DataverseWebApis.Models.Customers;
using DataverseWebApis.Services;
using Microsoft.AspNetCore.Mvc;

namespace DataverseWebApis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomersService _customersService;

        public CustomersController(ICustomersService customersService)
        {
            _customersService = customersService;
        }

        [HttpGet("GetCustomers")]
        public async Task<IActionResult> GetCustomers()
        {
            try
            {
                //Function that gets user by ID
                var result = await _customersService.GetCustomers();

                return Ok(result);
            }
            catch (AppException e)
            {
                return BadRequest(new { message = e.Message, error = e.InnerException });
            }
        }

        [HttpPost("AddCustomers")]
        public async Task<IActionResult> AddCustomers(AddCustomerModel model)
        {
            try
            {
                //Function that gets user by ID
                await _customersService.PostCustomers(model);

                return Ok();
            }
            catch (AppException e)
            {
                return BadRequest(new { message = e.Message, error = e.InnerException });
            }
        }
    }
    
}
