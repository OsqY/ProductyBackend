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
    public class TodoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TodoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Auth0Id == auth0Id);

            if (string.IsNullOrEmpty(auth0Id))
            {
                auth0Id = User.FindFirst("sub")?.Value;
            }

            if (user == null)
            {
                return new UnauthorizedResult();
            }

            if (!ModelState.IsValid)
            {
                return new BadRequestResult();
            }

            var todos = _context.Todos.Where(t => t.UserId == user.Id).AsQueryable();

            // if (!string.IsNullOrEmpty(input.FilterQuery))
            //     todos = todos.Where(t => t.Name.Contains(input.FilterQuery));

            var recordCount = await todos.CountAsync();
            // todos = todos
            //     .OrderBy($"{input.SortColumn} {input.SortOrder}")
            //     .Skip(input.PageIndex * input.PageSize)
            //     .Take(input.PageSize);
            //
            var result = new RestDTO<Todo[]> { Data = await todos.ToArrayAsync(), };

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodo(int id)
        {
            var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (auth0Id == null)
                auth0Id = User.FindFirst("sub")?.Value;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Auth0Id == auth0Id);

            if (user == null)
                return new UnauthorizedResult();

            var todo = await _context.Todos.FirstOrDefaultAsync(t =>
                t.Id == id && t.UserId == user.Id
            );

            if (todo != null)
                return Ok(todo);

            return new NotFoundResult();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodo(TodoDTO input)
        {
            var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(auth0Id))
                auth0Id = User.FindFirst("sub")?.Value;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Auth0Id == auth0Id);
            Console.WriteLine(auth0Id);

            if (user == null)
                return new UnauthorizedResult();

            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid todo item" });

            Todo todo = new Todo
            {
                Name = input.Name,
                Description = input.Description == null ? "" : input.Description,
                DeadLine = DateTime.Parse(input.DeadLine),
                StartTime = DateTime.Parse(input.StartTime),
                IsCompleted = input.IsCompleted,
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                UserId = user.Id
            };
            await _context.Todos.AddAsync(todo);
            await _context.SaveChangesAsync();

            return new CreatedResult();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(TodoDTO input, int id)
        {
            var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(auth0Id))
                auth0Id = User.FindFirst("sub")?.Value;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Auth0Id == auth0Id);

            if (user == null)
                return new UnauthorizedResult();

            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid todo item" });

            var oldTodo = await _context.Todos.FirstOrDefaultAsync(t =>
                t.Id == id && t.UserId == user.Id
            );

            if (oldTodo == null)
                return new NotFoundObjectResult(new { message = "That todo doesn't exists" });

            oldTodo.Name = input.Name;
            oldTodo.Description = input.Description;
            oldTodo.StartTime = DateTime.Parse(input.StartTime);
            oldTodo.DeadLine = DateTime.Parse(input.DeadLine);
            oldTodo.IsCompleted = input.IsCompleted;
            oldTodo.LastModifiedDate = DateTime.Now;

            _context.Update(oldTodo);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (auth0Id == null)
                auth0Id = User.FindFirst("sub")?.Value;

            Console.WriteLine(auth0Id);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Auth0Id == auth0Id);

            if (user == null)
                return new UnauthorizedResult();

            Console.WriteLine(id);

            var oldTodo = await _context.Todos.FirstOrDefaultAsync(t =>
                t.Id == id && t.UserId == user.Id
            );

            if (oldTodo != null)
            {
                _context.Remove(oldTodo);
                await _context.SaveChangesAsync();
                return Ok();
            }

            return new NotFoundResult();
        }
    }
}
