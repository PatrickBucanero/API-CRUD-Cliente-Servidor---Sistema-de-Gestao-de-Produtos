using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoooAPI.Data;
using ProjetoooAPI.Models;
using ProjetoooAPI.ViewModels;
using ProjetoooAPI.Services;


namespace ProjetoooAPI.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {

        [HttpPost("account/login")]
        public IActionResult Login(
            [FromBody] UserLoginViewModel model,
            [FromServices] AppDbContext context,
            [FromServices] TokenService tokenService)
        {
            var user = context.Usuarios.FirstOrDefault(x => x.Email == model.Email);

            if (user == null)
                return StatusCode(401, new { message = "Usuário ou senha inválidos" });

            if (model.Password != user.Password)
                return StatusCode(401, new { message = "Usuário ou senha inválidos" });

            try
            {
                var token = tokenService.CreateToken(user);
                return Ok(new { token = token });
            }
            catch
            {
                return StatusCode(500, new { message = "Falha interna no servidor" });
            }
        }

        [HttpPost("account/signup")]
        public IActionResult Signup(
            [FromBody] UserSignupViewModel model,
            [FromServices] AppDbContext context)
        {
            var user = context.Usuarios.FirstOrDefault(x => x.Email == model.Email);

            if (user != null)
                return StatusCode(401, new { message = "E-mail já cadastrado" });

            try
            {
                var usuarioNew = new Usuario
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password,
                    Role = "Cliente"
                };

                context.Usuarios.Add(usuarioNew);
                context.SaveChanges();

                return Ok(new { userId = usuarioNew.Id });
            }
            catch
            {
                return StatusCode(500, new { message = "Falha interna no servidor" });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("account/user")]
        public IActionResult Get(
            [FromServices] AppDbContext context)
        {
            try
            {
                var usuarios = context.Usuarios.ToList().Select(x => new UsuarioReturnViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    Role = x.Role,
                    Password = x.Password
                });

                return Ok(usuarios);
            }
            catch
            {
                return StatusCode(500, new { message = "Falha interna no servidor" });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("account/user/{id}")]
        public IActionResult Delete(
        [FromServices] AppDbContext context,
    [   FromRoute] int id)
        {
            try
            {
                // Localiza o usuário pelo ID
                var usuario = context.Usuarios.FirstOrDefault(x => x.Id == id);

                if (usuario == null)
                {
                    return NotFound(new { message = "Usuário não encontrado" });
                }

                // Remove o usuário
                context.Usuarios.Remove(usuario);
                context.SaveChanges();

                return Ok(new { message = "Usuário removido com sucesso" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Falha interna no servidor", error = ex.Message });
            }
        }

    }
}