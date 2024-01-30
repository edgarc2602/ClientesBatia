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
    public class CuentaController : ControllerBase
    {
        private readonly ICuentaService _logic;

        public CuentaController(ICuentaService logic)
        {
            _logic = logic;
        }
    }
}
