using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        public EventoController()
        {
           
        }

        [HttpGet]
        public string Get()
        {
            return  "Exemplo de Get";
        }

        [HttpPost]
        public string Post()
        {
            return  "Exemplo de Post";
        }

        [HttpPut("{id}")]
        public string Put(int id)
        {
            return  $"Exemplo de Put com id = {id}";
        }

        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            return  $"Exemplo de Delete com id = {id}";
        }
    }
}
