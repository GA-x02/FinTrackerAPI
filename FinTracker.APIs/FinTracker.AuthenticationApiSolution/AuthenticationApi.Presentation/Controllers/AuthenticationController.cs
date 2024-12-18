using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using AuthenticationApi.Domain.Entities;
using FinTracker.SharedLibrary.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IAppUser appUser) : Controller
    {
        [HttpPost("login")]
        public async Task<ActionResult<Response>> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resp = await appUser.Login(loginDTO);
            return resp.Flag ? Ok(resp) : BadRequest(resp.Message);
        }

        [HttpPost("register")]
        public async Task<ActionResult<Response>> Register(AppUserDTO registerDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resp = await appUser.Register(registerDTO);
            return resp.Flag ? Ok(resp) : BadRequest(resp.Message);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GetAppUserDTO>> GetUser(Guid id)
        {
            var user = await appUser.GetAppUser(id);
            if (user is null)
                return NotFound($"User with id = {id} does not exist");

            return Ok(user);
        }
    }
}
