using System.Collections.Generic;

namespace SistemaClientesBatia.DTOs
{
    public class ListaTicketDTO
    {
        public List<TicketMinDTO> Tickets { get; set; }
        public int Pagina { get; set; }
        public int NumPaginas { get; set; }
        public int Rows { get; set; }
        public int IdPrioridad { get; set; }
        public int IdCategoria { get; set; }
        public int IdStatus { get; set; }
    }
}
