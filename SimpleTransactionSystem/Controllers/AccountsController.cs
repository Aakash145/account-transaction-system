using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleTransactionSystem.Managers;

namespace SimpleTransactionSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountManager accountManager;

        public AccountController(IAccountManager accountManager)
        {
            this.accountManager = accountManager;
        }

        [HttpPost("transfer")]
        //Returns bad request with message if validation fails
        public IActionResult TransferFunds([FromBody] TransferRequest request)
        {
            try
            {
                accountManager.TransferFunds(request.FromAccountId, request.ToAccountId, request.Amount);
                return Ok("Funds transferred successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // Validation errors
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // Invalid operation errors
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpGet("history/{accountId}")]
        //returns 404 if there is error in transaction history
        public IActionResult GetTransactionHistory(int accountId)
        {
            try
            {
                var transactions = accountManager.GetTransactionHistory(accountId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
    }

    public class TransferRequest
    {
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public decimal Amount { get; set; }
    }

}
