using Microsoft.VisualBasic;
using System;

namespace SistemaClientesBatia.DTOs
{
    public class SupervisionDTO
    {
        public int IdSupervision { get; set; }
        public string Fecha { get; set; }
        public string Cliente { get; set; }
        public string Inmueble { get; set; }
        public string Usuario { get; set; }
        public string Entrevista { get; set; }
        public string ClienteNombre { get; set; }
        public string Calificacion { get; set; }
    }
}
