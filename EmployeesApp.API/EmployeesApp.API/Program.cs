using EmployeesApp.API.ApplicationContext;
using EmployeesApp.API.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

builder.Services.AddTransient<UserRepository, UserRepository>();

var config = builder.Configuration.GetSection("AppSettings");
var key = Encoding.UTF8.GetBytes(config.GetSection("Secret").Value);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidAudience = config.GetSection("Audience").Value,
        ValidIssuer = config.GetSection("Issuer").Value,
        ValidateLifetime = true
    };
});
    
builder.Services.AddAuthorization();
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.Run();
