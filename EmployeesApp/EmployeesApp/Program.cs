using EmployeesApp.ApplicationContext;
using EmployeesApp.Repository;
using EmployeesApp.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IRepository<Employee>, EmployeeRepository>();
builder.Services.AddTransient<IRepository<Todo>, TodoRepository>();

var app = builder.Build();


app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
