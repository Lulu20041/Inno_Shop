using Application.Commands;
using Application.DTO;
using Application.Interfaces;
using MediatR;
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
        private readonly IHttpContextAccessor accessor;
        private readonly IUserService service;
        private readonly IMediator mediator;

        public AuthController(IHttpContextAccessor accessor,IUserService service,IMediator mediator)
        {
            this.accessor = accessor;
            this.service = service;
            this.mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Post(UserDTO request, CancellationToken cancellationToken)
        {
            await service.Register(request.Name, request.Email, request.Password, cancellationToken);
            return Ok();
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(
        [FromQuery] string token,
        [FromQuery] string email,
        CancellationToken cancellationToken)
        {
            await mediator.Send(new ConfirmEmailCommand(token, email), cancellationToken);
            return Ok(new { message = "Email confirmed successfully" });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDTO request, CancellationToken cancellationToken)
        {
            string token = await service.Login(request.Email, request.Password, cancellationToken);

            var context = accessor.HttpContext;
            context?.Response.Cookies.Append("wjt", token);

            return Ok(token);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin(UserDTO request, CancellationToken cancellationToken)
        {
            await service.RegisterAdmin(request.Name, request.Email, request.Password, cancellationToken);
            return Ok();
        }
    }
}
