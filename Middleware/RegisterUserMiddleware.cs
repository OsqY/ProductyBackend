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
            if (context.User.Identity.IsAuthenticated)
            {
                var auth0Id = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(auth0Id))
                {
                    var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Auth0Id == auth0Id);
                    if (user == null)
                    {
                        new AppUser
                        {
                            Auth0Id = auth0Id,
                            Name = context.User.Identity.Name,
                            Email = context.User.FindFirst(ClaimTypes.Email)?.Value
                        };

                        dbContext.Users.Add(user);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
            await _next(context);
        }
    }
}
