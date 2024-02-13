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
    public class SupervisionController : ControllerBase
    {
        private readonly ISupervisionService _logic;

        public SupervisionController(ISupervisionService logic)
        {
            _logic = logic;
        }

        [HttpPost("[action]/{pagina}")]
        public async Task <ListaSupervisionDTO> GetListaSupervision(ParamDashboardDTO param, int pagina = 1)
        {
            var listaSupervision = new ListaSupervisionDTO()
            {
                Pagina = pagina,
            };
            return await _logic.GetListaSupervision(param, listaSupervision);
        }
    }
}
