using E_commerce_Admin_Dashboard.Helpers;
using E_commerce_Admin_Dashboard.Interfaces.Helpers;
using E_commerce_Admin_Dashboard.Interfaces.Mappers;
using E_commerce_Admin_Dashboard.Interfaces.Repos;
using E_commerce_Admin_Dashboard.Interfaces.Services;
using E_commerce_Admin_Dashboard.Mappers;
using E_commerce_Admin_Dashboard.Repositories;
using E_commerce_Admin_Dashboard.Services;
using Microsoft.EntityFrameworkCore;
using E_commerce_Admin_Dashboard.Middlewares;
using Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});

// Add services to the container.

builder.Services.AddHttpContextAccessor();

// Configure EF Core with connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Confiigure Default Classes for Interfaces
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IValidationService, ValidationService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddScoped<ICookiesHelper, CookiesHelper>();
builder.Services.AddScoped<IJwtHelper, JwtHelper>();
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

builder.Services.AddScoped<ICustomerMapper, CustomerMapper>();
builder.Services.AddScoped<IAdminMapper, AdminMapper>();
builder.Services.AddScoped<IGeneralMapper, GeneralMapper>();

builder.Services.AddScoped<AccessTokenValidator>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SuperAdminOnly", policy =>
        policy.RequireRole("super_admin"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseMiddleware<AccessTokenValidator>();

app.UseAuthorization();

app.MapControllers();

app.Run();
