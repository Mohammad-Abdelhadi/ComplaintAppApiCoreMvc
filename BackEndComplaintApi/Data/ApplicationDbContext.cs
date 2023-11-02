using BackEndComplaintApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Complaint> Complaints { get; set; }
    public DbSet<Demand> Demands { get; set; }
    public DbSet<UserLoginModel> UserLoginModels { get; set; }

    // Other DbSet properties as needed

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed static data into Users table with hashed passwords
        var passwordHasher = new PasswordHasher<User>();

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Username = "admin",
                Email = "admin@admin.com",
                PhoneNumber = "0788888888",
                Password = passwordHasher.HashPassword(null, "12345678"), // Hashed password
                ConfirmPassword = "12345678",
                Role = "admin"
            },
            new User
            {
                Id = 2,
                Username = "user",
                Email = "user@user.com",
                PhoneNumber = "0799999999",
                Password = passwordHasher.HashPassword(null, "12345678"), // Hashed password
                ConfirmPassword = "12345678",
                Role = "user"
            }
        );
    }
}
