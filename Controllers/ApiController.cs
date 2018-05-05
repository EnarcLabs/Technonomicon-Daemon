using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EnarcLabs.Technonomicon.Daemon.Controllers
{
    public class ApiController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public ApiController(IConfiguration configuration, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var a = await _signInManager.PasswordSignInAsync(username, password, false, false);
            if(a.Succeeded)
                return new ObjectResult(GenerateToken(username));

            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            var usr = await _userManager.CreateAsync(new IdentityUser { Email = username, UserName = username }, password);
            if(!usr.Succeeded)
                return Json(new
                {
                    success = false,
                    errors = usr.Errors.Select(x => x.Description).ToArray()
                });
            return Json(new { success = true });
        }

        [Authorize]
        [HttpGet]
        public IActionResult Item(int? id = null)
        {
            if(id != null)
                return Json(new { message = $"Here's your item: {id.Value}" });

            return Json(new []{
                new { message = "I am an object!" },
                new { message = "Me too!" }
            });
        }

        private string GenerateToken(string username)
        {
            var claims = new []
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
            };

            var token = new JwtSecurityToken(new JwtHeader(new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("SecretKey"))), SecurityAlgorithms.HmacSha256)), new JwtPayload(claims));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}