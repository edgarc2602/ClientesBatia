using System;

namespace SistemaClientesBatia.DTOs
{
    public class RegistroAsistenciaDTO
    {
        public int IdEmpleado { get; set; }
        public string Nombre { get; set; }
        public DateTime HoraEntrada { get; set; }
        public DateTime HoraSalida { get; set; }
        public bool Jornal { get; set; }

    }
}
