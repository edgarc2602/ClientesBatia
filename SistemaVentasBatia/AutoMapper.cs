using AutoMapper;
using SistemaClientesBatia.Models;
using SistemaClientesBatia.DTOs;
using SistemaClientesBatia.Enums;

namespace SistemaClientesBatia
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Catalogo, CatalogoDTO>();
            CreateMap<CatalogoDTO, Catalogo>();
            CreateMap<AccesoDTO, Acceso>();
            CreateMap<Acceso, AccesoDTO>();
            CreateMap<UsuarioDTO, Usuario>();
            CreateMap<Usuario, UsuarioDTO>();
            CreateMap<TicketDTO, Ticket>();
            CreateMap<Ticket, TicketDTO>();
            CreateMap<Sucursales,SucursalesDTO>();
            CreateMap<SucursalesDTO,Sucursales>();
            CreateMap<AsistenciaMes,AsistenciaMesDTO>();
            CreateMap<AsistenciaMesDTO,AsistenciaMes>();
            CreateMap<Incidencia,IncidenciaDTO>();
            CreateMap<IncidenciaDTO,Incidencia>();
            CreateMap<Evaluacion,EvaluacionDTO>();
            CreateMap<EvaluacionDTO,Evaluacion>();
            CreateMap<Supervision,SupervisionDTO>();
            CreateMap<SupervisionDTO,Supervision>();
        }
    }
}