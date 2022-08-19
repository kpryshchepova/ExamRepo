using EmployeesApp.ApplicationContext;
using EmployeesApp.Repository;
using EmployeesApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using EmployeesApp.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddHttpContextAccessor();

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


builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IRepository<Employee>, EmployeeRepository>();
builder.Services.AddTransient<IRepository<Todo>, TodoRepository>();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.UseMiddleware<AuthMiddleware>();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
