using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaClientesBatia.DTOs;
using SistemaClientesBatia.Models;
using SistemaClientesBatia.Services;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace SistemaClientesBatia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluacionController : ControllerBase
    {
        private readonly IEvaluacionService _logic;

        public EvaluacionController(IEvaluacionService logic)
        {
            _logic = logic;
        }

        [HttpPost("[action]/{pagina}")]
        public async Task <ListaEvaluacionDTO> GetListaEvaluacion(ParamDashboardDTO param, int pagina = 1)
        {
            var listaEvaluacion = new ListaEvaluacionDTO()
            {
                Pagina = pagina,
            };
            return await _logic.GetListaEvaluacion(param, listaEvaluacion);
        }
    }
}
