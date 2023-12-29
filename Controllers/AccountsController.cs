using DataverseWebApis.Helpers;
using DataverseWebApis.Services;
using Microsoft.AspNetCore.Mvc;

namespace DataverseWebApis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController: ControllerBase
    {
        private readonly IAccountService _accountService;
        
        public AccountsController (IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("GetAccountsCases")]
        public async Task<IActionResult> GetAccountsCases()
        {
            try
            {
                //Function that gets user by ID
                var result = await _accountService.GetAccountsCases();

                return Ok(result);
            }
            catch (AppException e)
            {
                return BadRequest(new { message = e.Message, error = e.InnerException });
            }
        }
    }
}
