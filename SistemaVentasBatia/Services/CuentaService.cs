using AutoMapper;
using SistemaClientesBatia.Controllers;
using SistemaClientesBatia.DTOs;
using SistemaClientesBatia.Models;
using SistemaClientesBatia.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaClientesBatia.Services
{
    public interface ICuentaService
    {
        //Task<ListadoEstadoDeCuentaDTO> GetEstadoDeCuenta(ListadoEstadoDeCuentaDTO estadodecuenta, int idProveedor);
    }

    public class CuentaService : ICuentaService
    {
        private readonly ICuentaRepository _CuentaRepo;
        private readonly IMapper _mapper;

        public CuentaService(ICuentaRepository CuentaRepo, IMapper mapper)
        {
            _CuentaRepo = CuentaRepo;
            _mapper = mapper;
        }

        //public async Task<ListadoEstadoDeCuentaDTO> GetEstadoDeCuenta(ListadoEstadoDeCuentaDTO estadodecuenta, int idProveedor)
        //{
        //    estadodecuenta.Rows = await _CuentaRepo.ContarEstadoDeCuenta(idProveedor);

        //    if (estadodecuenta.Rows > 0)
        //    {
        //        estadodecuenta.NumPaginas = (estadodecuenta.Rows / 40);

        //        if (estadodecuenta.Rows % 40 > 0)
        //        {
        //            estadodecuenta.NumPaginas++;
        //        }
        //        estadodecuenta.EstadosDeCuenta = _mapper.Map<List<EstadoDeCuentaDTO>>(await _CuentaRepo.GetEstadoDeCuenta(idProveedor, estadodecuenta.Pagina));
        //    }
        //    else
        //    {
        //        estadodecuenta.EstadosDeCuenta = new List<EstadoDeCuentaDTO>();
        //    }
        //    return estadodecuenta;
        //}
    }
}
