using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SimpleChatServer.Configuration;
using SimpleChatServer.Models;
using SimpleChatServer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// configuration services
builder.Services
    .Configure<JwtConfig>(builder.Configuration.GetSection("JWT"))
    .Configure<SuperAdministratorConfig>(builder.Configuration.GetSection("SuperAdministrator"));

// JWT
builder.Services.AddAuthentication(
    options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(jwt =>
    {
        var secret = 
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"] ?? JwtConfig.DefaultSecret);

        jwt.SaveToken = true;
        jwt.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secret),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            RequireExpirationTime = true,
        };
    });

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CORS",
        policyBuilder =>
        {
            string[] origins =
                builder.Configuration.GetSection("CORS:Origins").Get<string[]>() ?? Array.Empty<string>();

            policyBuilder
                .AllowCredentials()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed(_ => true);
        });
});

// Server event
builder.Services.AddSingleton<EventService>();

// database
builder.Services.AddDbContext<ChatDbContext>(
    option =>
    {
        option.UseSqlite(builder.Configuration.GetConnectionString("SQLite"));
    });

//builder.Services.Add

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CORS");

app.UseStaticFiles("/static");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
