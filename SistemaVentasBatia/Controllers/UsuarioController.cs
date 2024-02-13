using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaClientesBatia.DTOs;
using SistemaClientesBatia.Models;
using SistemaClientesBatia.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaClientesBatia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _logic;

        public UsuarioController(IUsuarioService logic)
        {
            _logic = logic;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<UsuarioDTO>> Login(AccesoDTO dto)
        {
            return await _logic.Login(dto);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<DashboardDTO>> GetDashboard(ParamDashboardDTO param)
        {
            return await _logic.GetDashboard(param);
        }

        [HttpGet("[action]/{idCliente}")]
        public async Task <List<SucursalesDTO>> GetSucursales (int idCliente)
        {
            return await _logic.GetSucursales(idCliente);
        }
    }
}
