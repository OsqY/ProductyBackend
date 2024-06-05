using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Producty.Models;

namespace Producty.Middleware
{
    public class RegisterUserMiddleware
    {
        private readonly RequestDelegate _next;

        public RegisterUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, AppDbContext dbContext)
        {
            Console.WriteLine("Request started");

            if (!context.User.Identity.IsAuthenticated)
            {
                Console.WriteLine("User unauthenticated");
                throw new Exception("User unauthenticated");
            }
            else
            {
                var auth0Id = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(auth0Id))
                {
                    auth0Id = context.User.FindFirst("sub")?.Value;
                }

                Console.WriteLine($"Auth0Id: {auth0Id}");

                if (!string.IsNullOrEmpty(auth0Id))
                {
                    var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Auth0Id == auth0Id);
                    if (user == null)
                    {
                        user = new AppUser { Auth0Id = auth0Id, };

                        Console.WriteLine("User added");
                        dbContext.Users.Add(user);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
            await _next(context);
        }
    }
}
