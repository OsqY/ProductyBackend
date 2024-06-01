using System.Linq.Dynamic.Core;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Producty.DTO;
using Producty.Models;

namespace Producty.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TodoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetTodo([FromQuery] RequestDTO input)
        {
            var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Auth0Id == auth0Id);

            if (user == null)
            {
                return new UnauthorizedResult();
            }

            if (!ModelState.IsValid)
            {
                return new BadRequestResult();
            }
            var todos = _context.Todos.Where(t => t.UserId == user.Id).AsQueryable();

            if (!string.IsNullOrEmpty(input.FilterQuery))
                todos = todos.Where(t => t.Name.Contains(input.FilterQuery));

            var recordCount = await todos.CountAsync();
            todos = todos
                .OrderBy($"{input.SortColumn} {input.SortOrder}")
                .Skip(input.PageIndex * input.PageSize)
                .Take(input.PageSize);

            var result = new RestDTO<Todo[]> { Data = await todos.ToArrayAsync(), };

            return Ok(result);
        }
    }
}
