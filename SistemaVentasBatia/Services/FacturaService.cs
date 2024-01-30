using AutoMapper;
using Microsoft.AspNetCore.Http;
using SistemaClientesBatia.Controllers;
using SistemaClientesBatia.DTOs;

using SistemaClientesBatia.Models;
using SistemaClientesBatia.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace SistemaClientesBatia.Services
{
    public interface IFacturaService
    {
        //Task<ListadoOrdenCompraDTO> ObtenerOrdenesCompra(ListadoOrdenCompraDTO ordenescompra, int idProveedor, string fechaInicio, string fechaFin, int idStatus);
        //Task<decimal> ObtenerSumaFacturas(int idOrden);
        //Task<List<FacturaDTO>> ObtenerFacturas(int idOrden);
    }

    public class FacturaService : IFacturaService
    {
        private readonly IFacturaRepository _FacturaRepo;
        private readonly IMapper _mapper;

        public FacturaService(IFacturaRepository FacturaRepo, IMapper mapper)
        {
            _FacturaRepo = FacturaRepo;
            _mapper = mapper;
        }

        //public async Task<ListadoOrdenCompraDTO> ObtenerOrdenesCompra(ListadoOrdenCompraDTO ordenescompra, int idProveedor, string fechaInicio, string fechaFin, int idStatus)
        //{
        //    ordenescompra.Rows = await _FacturaRepo.ContarOrdenesCompra(idProveedor, fechaInicio, fechaFin, idStatus);
        //    if (ordenescompra.Rows > 0)
        //    {
        //        ordenescompra.NumPaginas = (ordenescompra.Rows / 40);

        //        if (ordenescompra.Rows % 40 > 0)
        //        {
        //            ordenescompra.NumPaginas++;
        //        }
        //        ordenescompra.Ordenes = _mapper.Map<List<OrdenCompraDTO>>(await _FacturaRepo.ObtenerOrdenesCompra(idProveedor, fechaInicio, fechaFin, ordenescompra.Pagina,idStatus));
        //    }
        //    else
        //    {
        //        ordenescompra.Ordenes = new List<OrdenCompraDTO>();
        //    }
        //    return ordenescompra;
        //}

        //public async Task<decimal> ObtenerSumaFacturas(int idOrden)
        //{
        //    return await _FacturaRepo.ObtenerSumaFacturas(idOrden);
        //}

        //public async Task<List<FacturaDTO>> ObtenerFacturas(int idOrden)
        //{
        //    _ = new List<FacturaDTO>();
        //    List<FacturaDTO> facturas = _mapper.Map<List<FacturaDTO>>(await _FacturaRepo.ObtenerFacturas(idOrden));
        //    return facturas;
        //}

        
    }
}
