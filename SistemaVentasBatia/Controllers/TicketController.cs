using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaClientesBatia.DTOs;
using SistemaClientesBatia.Models;
using SistemaClientesBatia.Services;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace SistemaClientesBatia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _logic;

        public TicketController(ITicketService logic)
        {
            _logic = logic;
        }
        [HttpPost("[action]")]
        public async Task<bool> GuardarTicket(TicketDTO ticket)
        {
            return await _logic.GuardarTicket(ticket);
        }

        [HttpGet("[action]/{idPrioridad}/{idCategoria}/{idStatus}/{idCliente}/{pagina}")]
        public async Task<ActionResult<ListaTicketDTO>> ObtenerListaTickets(int idPrioridad, int idCategoria, int idStatus, int idCliente = 0, int pagina = 1)
        {
            var listaTicket = new ListaTicketDTO()
            {
                Pagina = pagina,
                IdPrioridad = idPrioridad,
                IdCategoria = idCategoria,
                IdStatus = idStatus
            };
            await _logic.ObtenerListaTickets(listaTicket, idCliente);
            return listaTicket;
        }

        [HttpGet("[action]/{idClienteTicket}/{idCliente}")]
        public async Task<bool> CerrarTicket(int idClienteTicket, int idCliente)
        {
            return await _logic.CerrarTicket(idClienteTicket, idCliente);
        }
    }
}
