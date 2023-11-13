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
    public async Task<IActionResult> Register(ViewData req)
    {
        // Check if the email is already registered
        var existingUser = _context.Users.SingleOrDefault(u => u.Email == req.Users.Email);
        if (existingUser != null)
        {
            return Conflict(new { message = "Email already registered" });
        }

        // using ASP.NET Core's built-in PasswordHasher
        var passwordHasher = new PasswordHasher<User>();
        req.Users.Password = passwordHasher.HashPassword(req.Users, req.Users.Password);

        _context.Users.Add(req.Users);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Registration successful", Id = req.Users.Id,Email = req.Users.Email,username = req.Users.Username,phonenumber = req.Users.PhoneNumber,role=req.Users.Role });
    }

    [HttpPost("login")]
    public IActionResult Login(ViewData req)
    {
        // Find the user by Email
        var user = _context.Users.SingleOrDefault(u => u.Email == req.UserLoginModels.Email);

        // Check if the user exists
        if (user == null)
        {
            return Unauthorized(new { message = "Email Not Found" });
        }

        var passwordHasher = new PasswordHasher<User>();
        var result = passwordHasher.VerifyHashedPassword(user, user.Password, req.UserLoginModels.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            return Unauthorized(new { message = "Invalid Email or password" });
        }
        return Ok(user);
    }
    

}
