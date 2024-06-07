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
    public class JournalEntryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public JournalEntryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetJournalEntries()
        {
            var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (auth0Id == null)
                auth0Id = User.FindFirst("sub")?.Value;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Auth0Id == auth0Id);

            if (user == null)
                return new UnauthorizedResult();

            var journalEntries = _context.JournalEntries.Where(j => j.UserId == user.Id);

            var result = new RestDTO<JournalEntry[]>()
            {
                Data = await journalEntries.ToArrayAsync()
            };

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetJournal(int id)
        {
            var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (auth0Id == null)
                auth0Id = User.FindFirst("sub")?.Value;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Auth0Id == auth0Id);

            if (user == null)
                return new UnauthorizedResult();

            var journalEntry = await _context.JournalEntries.FirstOrDefaultAsync(j =>
                j.Id == id && j.UserId == user.Id
            );

            if (journalEntry == null)
                return new NotFoundObjectResult(
                    new { message = "That journal entry doesn't exists" }
                );

            return Ok(journalEntry);
        }

        [HttpPost]
        public async Task<IActionResult> CreateJournalEntry(JournalEntryDTO input)
        {
            var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (auth0Id == null)
                auth0Id = User.FindFirst("sub")?.Value;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Auth0Id == auth0Id);

            if (user == null)
                return new UnauthorizedResult();

            if (!ModelState.IsValid)
                return new BadRequestObjectResult(
                    new { message = "That journal entry is not valid." }
                );

            JournalEntry journalEntry = new JournalEntry()
            {
                Name = input.Name,
                Description = input.Description,
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                UserId = user.Id
            };

            await _context.JournalEntries.AddAsync(journalEntry);
            await _context.SaveChangesAsync();

            return new CreatedResult();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJournalEntry(JournalEntryDTO input, int id)
        {
            var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (auth0Id == null)
                auth0Id = User.FindFirst("sub")?.Value;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Auth0Id == auth0Id);

            if (user == null)
                return new UnauthorizedResult();

            if (!ModelState.IsValid)
                return new BadRequestObjectResult(
                    new { message = "That journal entry is not valid" }
                );

            var oldJournalEntry = await _context.JournalEntries.FirstOrDefaultAsync(j =>
                j.Id == id && j.UserId == user.Id
            );

            if (oldJournalEntry == null)
                return new NotFoundObjectResult(
                    new { message = "That journal entry doesn't exists" }
                );

            oldJournalEntry.Name = input.Name;
            oldJournalEntry.Description = input.Description;
            oldJournalEntry.LastModifiedDate = DateTime.Now;

            _context.Update(oldJournalEntry);
            await _context.SaveChangesAsync();

            return new OkObjectResult(new { message = "Journal entry updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJournalEntry(int id)
        {
            var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (auth0Id == null)
                auth0Id = User.FindFirst("sub")?.Value;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Auth0Id == auth0Id);

            if (user == null)
                return new UnauthorizedResult();

            var journalEntry = await _context.JournalEntries.FirstOrDefaultAsync(j =>
                j.Id == id && j.UserId == user.Id
            );

            if (journalEntry == null)
                return new BadRequestObjectResult(
                    new { message = "That journalEntry doesn't exists" }
                );

            _context.Remove(journalEntry);
            await _context.SaveChangesAsync();

            return new OkObjectResult(new { message = "Journal entry deleted successfully" });
        }
    }
}
