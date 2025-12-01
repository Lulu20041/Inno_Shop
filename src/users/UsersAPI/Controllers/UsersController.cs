using Application.DTO;
using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UsersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService service;

        public UsersController(IUserService service) => this.service = service; 

        [HttpGet]
        public async Task<ActionResult<List<User>>> Get()
        {
            var users = await service.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            var user = await service.GetByIdAsync(id);
            return Ok(user);
        }

        [HttpPut("{id}/activate")]
        public async Task<IActionResult> ActivateById(int id)
        {
            await service.ActivateUserAsync(id);
            return Ok();
        }

        [HttpPut("{id}/role")]
        public async Task<IActionResult> UpdateUserRole(int id, [FromBody] int role)
        {
            await service.UpdateUserRole(id, (UserRole)role);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeactivateById(int id)
        {
            await service.DeactivateUserAsync(id);
            return Ok();
        }
    }
}
