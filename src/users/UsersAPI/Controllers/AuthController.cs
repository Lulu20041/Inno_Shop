using Application.DTO;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UsersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "UserOrAdmin")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService service;

        public AuthController(IUserService service) => this.service = service;

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Post(UserDTO request)
        {
            await service.Register(request.Name, request.Email, request.Password);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDTO request)
        {
            string token = await service.Login(request.Email, request.Password);
            return Ok(token);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin(UserDTO request)
        {
            await service.RegisterAdmin(request.Name, request.Email, request.Password);
            return Ok();
        }
    }
}
