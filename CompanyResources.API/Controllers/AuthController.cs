using CompanyResources.API.Data;
using CompanyResources.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CompanyResources.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(RegisterDto request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                return BadRequest("Użytkownik już istnieje.");

            var user = new User { Email = request.Email, PasswordHash = request.Password, Role = request.Role };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new AuthResponse { Success = true });
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email && u.PasswordHash == request.Password);
            if (user == null) return BadRequest("Błędny email lub hasło.");

            string token = CreateToken(user);
            return Ok(new AuthResponse { Success = true, Token = token });
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim> {
        new Claim(ClaimTypes.Name, user.Email),
        new Claim(ClaimTypes.Role, user.Role)
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ToJestBardzoDlugiIBezpiecznyKluczKtoryMaWystarczajacoDuzoZnakowDlaSHA512!!!"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
