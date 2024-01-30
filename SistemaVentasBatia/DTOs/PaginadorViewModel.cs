using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaClientesBatia.DTOs
{
    public class PaginadorViewModel
    {
        public string Controlador { get; set; }

        public string Action { get; set; }

        public int Pagina { get; set; }

        public int NumPaginas { get; set; }

        public int Rows { get; set; }

    }
}
