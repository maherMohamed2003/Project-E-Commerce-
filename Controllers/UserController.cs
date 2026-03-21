using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
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
        public async Task<IActionResult> Register([FromBody] RegisterDTO register)
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
                EmailToken = Guid.NewGuid().ToString(),
                Email = register.Email
            };


            newUser.Password = hasher.HashPassword(newUser, register.Password);
            await _context.Customers.AddAsync(newUser);
            await _context.SaveChangesAsync();
            
            var confirmationLink = $"{Request.Scheme}://{Request.Host}/api/User/ConfirmEmail?token={newUser.EmailToken}";
            await SendEmailAsync(newUser.Email, $"Confirm your Email:\n{confirmationLink}");

            return Ok("Check your email to confirm your account");
            
        }

        [HttpGet]
        [Route("ConfirmEmail/{token}")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string token)
        {
            var nowUser = await _context.Customers.FirstOrDefaultAsync(u => u.EmailToken == token);
            if (nowUser == null)
                return NotFound("The Account Is Already Activated!");
            nowUser.IsEmailVerified = true;
            nowUser.EmailToken = "";
            await _context.SaveChangesAsync();
            var cart = new Cart
            {
                CustomerId = nowUser.Id
            };

            var fav = new Favourite
            {
                CustomerId = nowUser.Id
            };

            var role = new Role
            {
                Name = "User",
                customerId = nowUser.Id
            };

            await _context.Carts.AddAsync(cart);
            await _context.Favourites.AddAsync(fav);
            await _context.Roles.AddAsync(role);


            var JWTtoken = GenerateToken(nowUser);
            await _context.SaveChangesAsync();
            return Ok(JWTtoken);
        }

        [HttpPost]
        [Route("Login/")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            var user = _context.Customers.FirstOrDefault(u => u.Email == login.Email);
           
            if (user == null)
                return NotFound("Invalid Email Or Password");

            if (!user.IsEmailVerified)
            {
                await SendEmailAsync(user.Email, $"Please Verify Your Email To Login:\n{Request.Scheme}://{Request.Host}/api/User/ConfirmEmail?token={user.EmailToken}");
                return BadRequest("Please Check Your Email To Verify First");
            }

            var hasher = new PasswordHasher<Customer>();
            var result = hasher.VerifyHashedPassword(user, user.Password, login.Password);
            
            if (result == PasswordVerificationResult.Failed)
                return NotFound("Invalid Email Or Password");
            var token = GenerateToken(user);
            
            return Ok(token);
        }

        [HttpPut]
        [Route("EditProfile/")]
        public async Task<IActionResult> EditProfile([FromBody] EditProfileDTO edit)
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
        [AllowAnonymous]
        public async Task<IActionResult> DeleteProfile(int id)
        {
            var user = await _context.Customers.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return NotFound("User Not Found");
            _context.Customers.Remove(user);
            await _context.SaveChangesAsync();
            return Ok("Profile Deleted Successfully");
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Customers.ToListAsync();
            return Ok(users);
        }



        private string GenerateToken(Customer r)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwt.Issuer,
                Expires = DateTime.Now.AddSeconds(20),
                Audience = _jwt.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key)), SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, r.Email),
                    new Claim(ClaimTypes.NameIdentifier , r.Id.ToString())
                }),
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            return accessToken;

        }

        private async Task SendEmailAsync( string to, string body)
        {
            var smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("maheralzamzamy3@gmail.com", "sxlv fxga ffqx xinw"),
                EnableSsl = true
            };

            var mail = new MailMessage
            {
                From = new MailAddress("maheralzamzamy3@gmail.com"),
                Subject = "Confirm your account",
                Body = body,
                IsBodyHtml = true
            };

            mail.To.Add(to);

            await smtp.SendMailAsync(mail);
        }
    }
}
