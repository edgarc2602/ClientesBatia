using SistemaClientesBatia.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace SistemaClientesBatia.DTOs
{
    public class TicketDTO
    {
        public int IdClienteTicket { get; set; }
        [Required]
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Descripcion { get; set; }
        [Required]
        public int IdCategoria { get; set; }
        public string Categoria { get; set; }
        [Required]
        public PrioridadTicket IdPrioridad { get; set; }
        public EstatusTicket IdStatus { get; set; }
        public DateTime FechaAlta { get; set; }
        [Required]
        public int IdCliente { get; set; }
    }
}
