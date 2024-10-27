using System;

namespace SistemaClientesBatia.DTOs
{
    public class RegistroAsistenciaDTO
    {
        public string Fecha {  get; set; }
        public int IdEmpleado { get; set; }
        public string Inmueble { get; set; }
        public string Puesto { get; set; }
        public string Nombre { get; set; }
        public string Nss { get; set; }
        public DateTime HoraEntrada { get; set; }
        public DateTime HoraSalida { get; set; }
        public bool Jornal { get; set; }
        public string Movimiento { get; set; }
        public int IdTurno { get; set; }
        public string Turno { get; set; }

    }

    public class EmpleadoNocturno
    {
        public int IdEmpleado { get; set; }
    }
}
