using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SistemaClientesBatia.DTOs;
using SistemaClientesBatia.Services;


namespace SistemaClientesBatia.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogoController : ControllerBase
    {
        private readonly ICatalogosService logic;

        public CatalogoController(ICatalogosService service)
        {
            logic = service;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<CatalogoDTO>> ObtenerMeses()
        {
            return await logic.ObtenerMeses();
        }
    }
}