using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaProveedoresBatia.DTOs
{
    public class ListadosDTO
    {
        public int IdListado { get; set; }
        public string Estatus { get; set; }
        public string  FechaProgramada { get; set; }
        public string  FechaEntrega { get; set; }
        public int IdInmueble { get; set; }
        public string Inmueble { get; set; }
        public string  AcuseEntrega { get; set; }
        public string  Carpeta { get; set; }
    }
}
