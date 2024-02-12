using System.Collections.Generic;

namespace SistemaClientesBatia.DTOs
{
    public class DashboardDTO
    {
        public decimal Asistencia { get; set; }
        public int Entregas { get; set; }
        public int Supervision { get; set; }
        public int Evaluaciones { get; set; }
        public List<SucursalesDTO> Sucursales { get; set; }
        public List<AsistenciaMesDTO> AsistenciaMes { get; set; }
        public List<IncidenciaDTO> Incidencia { get; set; }
    }
}
