using System.Collections.Generic;

namespace SistemaClientesBatia.DTOs
{
    public class ListaEvaluacionDTO
    {
        public List<EvaluacionDTO> Evaluaciones { get; set; }
        public int Pagina { get; set; }
        public int NumPaginas { get; set; }
        public int Rows { get; set; }

    }
}
