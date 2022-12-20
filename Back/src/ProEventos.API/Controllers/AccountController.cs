using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Contracts;
using ProEventos.Application.Dtos;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;

        public AccountController(IAccountService accountService, ITokenService tokenService)
        {
            this._accountService = accountService;
            this._tokenService = tokenService;
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {

            try
            {
                var user = await _accountService.GetUserByUserNameAsync(User.GetUserName());
                return Ok(user);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar usuario. Erro: {(ex.InnerException ?? ex).Message}");
            }
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserUpdateDto model)
        {

            try
            {
                if (await _accountService.UserExistsAsync(model.UserName))
                    return BadRequest($"Usuario {model.UserName} já existe.");

                var user = await _accountService.CreateAccountAsync(model);
                if (user != null)
                    return Ok(new
                    {
                        userName = user.UserName,
                        primeiroNome = user.PrimeiroNome,
                        token = await _tokenService.CreateToken(user)
                    });


                return BadRequest("Usuário não criado, tente novamente mais tarde");

            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar registrar novo usuario. Erro: {(ex.InnerException ?? ex).Message}");
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto model)
        {

            try
            {
                var user = await _accountService.GetUserByUserNameAsync(model.Username);
                if (user == null) return Unauthorized("Usuário ou senha não conferem");

                var result = await _accountService.CheckUserPasswordAsync(user, model.Password);
                if (!result.Succeeded) return Unauthorized("Usuário ou senha não conferem");

                var token = await _tokenService.CreateToken(user);

                return Ok(new
                {
                    userName = user.UserName,
                    primeiroNome = user.PrimeiroNome,
                    token = token
                });
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar realizar login. Erro: {(ex.InnerException ?? ex).Message}");
            }
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserUpdateDto model)
        {

            try
            {
                if (model.UserName != User.GetUserName())
                    return Unauthorized("Usuario é inválido");


                var user = await _accountService.GetUserByUserNameAsync(User.GetUserName());
                if (user == null) return Unauthorized("Usuário invalido");

                var userReturn = await _accountService.UpdateAccount(model);
                if (userReturn == null) return NoContent();

                return Ok(new
                {
                    userName = userReturn.UserName,
                    promeiroNome = userReturn.PrimeiroNome,
                    token = _tokenService.CreateToken(userReturn).Result
                });

            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar atualizar usuario. Erro: {(ex.InnerException ?? ex).Message}");
            }
        }

    }
}