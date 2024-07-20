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
    public class StudySessionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudySessionController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetStudySessions()
        {
            var auth0Id =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Auth0Id == auth0Id);

            if (user == null)
                return new UnauthorizedObjectResult(new { message = "That user doesn't exists" });

            var studySessions = _context.StudySessions.Where(s => s.UserId == user.Id);

            var results = new RestDTO<StudySession[]> { Data = await studySessions.ToArrayAsync() };

            return new OkObjectResult(results);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudySessionById(int id)
        {
            var auth0Id =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Auth0Id == auth0Id);

            if (user == null)
                return new UnauthorizedObjectResult(new { message = "That user doesn't exists" });

            var studySession = await _context.StudySessions.FirstOrDefaultAsync(s =>
                s.Id == id && s.UserId == user.Id
            );

            if (studySession != null)
                return Ok(studySession);

            return new NotFoundObjectResult(
                new { message = "Study Session with that id doesn't exists" }
            );
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudySession(StudySessionDTO input)
        {
            var auth0Id =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Auth0Id == auth0Id);

            if (user == null)
                return new UnauthorizedObjectResult(new { message = "That user doesn't exists" });

            if (!ModelState.IsValid)
                return new BadRequestObjectResult(
                    new { message = "That study session is not valid" }
                );

            StudySession studySession = new StudySession
            {
                Name = input.Name,
                StartTime = input.StartTime,
                EndTime = input.EndTime,
                UserId = user.Id,
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now
            };

            return new CreatedResult();
        }

        // [HttpPut]
        // public async Task<IActionResult> UpdateStudySession(StudySessionDTO input, int id) {
        //
        //     var auth0Id =
        //         User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
        //
        //     var user = await _context.Users.FirstOrDefaultAsync(u => u.Auth0Id == auth0Id);
        //
        //     if (user == null)
        //         return new UnauthorizedObjectResult(new { message = "That user doesn't exists" });
        //
        //     if (!ModelState.IsValid)
        //         return new BadRequestObjectResult(
        //             new { message = "That study session is not valid" }
        //         );
        //
        //
        //     var studySession = await _context.StudySessions.FirstOrDefaultAsync(s =>
        //         s.Id == id && s.UserId == user.Id
        //     );
        //
        //     if (studySession != null)
        //       studySession.
        //       studySession.LastModifiedDate = DateTime.Now
        //
        //     return new NotFoundObjectResult(
        //         new { message = "Study Session with that id doesn't exists" }
        //     );
        //
        // }
    }
}
