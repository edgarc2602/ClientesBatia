using AutoMapper;
using SistemaClientesBatia.Controllers;
using SistemaClientesBatia.DTOs;
using SistemaClientesBatia.Models;
using SistemaClientesBatia.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace SistemaClientesBatia.Services
{
    public interface ISupervisionService
    {
        Task<ListaSupervisionDTO> GetListaSupervision(ParamDashboardDTO param, ListaSupervisionDTO listaSupervision);
    }

    public class SupervisionService : ISupervisionService
    {
        private readonly ISupervisionRepository _SupervisionRepo;
        private readonly IMapper _mapper;

        public SupervisionService(ISupervisionRepository SupervisionRepo, IMapper mapper)
        {
            _SupervisionRepo = SupervisionRepo;
            _mapper = mapper;
        }

        public async Task<ListaSupervisionDTO> GetListaSupervision(ParamDashboardDTO param, ListaSupervisionDTO listaSupervision)
        {
            listaSupervision.Rows = await _SupervisionRepo.ContarSupervisiones(param);
            if (listaSupervision.Rows > 0)
            {
                listaSupervision.NumPaginas = (listaSupervision.Rows / 40);

                if (listaSupervision.Rows % 40 > 0)
                {
                    listaSupervision.NumPaginas++;
                }
                var lista = await _SupervisionRepo.ObtenerSupervisiones(param.Mes, param.Anio, param.IdCliente, listaSupervision.Pagina, param.IdInmueble);
                listaSupervision.Supervisiones = _mapper.Map<List<SupervisionDTO>>(lista);
            }
            else
            {
                listaSupervision.Supervisiones = new List<SupervisionDTO>();
            }
            return listaSupervision;
        }
    }
}
