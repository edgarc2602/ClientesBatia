using AutoMapper;
using SistemaClientesBatia.Controllers;
using SistemaClientesBatia.DTOs;
using SistemaClientesBatia.Models;
using SistemaClientesBatia.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace SistemaClientesBatia.Services
{
    public interface ITicketService
    {
        //Task<ListadoEstadoDeCuentaDTO> GetEstadoDeCuenta(ListadoEstadoDeCuentaDTO estadodecuenta, int idProveedor);
        Task<bool> GuardarTicket(TicketDTO ticketM);
        Task ObtenerListaTickets(ListaTicketDTO listaTicket, int idCliente);
        Task<bool> CerrarTicket(int idClienteTicket, int idCliente);
    }

    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _TicketRepo;
        private readonly IMapper _mapper;

        public TicketService(ITicketRepository TicketRepo, IMapper mapper)
        {
            _TicketRepo = TicketRepo;
            _mapper = mapper;
        }
        public async Task<bool> GuardarTicket(TicketDTO ticketM)
        {
            var ticket = _mapper.Map<Ticket>(ticketM);
            return await _TicketRepo.GuardarTicket(ticket);
        }

        public async Task ObtenerListaTickets(ListaTicketDTO listaTicket, int idCliente)
        {
            listaTicket.Rows = await _TicketRepo.ContarTickets(listaTicket.IdPrioridad, listaTicket.IdCategoria, listaTicket.IdStatus, idCliente);

            if (listaTicket.Rows > 0)
            {
                listaTicket.NumPaginas = (listaTicket.Rows / 10);

                if (listaTicket.Rows % 10 > 0)
                {
                    listaTicket.NumPaginas++;
                }
                var lista = await _TicketRepo.ObtenerTickets(listaTicket.Pagina, listaTicket.IdPrioridad, listaTicket.IdCategoria, listaTicket.IdStatus, idCliente);
                listaTicket.Tickets = lista.Select(c =>
                new TicketMinDTO
                {
                    IdClienteTicket = c.IdClienteTicket,
                    Nombre = c.Nombre,
                    Paterno = c.Paterno,
                    Materno = c.Materno,
                    Email = c.Email,
                    Descripcion = c.Descripcion,
                    Categoria = c.Categoria,
                    Prioridad = c.IdPrioridad.ToString(),
                    Status = c.IdStatus.ToString(),
                    FechaAlta = c.FechaAlta,
                    IdCliente = c.IdCliente
                }).ToList();
            }
            else
            {
                listaTicket.Tickets = new List<TicketMinDTO>();
            }
        }
        public async Task<bool> CerrarTicket(int idClienteTicket, int idCliente)
        {
            return await _TicketRepo.CerrarTicket(idClienteTicket, idCliente);
        }
    }
}
