
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Models;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        private IEnumerable<Evento> _eventos = new Evento[] {
                new Evento() {
                EventoId = 1,
                Tema = "Angular 11 e .NET 5",
                Local = "São Paulo",
                Lote = "1º Lote",
                QtdPessoas = 100,
                DataEvento = DateTime.Now.AddDays(2).ToString(),
                ImageUrl = "Image1.png"
                },
                new Evento() {
                EventoId = 2,
                Tema = "Angular 11 e .NET 5",
                Local = "São Paulo",
                Lote = "1º Lote",
                QtdPessoas = 100,
                DataEvento = DateTime.Now.AddDays(2).ToString(),
                ImageUrl = "Image2.png"
                }

            };
        public EventoController()
        {

        }

        [HttpGet]
        public IEnumerable<Evento> Get()
        {
            return _eventos;
        }

        [HttpGet("{id}")]
        public IEnumerable<Evento> Get(int id)
        {
            return _eventos.Where(x=>x.EventoId == id);
        }

        [HttpPost]
        public string Post()
        {
            return "Exemplo de Post";
        }

        [HttpPut("{id}")]
        public string Put(int id)
        {
            return $"Exemplo de Put com id = {id}";
        }

        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            return $"Exemplo de Delete com id = {id}";
        }
    }
}
