using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Contracts;
using ProEventos.Application.Dtos;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly IEventoService _eventoService;
        private readonly IAccountService _accountService;
        private readonly IWebHostEnvironment _env;

        public EventosController(IEventoService eventoService, 
            IAccountService accountService,
            IWebHostEnvironment env)
        {

            _eventoService = eventoService;
            _accountService = accountService;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                
                var eventos = await _eventoService.GetAllEventosAsync(User.GetUserId(), includePalestrantes: true);
                if (eventos == null || eventos.Length == 0) return NoContent();

                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar eventos: {(ex.InnerException ?? ex).Message}");
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), id, includePalestrantes: true);
                if (evento == null || evento.Id == 0) return NoContent();

                return Ok(evento);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao recuperar evento: {(ex.InnerException ?? ex).Message}");
            }
        }

        [HttpGet("{tema}/tema")]
        public async Task<IActionResult> GetbyTema(string tema)
        {
            try
            {
                var evento = await _eventoService.GetAllEventosByTemaAsync(User.GetUserId(), tema, includePalestrantes: true);
                if (evento == null || evento.Length == 0)
                {
                    return NoContent();
                }

                return Ok(evento);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao recuperar evento: {(ex.InnerException ?? ex).Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(EventoDto model)
        {
            try
            {
                var evento = await _eventoService.Add(User.GetUserId(), model);
                if (evento == null) return NoContent();

                return Ok(evento);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao adicionar evento: {(ex.InnerException ?? ex).Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EventoDto model)
        {
            try
            {
                var evento = await _eventoService.Update(User.GetUserId(), id, model);
                if (evento == null || evento.Id == 0) return NoContent();

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar atualizar evento: {(ex.InnerException ?? ex).Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), id, includePalestrantes: false);
                if (evento == null) return NoContent();

                if (await _eventoService.Remove(User.GetUserId(), id))
                {
                    DeleteImage(evento.ImageUrl);
                    return Ok(new { message = "Evento deletado" });
                }
                else
                {
                    throw new Exception("Ocorreu um problema não especifico ao tentar deletar o evento");
                }

                
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar deletar evento: {(ex.InnerException ?? ex).Message}");
            }
        }


        [HttpPost("upload-image/{eventoId}")]
        public async Task<IActionResult> UploadImage(int eventoId)
        {

            var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), eventoId, false);
            if (evento == null) return NoContent();

            var file = Request.Form.Files[0];
            if (file.Length > 0)
            {
                DeleteImage(evento.ImageUrl);
                evento.ImageUrl = await SaveImage(file);
            }

            var eventoRetorno = await _eventoService.Update(User.GetUserId(), eventoId, evento);

            return Ok(true);
        }

        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            var imageName = new string(Path.GetFileNameWithoutExtension(imageFile.FileName)
                            .Take(10).ToArray())
            .Replace(' ', '-');

            imageName = $"{imageName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(imageFile.FileName)}";

            var imagePath = Path.Combine(_env.ContentRootPath, @"Resources\Images", imageName);

            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return imageName;
        }

        [NonAction]
        public void DeleteImage(string imageUrl)
        {
            var imagePath = Path.Combine(_env.ContentRootPath, @"Resources\Images", imageUrl);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }
    }
}
