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
    public class IncomeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public IncomeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetIncomes()
        {
            var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (auth0Id == null)
                auth0Id = User.FindFirst("sub")?.Value;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Auth0Id == auth0Id);

            if (user == null)
                return new UnauthorizedResult();

            var incomes = _context.Incomes.Where(i => i.UserId == user.Id);

            var result = new RestDTO<Income[]> { Data = await incomes.ToArrayAsync(), };
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetIncomeById(int id)
        {
            var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (auth0Id == null)
                auth0Id = User.FindFirst("sub")?.Value;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Auth0Id == auth0Id);

            if (user == null)
                return new UnauthorizedResult();

            var income = await _context.Incomes.FirstOrDefaultAsync(i =>
                i.Id == id && i.UserId == user.Id
            );

            if (income != null)
                return Ok(income);

            return new NotFoundObjectResult(new { message = "That income doesn't exists" });
        }

        [HttpPost]
        public async Task<IActionResult> CreateIncome(IncomeDTO input)
        {
            var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (auth0Id == null)
                auth0Id = User.FindFirst("sub")?.Value;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Auth0Id == auth0Id);

            if (user == null)
                return new UnauthorizedResult();

            if (!ModelState.IsValid)
                return new BadRequestObjectResult(new { message = "That income is not valid" });

            Income income = new Income()
            {
                Name = input.Name,
                Category = input.Category,
                EarnedMoney = input.EarnedMoney,
                UserId = user.Id
            };

            await _context.Incomes.AddAsync(income);
            await _context.SaveChangesAsync();

            return new CreatedResult();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIncome(IncomeDTO input, int id)
        {
            var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (auth0Id == null)
                auth0Id = User.FindFirst("sub")?.Value;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Auth0Id == auth0Id);

            if (user == null)
                return new UnauthorizedResult();

            if (!ModelState.IsValid)
                return new BadRequestObjectResult(new { message = "That income is not valid" });

            var oldIncome = await _context.Incomes.FirstOrDefaultAsync(i =>
                i.Id == id && i.UserId == user.Id
            );

            if (oldIncome == null)
                return new NotFoundObjectResult(new { message = "That income doesn't exists" });

            oldIncome.Name = input.Name;
            oldIncome.EarnedMoney = input.EarnedMoney;
            oldIncome.Category = input.Category;

            _context.Incomes.Update(oldIncome);
            await _context.SaveChangesAsync();

            return new OkObjectResult(new { message = "Income updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncome(int id)
        {
            var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (auth0Id == null)
                auth0Id = User.FindFirst("sub")?.Value;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Auth0Id == auth0Id);

            if (user == null)
                return new UnauthorizedResult();

            if (!ModelState.IsValid)
                return new BadRequestObjectResult(new { message = "That income is not valid" });

            var income = await _context.Incomes.FirstOrDefaultAsync(i =>
                i.Id == id && i.UserId == user.Id
            );

            if (income == null)
                return new NotFoundObjectResult(new { message = "That income doesn't exists" });

            _context.Incomes.Remove(income);
            await _context.SaveChangesAsync();

            return new OkObjectResult(new { message = "Income deleted successfully" });
        }
    }
}
