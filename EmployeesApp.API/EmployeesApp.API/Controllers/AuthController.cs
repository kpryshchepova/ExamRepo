using EmployeesApp.API.Models;
using EmployeesApp.API.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EmployeesApp.API.Controllers;

[Route("api")]
[ApiController]
public class AuthController : ControllerBase
{
    private UserRepository _userRepository;
    private IConfiguration _configuration;
    public AuthController(UserRepository userRepository, IConfiguration config)
    {
        _userRepository = userRepository;
        _configuration = config;
    }

    [HttpPost("Login")]
    public async Task<ActionResult<string>> Login([FromBody]User request)
    {
        User user = await _userRepository.GetUserAuthDataByNameAsync(request.UserName);

        if(user == null)
        {
            return Unauthorized("User is not found");
        }
        
        if (user.UserName == request.UserName && user.Password != request.Password)
        {
            return BadRequest("Wrong password");
        }

        string token = CreateToken(user);
        return Ok(token);
    }

    [HttpPost("Register")]
    public async Task<ActionResult<User>> Register(User user)
    {

        User ifUserIsDuplicate = await _userRepository.GetUserAuthDataByNameAsync(user.UserName);
        if (ifUserIsDuplicate != null)
        {
            return Conflict("Such user name is already exists");
        }

        await _userRepository.CreateUserAsync(user);

        return Ok(user);
    }

    private string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim("id", user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
        };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Secret").Value));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

}

