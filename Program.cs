using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Producty.Middleware;
using Producty.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(opts =>
{
    opts.AddDefaultPolicy(cfg =>
    {
        cfg.WithHeaders(builder.Configuration["AllowedHeaders"]!);
        cfg.WithOrigins(builder.Configuration["AllowedOrigins"]!);
        cfg.WithMethods(builder.Configuration["AllowedMethods"]!);
    });

    opts.AddPolicy(
        name: "All Allowed",
        cfg =>
        {
            cfg.AllowAnyHeader();
            cfg.AllowAnyMethod();
            cfg.AllowAnyOrigin();
        }
    );
});

builder.Services.AddControllers(opts =>
{
    opts.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor(
        (x, y) => $"The value: '{x}' is not valid for '{y}'!"
    );
    opts.ModelBindingMessageProvider.SetValueIsInvalidAccessor(x => $"The value {x} is invalid!");
    opts.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(
        () => "A value is required to access this resource!"
    );
    opts.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(x => $"{x} must be a number!");
    opts.CacheProfiles.Add("NoCache", new CacheProfile() { NoStore = true, });
    opts.CacheProfiles.Add(
        "Any-60",
        new CacheProfile() { Duration = 60, Location = ResponseCacheLocation.Any }
    );
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");

builder.Services.AddDbContext<AppDbContext>(opts =>
{
    opts.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

var authAudience = builder.Configuration["Auth0:Audience"];
var authDomain = builder.Configuration["Auth0:Domain"];
builder
    .Services.AddAuthentication(opts =>
    {
        opts.DefaultAuthenticateScheme =
            opts.DefaultChallengeScheme =
            opts.DefaultForbidScheme =
            opts.DefaultScheme =
            opts.DefaultSignInScheme =
            opts.DefaultSignOutScheme =
                JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(opts =>
    {
        opts.Authority = $"https://{authDomain}";
        opts.Audience = authAudience;

        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"https://{authDomain}",
            ValidateAudience = true,
            ValidAudience = authAudience,
            ValidateLifetime = true,
        };
    });

builder.Services.AddAuthorization(opts =>
{
    opts.AddPolicy(
        "ApiScope",
        policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireClaim("scope", "read:messages");
        }
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}

app.UseMiddleware<RegisterUserMiddleware>();
app.UseHttpsRedirection();
app.UseCors();

app.UseResponseCaching();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireAuthorization("ApiScope");

app.Run();
