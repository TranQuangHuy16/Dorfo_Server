using Microsoft.EntityFrameworkCore;
using Dorfo.Infrastructure.Persistence;
using Dorfo.Application.Interfaces.Services;
using Dorfo.Application.Services;
using Dorfo.Application.Interfaces.Repositories;
using Dorfo.Infrastructure.Persistence.Repositories;
using Dorfo.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add DbContext
//builder.Services.AddDbContext<DorfoDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DorfoDb")));
//// Đăng ký Repository
//builder.Services.AddScoped<IUserRepository, UserRepository>();

//// Đăng ký Service
//builder.Services.AddScoped<IUserService, UserService>(); 

builder.Services.AddInfrastructure(builder.Configuration);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
