using AuthService.Api.Models;
using AuthService.Domain;
using AuthService.Infrastructure;
using Bogus;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<Faker<User>, UserFaker>();
builder.Services.AddSingleton<IUserRepository, FakeUserRepository>();
builder.Services.AddSingleton<IAuthService, MyAuthService>();
builder.Services.AddSingleton<ITokenService, JwtTokenService>();

var app = builder.Build();

app.MapPost("api/token/create", (AuthModel model, IAuthService authService, ITokenService tokenService) =>
{
    if (authService.TryAuthorize(model.Login, model.Password, out User user))
    {
        var token = tokenService.Create(user);

        return Results.Ok(token);
    }
    else
    {
        return Results.BadRequest(new { message = "Username or password is incorrect." });
    }

});

app.Run();
