using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using E_Commerce_Proj.Authentication;
using E_Commerce_Proj.Data;
using E_Commerce_Proj.DTOs.User;
using E_Commerce_Proj.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using storeProject.Models;

namespace E_Commerce_Proj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JWTOptions _jwt;

        public UserController(AppDbContext context, JWTOptions jwt)
        {
            _context = context;
            _jwt = jwt;
        }

        [HttpPost]
        [Route("Register/")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterDTO register)
        {
            if(_context.Customers.Any(u => u.Email == register.Email))
                return BadRequest("Email Already Exists");

            if(_context.Customers.Any(u => u.FName == register.FName && u.LName == register.LName))
                return BadRequest("User Already Exists");

            var hasher = new PasswordHasher<Customer>();
            var newUser = new Customer
            {
                FName = register.FName,
                LName = register.LName,
                Email = register.Email
            };
            newUser.Password = hasher.HashPassword(newUser, register.Password);
            await _context.Customers.AddAsync(newUser);
            
            var cart = new Cart
            {
                CustomerId = newUser.Id
            };

            var fav = new Favourite
            {
                CustomerId = newUser.Id
            };

            var role = new Role
            {
                Name = "User"
            };
            

            var token = GenerateToken(newUser);
            await _context.SaveChangesAsync();
            return Ok(token);
        }

        [HttpPost]
        [Route("Login/")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            var user = _context.Customers.FirstOrDefault(u => u.Email == login.Email);
           
            if (user == null)
                return NotFound("Invalid Email Or Password");
            var hasher = new PasswordHasher<Customer>();
            var result = hasher.VerifyHashedPassword(user, user.Password, login.Password);
            
            if (result == PasswordVerificationResult.Failed)
                return NotFound("Invalid Email Or Password");
            var token = GenerateToken(user);
            
            return Ok(token);
        }

        [HttpPut]
        [Route("EditProfile/{id}")]
        public async Task<IActionResult> EditProfile(int id ,EditProfileDTO edit)
        {
            var user = await _context.Customers.FirstOrDefaultAsync(u => u.FName == edit.FName && u.LName == edit.LName);
            if (user == null)
                return NotFound("User Not Found");
            user.FName = edit.FName;
            user.LName = edit.LName;
            user.Email = edit.Email;
            var hasher = new PasswordHasher<Customer>();
            user.Password = hasher.HashPassword(user, edit.Password);
            await _context.SaveChangesAsync();
            return Ok("Profile Updated Successfully");
        }

        [HttpDelete]
        [Route("DeleteProfile/{id}")]
        public async Task<IActionResult> DeleteProfile(int id)
        {
            var user = await _context.Customers.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return NotFound("User Not Found");
            _context.Customers.Remove(user);
            await _context.SaveChangesAsync();
            return Ok("Profile Deleted Successfully");
        }

        private string GenerateToken(Customer r)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var credintials = new SigningCredentials(key , SecurityAlgorithms.HmacSha256);
            var roles = _context.Roles.Where(r => r.customerId == r.Id).Select(r => r.Name).ToList();
            
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,r.Id.ToString()),
                new Claim(ClaimTypes.Email,r.Email),
                new Claim(ClaimTypes.Name,r.FName + " " + r.LName)
            };
            
            foreach(var role in roles)
            {
                claims.Append(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: credintials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
