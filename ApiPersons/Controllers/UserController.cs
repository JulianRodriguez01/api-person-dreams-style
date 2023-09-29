using ApiPersons.Models;
using ApiPersons.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPersons.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UserController(IUserRepository userRepository) { 
            this.userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> getListUser() {
            return Ok(await userRepository.getListUsers());
        }

        [HttpGet("{document_number}")]
        [ProducesResponseType(typeof(User), 200)] 
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> getUser(string document_number)
        {
            var user = await userRepository.getUser(document_number);
            return Ok(user);
        }


        [HttpPost]
        public async Task<IActionResult> addUser([FromBody] User user)
        {
            if (user == null) 
                return BadRequest();
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Created("Created", await userRepository.addUser(user));
        }

        [HttpPut]
        public async Task<IActionResult> updateUser([FromBody] User user)
        {
            if (user == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await userRepository.updateUser(user);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> deleteUser([FromBody] string document_number)
        {
            await userRepository.removeUser(new User { document_number = document_number });
            return NoContent();
        }
        /*
        [HttpGet("{email}, {password}")]
        public async Task<IActionResult> login([FromBody] string email, string password)
        {
            await userRepository.login(email, password);
            return Ok();
        }
        */
    }
}
