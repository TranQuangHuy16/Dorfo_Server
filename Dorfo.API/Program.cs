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
using StackExchange.Redis;
using Net.payOS;


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

var payosConfig = builder.Configuration.GetSection("PayOS");
builder.Services.AddSingleton(sp => new PayOS(
                payosConfig["ClientId"] ?? throw new Exception("Missing ClientId"),
                payosConfig["ApiKey"] ?? throw new Exception("Missing ApiKey"),
                payosConfig["ChecksumKey"] ?? throw new Exception("Missing ChecksumKey")
            ));

// Bind Smtp config
builder.Services.Configure<SmtpSettings>(
    builder.Configuration.GetSection("Smtp"));

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()   // Cho phép tất cả domain
                  .AllowAnyMethod()   // Cho phép tất cả method (GET, POST, PUT, DELETE...)
                  .AllowAnyHeader();  // Cho phép tất cả header
        });
});


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

    c.MapType<TimeSpan>(() => new Microsoft.OpenApi.Models.OpenApiSchema
    {
        Type = "string",
        Example = new Microsoft.OpenApi.Any.OpenApiString("08:00:00"),
        Format = "time"
    });

    c.MapType<TimeSpan?>(() => new Microsoft.OpenApi.Models.OpenApiSchema
    {
        Type = "string",
        Example = new Microsoft.OpenApi.Any.OpenApiString("08:00:00"),
        Format = "time"
    });
});

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var config = builder.Configuration.GetSection("Redis");
    var connectionString = $"{config["Host"]}:{config["Port"]}";
    return ConnectionMultiplexer.Connect(connectionString);
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});


builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

var app = builder.Build();

// Auto migrate khi khởi động
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DorfoDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors("AllowFrontend");

app.UseAuthentication();  // phải trước UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();
