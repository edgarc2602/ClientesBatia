using AutoMapper;
using SistemaClientesBatia.Models;
using SistemaClientesBatia.Repositories;
using SistemaClientesBatia.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaClientesBatia.Enums;
using Microsoft.Extensions.Options;
using SistemaClientesBatia.Controllers;

namespace SistemaClientesBatia.Services
{
    public interface ICatalogosService
    {
        Task<List<CatalogoDTO>> ObtenerMeses();
        Task<List<CatalogoDTO>> GetPrioridadTK();
        Task<List<CatalogoDTO>> GetStatusTK();
        Task<List<CatalogoDTO>> GetCategoriaTK();
    }

    public class CatalogosService : ICatalogosService
    {
        private readonly ICatalogosRepository catalogosRepo;
        private readonly IMapper mapper;

        public CatalogosService(ICatalogosRepository catalogosRepo, IMapper mapper)
        {
            this.catalogosRepo = catalogosRepo;
            this.mapper = mapper;
        }

        public async Task<List<CatalogoDTO>> ObtenerMeses()
        {
            var meses = mapper.Map<List<CatalogoDTO>>(await catalogosRepo.ObtenerMeses());
            return meses;
        }

        public async Task <List<CatalogoDTO>> GetPrioridadTK()
        {
            var prioridades = mapper.Map <List<CatalogoDTO>> (await catalogosRepo.GetPrioridadTK());
            return prioridades;
        }

        public async Task<List<CatalogoDTO>> GetStatusTK()
        {
            var prioridades = mapper.Map<List<CatalogoDTO>>(await catalogosRepo.GetStatusTK());
            return prioridades;
        }

        public async Task<List<CatalogoDTO>> GetCategoriaTK()
        {
            var prioridades = mapper.Map<List<CatalogoDTO>>(await catalogosRepo.GetCategoriaTK());
            return prioridades;
        }
    }
}
