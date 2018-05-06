using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EnarcLabs.Technonomicon.Daemon.MessageService;
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
        private readonly IMessageService _messageService;

        private string CurrentUsername => User.FindFirst(ClaimTypes.Name).Value;

        public ApiController(IConfiguration configuration, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IMessageService messageService)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _messageService = messageService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Token(string username, string password)
        {
            var usr = await _userManager.FindByNameAsync(username);
            var signInResult = await _signInManager.PasswordSignInAsync(usr, password, false, false);
            if (signInResult.Succeeded && usr.EmailConfirmed) //Users are required to have a confirmed email before they can get tokens.
                return new ObjectResult(GenerateToken(username));

            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(string username, string email, string password)
        {
            var usr = new IdentityUser { Email = email, UserName =  username };
            var result = await _userManager.CreateAsync(usr
                , password);
            if(!result.Succeeded)
                return Json(new
                {
                    success = false,
                    errors = result.Errors.Select(x => x.Description).ToArray()
                });

            //TODO: Handle the result of this
            await _messageService.Send(new MailMessage
                {
                    To = {email},
                    Body = $"Your verification token is: {await _userManager.GenerateEmailConfirmationTokenAsync(usr)}"
                });

            return Json(new { success = true });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Validate(string username, string emailToken)
        {
            var result = await _userManager.ConfirmEmailAsync(await _userManager.FindByNameAsync(username), emailToken);
            if (!result.Succeeded)
                return Json(new
                {
                    success = false,
                    errors = result.Errors.Select(x => x.Description).ToArray()
                });

            return Json(new {success = true});
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

            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("SecretKey"))),
                    SecurityAlgorithms.HmacSha256)), new JwtPayload(claims));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}