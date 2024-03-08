using AutoMapper;
using SistemaClientesBatia.Controllers;
using SistemaClientesBatia.DTOs;
using SistemaClientesBatia.Models;
using SistemaClientesBatia.Repositories;
using SistemaProveedoresBatia.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace SistemaClientesBatia.Services
{
    public interface IEntregaService
    {
        Task<ListadoMaterialDTO> ObtenerListados(ListadoMaterialDTO listados, ParamDashboardDTO param, int idStatus);
    }
    public class EntregaService : IEntregaService
    {
        private readonly IEntregaRepository _EntregaRepo;
        private readonly IMapper _mapper;

        public EntregaService(IEntregaRepository EntregaRepo, IMapper mapper)
        {
            _EntregaRepo = EntregaRepo;
            _mapper = mapper;
        }

        public async Task<ListadoMaterialDTO> ObtenerListados(ListadoMaterialDTO listados, ParamDashboardDTO param, int idStatus)
        {
            listados.Rows = await _EntregaRepo.ContarListados(param.Mes, param.Anio, param.IdCliente, param.IdInmueble, idStatus);
            if (listados.Rows > 0)
            {
                listados.NumPaginas = (listados.Rows / 40);

                if (listados.Rows % 40 > 0)
                {
                    listados.NumPaginas++;
                }
                listados.Listas = _mapper.Map<List<ListadosDTO>>(await _EntregaRepo.ObtenerListados(param.Mes, param.Anio, param.IdCliente, param.IdInmueble, idStatus, listados.Pagina));
            }
            else
            {
                listados.Listas = new List<ListadosDTO>();
            }
            return listados;
        }
    }
    
}
