using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaClientesBatia.DTOs;
using SistemaClientesBatia.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SistemaClientesBatia.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class EntregaController : ControllerBase
    {


        private readonly IEntregaService _logic;

        public EntregaController(IEntregaService logic)
        {
            _logic = logic;
        }

        
    }
}
