using WellnessDiaryApi.Data;
using WellnessDiaryApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WellnessDiaryApi.Data.Dto;

namespace WellnessDiaryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IConfiguration configuration, AppDbContext context, ILogger<AuthController> logger)
        {
            _configuration = configuration;
            _context = context;
            _logger = logger;
        }

        

        // Register a new user
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            // Check if username is already taken
            if (_context.Users.Any(u => u.Username == request.Username))
            {
                return BadRequest("Username is already taken.");
            }

            // Create password hash
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            // Create user
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                Password = request.Password, // Set the Password property
                PasswordHash = Convert.ToBase64String(passwordHash),
                PasswordSalt = Convert.ToBase64String(passwordSalt),
                RegistrationDate = DateTime.Now,
                Role = "user",
            };

            // Save user to database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }
               

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserLoginDto request)
        {
            // Log the username
            _logger.LogInformation("User login requested for username: {Username}", request.Username);

            // Retrieve user from database
            var user = _context.Users.SingleOrDefault(u => u.Username == request.Username);

            // Check if user exists
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            // Verify password
            if (!VerifyPasswordHash(request.Password, Convert.FromBase64String(user.PasswordHash), Convert.FromBase64String(user.PasswordSalt)))
            {
                return BadRequest("Wrong password.");
            }

            // Generate JWT token
            string accessToken = CreateToken(user);

            // Generate refresh token
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken.Token;
            user.TokenExpires = refreshToken.Expires;

            // Save changes to database
            await _context.SaveChangesAsync();

            // Set refresh token cookie
            Response.Cookies.Append("refreshToken", refreshToken.Token, new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.Expires,
                SameSite= SameSiteMode.None,
                Secure= true
            });

            return Ok(accessToken);
            //return Ok(new TokenResponse { AccessToken = accessToken, RefreshToken = refreshToken.Token });
        }

        //public class TokenResponse
        //{
        //    public string AccessToken { get; set; }
        //    public string RefreshToken { get; set; }
        //}


        // Refresh JWT token
        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized("No refresh token provided.");
            }

            var user = _context.Users.SingleOrDefault(u => u.RefreshToken == refreshToken);

            if (user == null)
            {
                return Unauthorized("Invalid refresh token.");
            }

            if (user.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Refresh token expired.");
            }

            // Generate new JWT token
            string newToken = CreateToken(user);

            // Generate new refresh token
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;

            // Save changes to database
            await _context.SaveChangesAsync();

            // Set new refresh token cookie
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires,
                SameSite = SameSiteMode.None,
                Secure= true
            });

            return Ok(newToken);
        }

        // Helper method to generate a refresh token
        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomNumber),
                    Expires = DateTime.Now.AddDays(7), // Set expiration for 7 days
                    Created = DateTime.Now
                };
            }
        }

        // Helper method to create JWT token
        private string CreateToken(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }

            if (string.IsNullOrEmpty(user.Username))
            {
                throw new ArgumentException("User's username cannot be null or empty.", nameof(user));
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new(JwtRegisteredClaimNames.Name, user.Username),
                new("role", value: user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        // Helper method to create password hash
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        // Helper method to verify password hash
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}

