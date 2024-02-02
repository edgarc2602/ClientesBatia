using SistemaClientesBatia.Enums;
using System;

namespace SistemaClientesBatia.DTOs
{
    public class TicketMinDTO
    {
        public int IdClienteTicket { get; set; }
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public string Email { get; set; }
        public string Descripcion { get; set; }
        public string Categoria { get; set; }
        public string Prioridad { get; set; }
        public string Status { get; set; }
        public DateTime FechaAlta { get; set; }
        public int IdCliente { get; set; }
    }
}
