using Microsoft.EntityFrameworkCore;
using Dorfo.Infrastructure.Persistence;
using Dorfo.Application.Interfaces.Services;
using Dorfo.Application.Services;
using Dorfo.Application.Interfaces.Repositories;
using Dorfo.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Dorfo.API.Middlewares;
using Dorfo.Application.Mappings;
using Microsoft.Extensions.DependencyInjection;
using Dorfo.Infrastructure.Configurations;


var builder = WebApplication.CreateBuilder(args);

// Add Authentication & Authorization
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddInfrastructure(builder.Configuration);

// Bind Smtp config
builder.Services.Configure<SmtpSettings>(
    builder.Configuration.GetSection("Smtp"));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Dorfo API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,   // ✅ dùng Http
        Scheme = "bearer",                                         // ✅ phải có bearer
        BearerFormat = "JWT",                                      // ✅ format JWT
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter JWT token with **Bearer** prefix. Example: `Bearer {your token}`"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"   // phải trùng với tên ở trên
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});


builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();  // phải trước UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();
