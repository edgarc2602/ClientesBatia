using AutoMapper;
using SistemaClientesBatia.Controllers;
using SistemaClientesBatia.DTOs;
using SistemaClientesBatia.Models;
using SistemaClientesBatia.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SistemaClientesBatia.Services
{
    public interface IEntregaService
    {
        //Task<ListadoMaterialDTO> ObtenerListados(ListadoMaterialDTO listados, int mes, int anio, int idProveedor, int idEstado, int tipo, int idStatus);
        
    }

    public class EntregaService : IEntregaService
    {
        private readonly IEntregaRepository _entregaRepo;
        private readonly IMapper _mapper;

        public EntregaService(IEntregaRepository entregaRepo, IMapper mapper)
        {
            this._entregaRepo = entregaRepo;
            this._mapper = mapper;
        }
        //public async Task<ListadoMaterialDTO> ObtenerListados(ListadoMaterialDTO listados, int mes, int anio, int idProveedor, int idEstado, int tipo, int idStatus)
        //{
        //    listados.Rows = await _entregaRepo.ContarListados(mes, anio, idProveedor, idEstado, tipo,idStatus);
        //    if (listados.Rows > 0)
        //    {
        //        listados.NumPaginas = (listados.Rows / 40);

        //        if (listados.Rows % 40 > 0)
        //        {
        //            listados.NumPaginas++;
        //        }
        //        listados.Listas = _mapper.Map<List<ListadosDTO>>(await _entregaRepo.ObtenerListados(mes, anio, idProveedor, idEstado, tipo, listados.Pagina, idStatus));
        //    }
        //    else
        //    {
        //        listados.Listas = new List<ListadosDTO>();
        //    }
        //    return listados;
        //}

    }
}
