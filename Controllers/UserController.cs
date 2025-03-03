using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QuanLyBanHang.Data;
using QuanLyBanHang.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace QuanLyBanHang.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public UserController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet("CheckToken")]
        [Authorize]
        public IActionResult CheckToken()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            if (username == null)
            {
                return Unauthorized(new { Message = "Token không hợp lệ" });
            }
            return Ok(new { Message = "Token hợp lệ", Username = username });
        }

        // ======= ĐĂNG KÝ =======
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest userInput)
        {
            if (userInput == null ||
                string.IsNullOrWhiteSpace(userInput.Username) ||
                string.IsNullOrWhiteSpace(userInput.Email) ||
                string.IsNullOrWhiteSpace(userInput.Password))
            {
                return BadRequest(new { Message = "Dữ liệu không hợp lệ." });
            }

            // Kiểm tra trùng Username hoặc Email
            if (_context.Users.Any(u => u.Username == userInput.Username || u.Email == userInput.Email))
            {
                return Conflict(new { Message = "Username hoặc Email đã tồn tại." });
            }

            // Mã hóa mật khẩu trước khi lưu vào cơ sở dữ liệu
            var passwordHasher = new PasswordHasher<User>();
            var newUser = new User
            {
                Username = userInput.Username,
                Email = userInput.Email,
                Password = passwordHasher.HashPassword(new User(), userInput.Password), // Fix lỗi CS8625
                Role = userInput.Role,
            };

            // Lưu User mới
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Đăng ký thành công!" });
        }

        // ======= ĐĂNG NHẬP =======
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginRequest loginInput)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(loginInput.Username) || string.IsNullOrWhiteSpace(loginInput.Password))
                {
                    return BadRequest(new { Message = "Thông tin đăng nhập không hợp lệ." });
                }

                var user = _context.Users.FirstOrDefault(u => u.Username == loginInput.Username || u.Email == loginInput.Username);
                if (user == null)
                {
                    return Unauthorized(new { Message = "Sai thông tin đăng nhập." });
                }

                var passwordHasher = new PasswordHasher<User>();
                var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.Password, loginInput.Password);

                if (passwordVerificationResult != PasswordVerificationResult.Success)
                {
                    return Unauthorized(new { Message = "Sai thông tin đăng nhập." });
                }

                var token = GenerateJwtToken(user);

                // ✅ Thêm object `User` vào response
                return Ok(new
                {
                    Message = "Đăng nhập thành công!",
                    AccessToken = token,
                    User = new
                    {
                        Username = user.Username,
                        Email = user.Email,
                        Role = user.Role,
                        UserID = user.UserID
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lỗi server", Error = ex.Message });
            }
        }

        // ======= TẠO JWT TOKEN =======
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Kiểm tra xem có giá trị không (Fix lỗi CS8604)
            var secretKey = _configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key không được cấu hình.");
            var key = Encoding.UTF8.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username ?? "UnknownUser"),  // Fix lỗi CS8604
                    new Claim(ClaimTypes.Email, user.Email ?? "UnknownEmail")   // Fix lỗi CS8604
                }),
                Expires = DateTime.UtcNow.AddHours(2), // Token hết hạn sau 2 giờ
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }

    // DTO: Dữ liệu nhận từ client khi đăng ký
    public class RegisterRequest
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
    }

    // DTO: Dữ liệu nhận từ client khi đăng nhập
    public class LoginRequest
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
