using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaClientesBatia.Enums;

namespace SistemaClientesBatia.Models
{
    public class UsuarioRegistro
    {

        public int IdAutorizacionVentas { get; set; }
        public int IdPersonal { get; set; }
        public int Autoriza { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Puesto { get; set; }
        public string Telefono { get; set; }
        public string TelefonoExtension { get; set; }
        public string TelefonoMovil { get; set; }
        public string Email { get; set; }
        public string Firma { get; set; }
        public int Revisa { get; set; }
    }
}
