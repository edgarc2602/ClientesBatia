using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaClientesBatia.DTOs;
using SistemaClientesBatia.Models;
using SistemaClientesBatia.Services;
using SistemaProveedoresBatia.DTOs;
using System.Collections.Generic;
using System.Net.NetworkInformation;
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

        [HttpPost("[action]/{idStatus}/{pagina}")]
        public async Task<ListadoMaterialDTO> ObtenerListados(ParamDashboardDTO param, int idStatus, int pagina)
        {
            ListadoMaterialDTO listados = new ListadoMaterialDTO
            {
                Pagina = pagina
            };
            return await _logic.ObtenerListados(listados, param, idStatus);
        }

        private readonly string _imageFolderPath = "\\\\192.168.2.4\\c$\\inetpub\\wwwroot\\SINGA_APP\\Doctos\\entrega\\";

        [HttpGet("getimage/{archivo}/{carpeta}")]
        public IActionResult GetImage(string archivo, string carpeta)
        {
            string imagePath = _imageFolderPath + carpeta + '/' + archivo;

            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound();
            }
            var imageBytes = System.IO.File.ReadAllBytes(imagePath);
            return File(imageBytes, "image/jpeg");
        }
    }
}
