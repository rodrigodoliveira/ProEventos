using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Contracts;
using ProEventos.Domain;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly IEventoService _eventoService;

        public EventosController(IEventoService eventoService)
        {
            this._eventoService = eventoService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var eventos = await _eventoService.GetAllEventosAsync(includePalestrantes: true);
                if (eventos == null || eventos.Length == 0)
                {
                    return NotFound("Nenhum evento econtrado");
                }

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
                var evento = await _eventoService.GetEventoByIdAsync(id, includePalestrantes: true);
                if (evento == null || evento.Id == 0)
                {
                    return NotFound("Evento não encontrado");
                }

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
                var evento = await _eventoService.GetAllEventosByTemaAsync(tema, includePalestrantes: true);
                if (evento == null || evento.Length == 0)
                {
                    return NotFound("Eventos por tema não encontrados");
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
        public async Task<IActionResult> Post(Evento model)
        {
            try
            {
                var evento = await _eventoService.Add(model);
                if (evento == null) return BadRequest("Erro ao tentar adicionar evento");

                return Ok(evento);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao adicionar evento: {(ex.InnerException ?? ex).Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Evento model)
        {
            try
            {
                var evento = await _eventoService.Update(id, model);
                if (evento == null || evento.Id == 0) return BadRequest("Erro ao atualizar evento");

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
                return await _eventoService.Remove(id)
                    ? Ok("Evento exclido com sucesso")
                    : BadRequest("Erro ao excluir evento");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar deletar evento: {(ex.InnerException ?? ex).Message}");
            }
        }
    }
}
