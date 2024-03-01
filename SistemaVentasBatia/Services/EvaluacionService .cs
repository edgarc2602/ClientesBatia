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
    public interface IEvaluacionService
    {
        Task<ListaEvaluacionDTO> GetListaEvaluacion(ParamDashboardDTO param, ListaEvaluacionDTO listaEvaluacion);
    }

    public class EvaluacionService : IEvaluacionService
    {
        private readonly IEvaluacionRepository _EvaluacionRepo;
        private readonly IMapper _mapper;

        public EvaluacionService(IEvaluacionRepository EvaluacionRepo, IMapper mapper)
        {
            _EvaluacionRepo = EvaluacionRepo;
            _mapper = mapper;
        }

        public async Task<ListaEvaluacionDTO> GetListaEvaluacion(ParamDashboardDTO param, ListaEvaluacionDTO listaEvaluacion)
        {
            listaEvaluacion.Rows = await _EvaluacionRepo.ContarEvaluaciones(param);
            if (listaEvaluacion.Rows > 0)
            {
                listaEvaluacion.NumPaginas = (listaEvaluacion.Rows / 40);

                if (listaEvaluacion.Rows % 40 > 0)
                {
                    listaEvaluacion.NumPaginas++;
                }
                var lista = await _EvaluacionRepo.ObtenerEvaluaciones(param.Mes, param.Anio, param.IdCliente, listaEvaluacion.Pagina, param.IdInmueble);
                listaEvaluacion.Evaluaciones = _mapper.Map<List<EvaluacionDTO>>(lista);
            }
            else
            {
                listaEvaluacion.Evaluaciones = new List<EvaluacionDTO>();
            }
            return listaEvaluacion;
        }
    }
}
