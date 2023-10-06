using ApiPersons.Models;
using ApiPersons.Repositories;
using ApiPersons.Utilities;
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User isAuthenticated = await userRepository.login(loginModel.Email, loginModel.Password);
            if (isAuthenticated == null)
            {
                return Unauthorized();
            }
            return Ok(new { message = "Ingresaste con exito a DreamsStyle." });
        }


        [HttpPost("sendEmail")]
        public async Task<IActionResult> SendRecoveryEmail([FromBody] RecoveryEmailModel recoveryEmailModel)
        {
            try
            {
                var user = await userRepository.getUserRecoveryAccount(recoveryEmailModel.Email);
                if(user == null)
                {
                    return BadRequest(new { message = "Error al enviar el correo de recuperación." });
                }
                var mailHelper = new MailHelper();
                string userName = user.name_user;
                string token = TokenGenerator.generateRandomToken();
                await userRepository.setToken(recoveryEmailModel.Email, token);
                mailHelper.SendEmail(recoveryEmailModel.Email, userName, token );
                return Ok(new { message = "Correo de recuperación enviado con éxito." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al enviar el correo de recuperación." });
            }
        }

        [HttpPost("updatePassword")]
        public async Task<IActionResult> SetNewPassword([FromBody] UpdatePasswordModel updatePasswordModel)
        {
            try
            {
                await userRepository.UpdateNewPassword(updatePasswordModel.Email, updatePasswordModel.Token, updatePasswordModel.NewPassword);
                return Ok(new { message = "Contraseña actualizada con éxito." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al actualizar la contraseña." });
            }
        }

        /*
        [HttpPost]
        [Route("RecoverPassword")]
        public async Task<IActionResult> recoverPassword([FromBody] string email)
        {
            User user = await userRepository.getUser(email);
            if (user == null)
            {
                return BadRequest();
            }
            string myToken = await TokenGenerator.generatePasswordResetTokenAsync();
            string link = Url.Action("ResetPassword", "Account", new { token = myToken }, protocol: HttpContext.Request.Scheme);
            _mailHelper.SendMail(request.Email, "Password Recover", $"<h1>Password Recover</h1>" +
                $"Click on the following link to change your password:<p>" +
                $"<a href = \"{link}\">Change Password</a></p>");

            return Ok(new Response { IsSuccess = true });
        }
        */
    }
}
