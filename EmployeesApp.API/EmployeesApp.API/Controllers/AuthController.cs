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
        var identity = await GetIdentity(request);
        if (identity == null)
        {
            return BadRequest("Invalid username or password!");
        }

        string token = CreateToken(identity);

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

    private string CreateToken(ClaimsIdentity identity)
    {
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Secret").Value));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: identity.Claims,
            expires: DateTime.Now.AddMinutes(5),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

    private async Task<ClaimsIdentity> GetIdentity(User request)
    {
        User user = await _userRepository.GetUserAuthDataByNameAsync(request.UserName);

        if (user != null && user.Password == request.Password)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                new Claim(JwtRegisteredClaimNames.Aud, _configuration.GetSection("AppSettings:Audience").Value),
                new Claim(JwtRegisteredClaimNames.Iss, _configuration.GetSection("AppSettings:Issuer").Value)
            };
            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }

        return null;

    }

}

