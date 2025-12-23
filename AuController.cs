using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthRole.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;


namespace AuthRole.Controllers
{
    [ApiController]
    [Route("api/au")]
    public class AuController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // USER REGISTER
        [HttpPost("register")]
        public IActionResult Register(Meet user)
        {
            user.Role = "User"; // Always User
            _context.meet.Add(user);
            _context.SaveChanges();
            return Ok("User Registered Successfully");
        }

        // LOGIN
        [HttpPost("login")]
        public IActionResult Login(Meet login)
        {
            var user = _context.meet
                .FirstOrDefault(x => x.UserName == login.UserName && x.Password == login.Password);

            if (user == null)
                return Unauthorized("Invalid Credentials");

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
    }
}
