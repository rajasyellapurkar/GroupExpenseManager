using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GroupExpenseManager.API.Database.Repository;
using GroupExpenseManager.API.Dtos;
using GroupExpenseManager.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GroupExpenseManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _autoMapper;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepository authRepository, IMapper autoMapper,IConfiguration config)
        {
            _autoMapper = autoMapper;
            _config = config;
            _authRepository = authRepository;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]UserForCreationDto user)
        {
            var userFromRepo = await _authRepository.GetUser(user.UserName);

            if (userFromRepo != null)
            {
                return BadRequest("Username already exits");
            }

            var mappedUser = _autoMapper.Map<User>(user);

            var newUser = await _authRepository.Register(mappedUser, user.Password);

            if (newUser != null)
            {
                var userToReturn = _autoMapper.Map<UserToReturnDto>(newUser);
                return CreatedAtRoute("GetUser", new { controller = "Users", id = userToReturn.Id }, userToReturn);
            }

            throw new Exception("Registeration failed");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]UserForLoginDto user)
        {
            var userFromRepo = await _authRepository.Login(user.UserName, user.Password);
            if (userFromRepo == null)
            {
                return Unauthorized("Invalid username or password");
            }
            var claims = new[]{
                new Claim(ClaimTypes.NameIdentifier,userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name,userFromRepo.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.
                        GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });

        }
    }
}