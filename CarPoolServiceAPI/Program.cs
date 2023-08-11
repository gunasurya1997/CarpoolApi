using CarpoolService.BAL.Services;
using CarpoolService.DAL.Repositories;
using CarPoolService.DAL;
using CarPoolService.Models;
using CarPoolService.Models.Interfaces;
using CarPoolService.Models.Interfaces.Repository_Interfaces;
using CarPoolService.Models.Interfaces.Service_Interface;
using CarPoolServiceAPI.Mappings;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<CarpoolDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CarpoolCS")));
builder.Services.AddCors(cors => cors.AddPolicy("MyPolicy", policy =>
{
    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));
builder.Services.AddScoped<IUserService, UserService>();  //Register the interface and implementation
builder.Services.AddScoped<IBCrypt, BCryptAdapter>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICarpoolRepository, CarPoolRideRepository>();
builder.Services.AddScoped<ICarpoolService, CarpoolRideService>();
builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(MappingProfile), typeof(Program));

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
app.UseRouting(); // Add routing middleware
app.UseCors("MyPolicy"); // Enable CORS

app.UseAuthorization();

app.MapControllers();

app.Run();
