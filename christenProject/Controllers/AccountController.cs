using christenProject.Authontication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace christenProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(UserManager<AppUser> userManager,IConfiguration configuration) : ControllerBase
    {
        [HttpPost("Register")]
        public async Task<IActionResult> Register(AddOrUpdateAppUserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.UserName);
                if (user is not null)
                    ModelState.AddModelError("", "UserName is Already Exist!");
                else
                {
                    user = new AppUser()
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        ProfilePicture = model.ProfilePicture,  
                        SecurityStamp = Guid.NewGuid().ToString()
                    };
                    var newUser = await userManager.CreateAsync(user, model.Password);
                    if (newUser.Succeeded)
                    {
                        //return Ok(newUser);
                        var token = GetToken(model.UserName);
                        if (token != null)
                        {
                            return Ok(new
                            {
                                SecurityToken = token
                            });
                        }
                    }
                    foreach (var error in newUser.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            return BadRequest(ModelState);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.UserName);
                if (user is not null)
                {
                    var userFound = await userManager.CheckPasswordAsync(user, model.Password);
                    if (userFound)
                    {
                        return Ok(new
                        {
                            SeurityToken =await GetToken(model.UserName),
                            ExpireyDate = DateTime.UtcNow.AddDays(1)
                        });
                    }
                }
                ModelState.AddModelError("", "UserName Not Fount, Register and try agian");
            }
            return BadRequest(ModelState);  
        }
        private async Task<string> GetToken(string userName)
        {
            var secret = configuration["JWTConfig:Secret"];
            var issuer = configuration["JWTConfig:ValidIssuer"];
            var audience = configuration["JWTConfig:ValidAudiences"];
            if (secret is null || issuer is null || audience is null)
                throw new SecurityTokenExpiredException("Error in Credentials");
            var user =await userManager.FindByNameAsync(userName);
            var claims = new ClaimsIdentity(new[]
            {
               new Claim(ClaimTypes.NameIdentifier, user.Id),
               new Claim(ClaimTypes.Name, user.UserName),
               new Claim(ClaimTypes.Email, user.Email),
               new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            });
            var symetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var singnCredential = new SigningCredentials(symetricKey, SecurityAlgorithms.HmacSha256Signature);
            SecurityTokenDescriptor jwtToken = new SecurityTokenDescriptor
            {
                Issuer = issuer,
                Audience = audience,
                Expires = DateTime.UtcNow.AddDays(1),
                Subject = claims,
                SigningCredentials = singnCredential,
            };
            var buildTokenHandler = new JwtSecurityTokenHandler();
            var Buidltoken = buildTokenHandler.CreateToken(jwtToken);
            var token = buildTokenHandler.WriteToken(Buidltoken);
            return token;
        }
    }
}