using System.Threading.Tasks;
using AutoMapper;
using GroupExpenseManager.API.Database.Repository;
using GroupExpenseManager.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroupExpenseManager.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController:ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _autoMapper;
        public UsersController(IAuthRepository authRepository, IMapper autoMapper)
        {
            _autoMapper = autoMapper;
            _authRepository = authRepository;
        } 
        
        [HttpGet("{id}",Name="GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var userFromRepo = await _authRepository.GetUser(id);
            if(userFromRepo == null)
            {
                return BadRequest("User does not exist");
            }
            var user = _autoMapper.Map<UserToReturnDto>(userFromRepo);
            return Ok(user);
        }
    }
}