using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Producty.Middleware;
using Producty.Models;

JsonSerializerOptions options =
    new() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true };
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
        cfg.WithOrigins(builder.Configuration["AllowedOrigins"]!).AllowCredentials();
        cfg.WithMethods(builder.Configuration["AllowedMethods"]!);
    });
});

builder
    .Services.AddControllers(opts =>
    {
        opts.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor(
            (x, y) => $"The value: '{x}' is not valid for '{y}'!"
        );
        opts.ModelBindingMessageProvider.SetValueIsInvalidAccessor(x =>
            $"The value {x} is invalid!"
        );
        opts.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(
            () => "A value is required to access this resource!"
        );
        opts.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(x =>
            $"{x} must be a number!"
        );
        opts.CacheProfiles.Add("NoCache", new CacheProfile() { NoStore = true, });
        opts.CacheProfiles.Add(
            "Any-60",
            new CacheProfile() { Duration = 60, Location = ResponseCacheLocation.Any }
        );
    })
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        opts.JsonSerializerOptions.WriteIndented = true;
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
        opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
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

// builder.Services.AddAuthorization(opts =>
// {
//     opts.AddPolicy(
//         "ApiScope",
//         policy =>
//         {
//             policy.RequireAuthenticatedUser();
//             policy.RequireClaim("scope", "openid", "profile", "email");
//         }
//     );
// });
//
builder.Services.AddControllers();

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

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();

app.UseResponseCaching();

app.UseAuthentication();
app.UseMiddleware<RegisterUserMiddleware>();
app.UseAuthorization();

app.MapControllers();
app.Run();
