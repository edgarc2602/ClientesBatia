using System;
using SistemaClientesBatia.Enums;

namespace SistemaClientesBatia.Models
{
    public class Ticket
    {
        public int IdClienteTicket { get; set; }
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public string Email { get; set; }
        public string Descripcion { get; set; }
        public int IdCategoria { get; set; }
        public string Categoria { get; set; }
        public PrioridadTicket IdPrioridad { get; set; }
        public EstatusTicket IdStatus { get; set; }
        public DateTime FechaAlta { get; set; }
        public int IdCliente { get; set; }
    }
}
