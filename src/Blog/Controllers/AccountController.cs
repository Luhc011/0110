using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.Services;
using Blog.ViewModels;
using Blog.ViewModels.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;
using System.Text.RegularExpressions;

namespace Blog.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost("v1/accounts")]
        public async Task<IActionResult> Post(
            [FromBody] RegisterViewModel model,
            [FromServices] EmailService emailService,
            [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Slug = model.Email.Replace("@", "-").Replace(".", "-")
            };

            var password = PasswordGenerator.Generate(25);
            user.PasswordHash = PasswordHasher.Hash(password);

            try
            {
                context.Users.Add(user);
                await context.SaveChangesAsync();

                emailService.Send(
                        user.Name,
                        user.Email,
                        "Bem vindo ao Blog",
                        $"Sua senha é <strong>{password}</strong>"
                    );

                return Ok(new ResultViewModel<dynamic>(new
                {
                    user = user.Email,
                    password
                }));
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new ResultViewModel<string>("Este email já esta cadastrado"));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                       new ResultViewModel<string>("Falha ao cadastrar usuário"));
            }
        }


        [HttpPost("v1/accounts/login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginViewModel model,
            [FromServices] DataContext context,
            [FromServices] TokenService service)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

            var user = await context
                .Users
                .AsNoTracking()
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Email == model.Email);

            if (user is null)
                return StatusCode(StatusCodes.Status404NotFound,
                                       new ResultViewModel<string>("Usuário ou senha inválida"));

            if (!PasswordHasher.Verify(user.PasswordHash, model.Password))
                return StatusCode(StatusCodes.Status404NotFound,
                                       new ResultViewModel<string>("Usuário ou senha inválida"));

            try
            {
                var token = service.GenerateToken(user);
                return Ok(new ResultViewModel<string>(token, null!));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                                          new ResultViewModel<string>("Falha interna no servidor"));
            }
        }

        [Authorize]
        [HttpPost("v1/accounts/upload-image")]
        public async Task<IActionResult> UploadImage(
            [FromBody] UploadImageViewModel model,
            [FromServices] DataContext context)
        {
            var fileName = $"{Guid.NewGuid()}.jpg";
            var data = new Regex(@"^data:image\/[a-z]+;base64,").Replace(model.Base64Image, string.Empty);
            var bytes = Convert.FromBase64String(data);

            try
            {
                await System.IO.File.WriteAllBytesAsync($"wwwroot/images/{fileName}", bytes);

                var user = await context.Users.FirstOrDefaultAsync(x => x.Email == User.Identity!.Name);
                user!.Image = fileName;
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<string>($"images/{fileName}", null!));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                    new ResultViewModel<string>("Falha interna no servidor"));
            }
        }
    }
}
