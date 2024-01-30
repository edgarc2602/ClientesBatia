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

    public enum EstatusProspecto
    {
        Activo = 1,
        Inactivo = 2
    }

    public enum EstatusDireccion
    {
        Activo = 1,
        Inactivo = 2
    }

    public enum EstatusCotizacion
    {
        Activa = 1,
        Inactivo = 3
    }

    public enum Servicio
    {
        Mantenimiento = 1,
        Limpieza = 2,
        Sanitización = 3
    }

    public enum EstatusPuesto
    {
        Activo = 1,
        Inactivo = 2
    }

    public enum EstatusTurno
    {
        Activo = 1,
        Inactivo = 2
    }

    public enum TipoAlerta
    {
        Exito = 1,
        Error = 2,
        Aviso = 3,
        Info = 4
    }

    public enum EstatusTipoInmueble
    {
        Activo = 1,
        Inactivo = 2
    }

    public enum DiaSemana
    {
        Lunes = 1,
        Martes = 2,
        Miércoles = 3,
        Jueves = 4,
        Viernes = 5,
        Sábado = 6,
        Domingo = 7
    }

    public enum Frecuencia
    {
        Mensual = 1,
        Bimestral = 2,
        Trimestral = 3,
        Cuatrimestral = 4,
        Semestral = 6,
        Anual = 12,
        Bienal = 24
    }

    [Flags]
    public enum Prenda
    {
        Camisa = 1,
        Pantalon = 2,
        Botas = 4,
        Gabardina = 8,
        Polo = 16
    }

    public enum Turno
    {
        MATUTINO = 1,
        VESPERTINO = 2,
        NOCTURNO = 3,
        MIXTO = 4,
        MEDIO = 5,
        CUARTO = 6,
        TURNOYMEDIO = 8,
        VEINTICUATRO = 9,
        COMPLETO = 10
    }
}
