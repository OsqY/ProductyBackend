using System.Linq.Dynamic.Core;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Producty.DTO;
using Producty.Models;

namespace Producty.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ExpenseController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetExpenses()
        {
            var auth0Id =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Auth0Id == auth0Id);

            if (user == null)
                return new UnauthorizedObjectResult(new { message = "That user doesn't exists" });

            var expenses = await _context.Expenses.Where(e => e.UserId == user.Id).ToArrayAsync();
            var results = new RestDTO<Expense[]> { Data = expenses };

            return Ok(results);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExpense(int id)
        {
            var auth0Id =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Auth0Id == auth0Id);

            if (user == null)
                return new UnauthorizedObjectResult(new { message = "That user doesn't exists" });

            var expense = await _context.Expenses.FirstOrDefaultAsync(e =>
                e.Id == id && e.UserId == user.Id
            );

            if (expense == null)
                return new NotFoundObjectResult(new { message = "That expense doesn't exists" });

            return Ok(expense);
        }

        [HttpPost]
        public async Task<IActionResult> CreateExpense(ExpenseDTO input)
        {
            var auth0Id =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Auth0Id == auth0Id);

            if (user == null)
                return new UnauthorizedObjectResult(new { message = "That user doesn't exists" });

            if (!ModelState.IsValid)
                return new BadRequestObjectResult(new { message = "That expense is invalid" });

            Expense expense = new Expense
            {
                Name = input.Name,
                Category = input.Category,
                SpentMoney = input.SpentMoney,
                UserId = user.Id
            };

            await _context.Expenses.AddAsync(expense);
            await _context.SaveChangesAsync();

            return new CreatedResult();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpense(ExpenseDTO input, int id)
        {
            var auth0Id =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Auth0Id == auth0Id);

            if (user == null)
                return new UnauthorizedObjectResult(new { message = "That user doesn't exists" });

            if (!ModelState.IsValid)
                return new BadRequestObjectResult(new { message = "That expense is invalid" });

            var oldExpense = await _context.Expenses.FirstOrDefaultAsync(e =>
                e.Id == id && e.UserId == user.Id
            );

            if (oldExpense == null)
                return new NotFoundObjectResult(new { message = "That expense doesn't exists" });

            oldExpense.Name = input.Name;
            oldExpense.Category = input.Category;
            oldExpense.SpentMoney = input.SpentMoney;
            oldExpense.LastModifiedDate = DateTime.Now;

            return new OkObjectResult(new { message = "Expense updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var auth0Id =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Auth0Id == auth0Id);

            if (user == null)
                return new UnauthorizedObjectResult(new { message = "That user doesn't exists" });

            var expense = await _context.Expenses.FirstOrDefaultAsync(e =>
                e.Id == id && e.UserId == user.Id
            );

            if (expense == null)
                return new NotFoundObjectResult(new { message = "That expense doesn't exists" });

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return new OkObjectResult(new { message = "Expense deleted successfully" });
        }
    }
}
