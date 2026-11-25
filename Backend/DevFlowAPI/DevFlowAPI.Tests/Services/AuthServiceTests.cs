using DevFlowAPI.Data;
using DevFlowAPI.DTOs.Auth;
using DevFlowAPI.Exceptions.Auth;
using DevFlowAPI.Models;
using DevFlowAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace DevFlowAPI.Tests.Services;

[TestFixture]
public class AuthServiceTests
{
    #region Login Tests

    [Test]
    public void LoginAsync_WhenUserDoesNotExist_ThrowsInvalidCredentialsException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new AppDbContext(options);

        var passwordHasher = new PasswordHasher<User>();

        var configuration = new ConfigurationBuilder()
            .Build();

        var authService = new AuthService(
            context,
            passwordHasher,
            configuration
        );

        var dto = new LoginDto
        {
            Email = "unknown@example.com",
            Password = "Password123!"
        };

        // Act & Assert
        Assert.ThrowsAsync<InvalidCredentialsException>(
            () => authService.LoginAsync(dto)
        );
    }
    [Test]
    public void LoginAsync_WhenPasswordIsIncorrect_ThrowsInvalidCredentialsException()
    {
      
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new AppDbContext(options);

        var passwordHasher = new PasswordHasher<User>();

        var configuration = new ConfigurationBuilder()
            .Build();

        var user = new User
        {
            FirstName = "Test",
            LastName = "User",
            Email = "test@example.com"
        };

        user.PasswordHash = passwordHasher.HashPassword(
            user,
            "CorrectPassword123!"
        );

        context.Users.Add(user);
        context.SaveChanges();

        var authService = new AuthService(
            context,
            passwordHasher,
            configuration
        );

        var dto = new LoginDto
        {
            Email = "test@example.com",
            Password = "WrongPassword123!"
        };

       
        Assert.ThrowsAsync<InvalidCredentialsException>(
            () => authService.LoginAsync(dto)
        );
    }
    [Test]
    public async Task LoginAsync_WhenCredentialsAreCorrect_ReturnsAuthResponseWithToken()
    {
        
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new AppDbContext(options);

        var passwordHasher = new PasswordHasher<User>();

        var configurationData = new Dictionary<string, string?>
        {
            ["Jwt:Key"] = "ThisIsASecretKeyForTestingPurposes123456!",
            ["Jwt:Issuer"] = "DevFlow",
            ["Jwt:Audience"] = "DevFlowUsers",
            ["Jwt:ExpirationInMinutes"] = "60"
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configurationData)
            .Build();

        var user = new User
        {
            FirstName = "Test",
            LastName = "User",
            Email = "test@example.com"
        };

        user.PasswordHash = passwordHasher.HashPassword(
            user,
            "CorrectPassword123!"
        );

        context.Users.Add(user);
        context.SaveChanges();

        var authService = new AuthService(
            context,
            passwordHasher,
            configuration
        );

        var dto = new LoginDto
        {
            Email = "test@example.com",
            Password = "CorrectPassword123!"
        };

       
        var result = await authService.LoginAsync(dto);

     
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Token, Is.Not.Null.And.Not.Empty);
    }

    #endregion

    #region Register Tests
    [Test]
    public void RegisterAsync_WhenEmailAlreadyExists_ThrowsEmailAlreadyRegisteredException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new AppDbContext(options);

        var passwordHasher = new PasswordHasher<User>();

        var configuration = new ConfigurationBuilder()
            .Build();

        var existingUser = new User
        {
            FirstName = "Existing",
            LastName = "User",
            Email = "test@example.com",
            PasswordHash = "SomeHash"
        };

        context.Users.Add(existingUser);
        context.SaveChanges();

        var authService = new AuthService(
            context,
            passwordHasher,
            configuration
        );

        var dto = new RegisterDto
        {
            FirstName = "New",
            LastName = "User",
            Email = "test@example.com",
            Password = "Password123!"
        };

        
        Assert.ThrowsAsync<EmailAlreadyRegisteredException>(
            () => authService.RegisterAsync(dto)
        );
    }


    [Test]
    public async Task RegisterAsync_WhenDataIsValid_CreatesUserAndReturnsToken()
    {
        
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new AppDbContext(options);

        var passwordHasher = new PasswordHasher<User>();

        var configurationData = new Dictionary<string, string?>
        {
            ["Jwt:Key"] = "ThisIsASecretKeyForTestingPurposes123456!",
            ["Jwt:Issuer"] = "DevFlow",
            ["Jwt:Audience"] = "DevFlowUsers",
            ["Jwt:ExpirationInMinutes"] = "60"
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configurationData)
            .Build();

        var authService = new AuthService(
            context,
            passwordHasher,
            configuration
        );

        var dto = new RegisterDto
        {
            FirstName = "Test",
            LastName = "User",
            Email = "new@example.com",
            Password = "Password123!",
            BirthDate = new DateOnly(2000, 1, 1)
        };

        
        var result = await authService.RegisterAsync(dto);

       
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Token, Is.Not.Null.And.Not.Empty);

        var savedUser = await context.Users
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        Assert.That(savedUser, Is.Not.Null);
        Assert.That(savedUser!.PasswordHash, Is.Not.EqualTo(dto.Password));

        Assert.That(
            passwordHasher.VerifyHashedPassword(
                savedUser,
                savedUser.PasswordHash,
                dto.Password
            ),
            Is.EqualTo(PasswordVerificationResult.Success)
        );
    }
    #endregion
}