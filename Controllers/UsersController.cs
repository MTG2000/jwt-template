using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using JwtTemplate.Domain.Models;
using JwtTemplate.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtTemplate.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository repository;

        public UsersController(IUserRepository _repository)
        {
            repository = _repository;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            return (await repository.GetAllAsync()).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(int id)
        {


            if (!hasAccess(User, id))
                return Forbid();
            return await repository.GetAsync(id);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]User newUser)
        {
            try
            {
                await repository.AddAsync(newUser);
                await repository.SaveAsync();
                return Ok(newUser.Id);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(User user)
        {

            var token = await repository.LoginUser(user);
            if (token == null) return BadRequest();
            return Ok(new { token = token });

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!hasAccess(User, id))
                return Forbid();
            try
            {
                var userToRemove = await repository.GetAsync(id);
                if (userToRemove == null)
                    return NotFound(new Response("No User was found with this id"));
                repository.Remove(userToRemove);
                await repository.SaveAsync();
                return Ok(new Response("User Was Deleted Successfully"));

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] User userData, int id)
        {
            if (!hasAccess(User, id))
                return Forbid();
            var user = await repository.GetAsync(id);

            user.Name = userData.Name ?? user.Name;
            try
            {
                await repository.SaveAsync();
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        private bool hasAccess(ClaimsPrincipal User, int id)
        {
            return int.Parse(User.Identity.Name) == id || User.IsInRole("Admin");//If He isnt the id owner neither is he the admin
        }

    }
}