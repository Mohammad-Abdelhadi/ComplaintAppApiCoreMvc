using BackEndComplaintApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AuthController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        // Check if the email is already registered
        var existingUser = _context.Users.SingleOrDefault(u => u.Email == user.Email);
        if (existingUser != null)
        {
            return Conflict(new { message = "Email already registered" });
        }

        // using ASP.NET Core's built-in PasswordHasher
        var passwordHasher = new PasswordHasher<User>();
        user.Password = passwordHasher.HashPassword(user, user.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Registration successful", Id = user.Id,Email = user.Email,username=user.Username,phonenumber = user.PhoneNumber,role=user.Role });
    }

    [HttpPost("login")]
    public IActionResult Login(UserLoginModel userLoginModel)
    {
        // Find the user by Email
        var user = _context.Users.SingleOrDefault(u => u.Email == userLoginModel.Email);

        // Check if the user exists
        if (user == null)
        {
            return Unauthorized(new { message = "Email Not Found" });
        }

        var passwordHasher = new PasswordHasher<User>();
        var result = passwordHasher.VerifyHashedPassword(user, user.Password, userLoginModel.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            return Unauthorized(new { message = "Invalid Email or password" });
        }
        return Ok(user);
    }
    

}
