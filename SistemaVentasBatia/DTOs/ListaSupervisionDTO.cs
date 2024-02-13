using System.Collections.Generic;

namespace SistemaClientesBatia.DTOs
{
    public class ListaSupervisionDTO
    {
        public List<SupervisionDTO> Supervisiones { get; set; }
        public int Pagina { get; set; }
        public int NumPaginas { get; set; }
        public int Rows { get; set; }
    }
}