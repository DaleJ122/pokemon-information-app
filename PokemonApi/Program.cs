using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PokemonApi.Infrastructure.Auth;
using PokemonApi.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlite(builder.Configuration.GetConnectionString("Default")));

var jwt = builder.Configuration.GetSection("Jwt");
builder.Services.AddSingleton<JwtIssuer>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["Key"]!))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddHttpClient("poke", c =>
    c.BaseAddress = new Uri("https://pokeapi.co/api/v2/"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type         = SecuritySchemeType.Http,
        Scheme       = "bearer",
        BearerFormat = "JWT",
        In           = ParameterLocation.Header,
        Description  = "Enter your JWT as: Bearer {token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


builder.Services.AddCors(o =>
    o.AddDefaultPolicy(p =>
        p.AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins("http://localhost:5173")));

var app = builder.Build();

app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/api/auth/login", (LoginDto dto, JwtIssuer issuer) =>
{
    if (dto.UserName != "admin" || dto.Password != "Pa$$w0rd!")
        return Results.Unauthorized();
    return Results.Ok(new { token = issuer.Issue(dto.UserName) });
});

app.MapGet("/api/pokemon", async (
        string term,
        AppDbContext db,
        IHttpClientFactory fac) =>
    {
        if (int.TryParse(term, out var id))
        {
            var e = await db.Pokemons.FindAsync(id);
            if (e is not null) return Results.Ok(e);
        }
        else
        {
            var e = await db.Pokemons.FirstOrDefaultAsync(p => p.Name == term);
            if (e is not null) return Results.Ok(e);
        }

        var client = fac.CreateClient("poke");
        var res = await client.GetAsync($"pokemon/{term.ToLower()}");
        if (!res.IsSuccessStatusCode) return Results.NotFound();

        using var doc = JsonDocument.Parse(await res.Content.ReadAsStringAsync());
        var r = doc.RootElement;

        var poke = new Pokemon(
            r.GetProperty("id").GetInt32(),
            r.GetProperty("name").GetString()!,
            r.GetProperty("sprites").GetProperty("front_default").GetString()!,
            r.GetProperty("height").GetInt32(),
            r.GetProperty("weight").GetInt32(),
            r.GetProperty("abilities")
                .EnumerateArray()
                .Select(x => x.GetProperty("ability").GetProperty("name").GetString()!)
                .ToArray(),
            r.GetProperty("types")
                .EnumerateArray()
                .Select(x => x.GetProperty("type").GetProperty("name").GetString()!)
                .ToArray()
        );

        db.Pokemons.Add(poke);
        await db.SaveChangesAsync();
        return Results.Ok(poke);
    })
    .RequireAuthorization();

app.Run("http://localhost:5100");

record LoginDto(string UserName, string Password);

public record Pokemon(
    int Id,
    string Name,
    string SpriteUrl,
    int Height,
    int Weight,
    string[] Abilities,
    string[] Types
);