using CarpoolService.BAL.Services;
using CarpoolService.DAL.Repositories;
using CarPoolService.DAL;
using CarPoolService.Models.Interfaces.Repository_Interfaces;
using CarPoolService.Models.Interfaces.Service_Interface;
using CarPoolServiceAPI.Mappings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<CarpoolDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CarpoolCS")));
builder.Services.AddCors(cors => cors.AddPolicy("MyPolicy", policy =>
{
    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));
builder.Services.AddAuthentication(configureOptions: options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
builder.Services.AddScoped<IUserService, UserService>();  
builder.Services.AddScoped<IBCryptService, BCryptAdapterService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICarpoolRepository, CarPoolRideRepository>();
builder.Services.AddScoped<ICarpoolService, CarpoolRideService>();
builder.Services.AddScoped<ITokenService,TokenService>();      
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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
