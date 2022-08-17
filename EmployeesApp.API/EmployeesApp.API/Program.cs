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
var key = Encoding.ASCII.GetBytes(config.GetSection("Secret").Value);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidAudience = config.GetSection("Audience").Value,
        ValidIssuer = config.GetSection("Issuer").Value
    };
});
    
builder.Services.AddAuthorization();

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

//app.MapGet("/", (ApplicationContext db) => db.UsersData.ToList());

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
