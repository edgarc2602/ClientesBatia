using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaClientesBatia.Enums
{
    [Flags]
    public enum Documento
    {
        RFC = 1,
        PoderNotarial = 2
    }


    public enum Servicio
    {
        Mantenimiento = 1,
        Limpieza = 2,
        Sanitización = 3
    }

    public enum EstatusTicket
    {
        Alta = 1,
        Recibido = 2,
        Atendido = 3,
        Cerrado = 4
    }

    public enum PrioridadTicket
    {
        Normal = 1,
        Urgente = 2
    }
    public enum TipoAlerta
    {
        Exito = 1,
        Error = 2,
        Aviso = 3,
        Info = 4
    }

}
