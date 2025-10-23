using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ventas.Application.Services;

namespace Ventas.WebAPI.Controllers
{
    [ApiController]
    [Route("api/ventas")]
    public class VentasController : ControllerBase
    {
        private readonly VentaService _ventaService;

        public VentasController(VentaService ventaService)
        {
            _ventaService = ventaService;
        }

        // GET: api/ventas
        [HttpGet("")]
        public IActionResult GetVentas()
        {
            var ventas = _ventaService.GetVentas();
            return Ok(ventas);
        }

        // GET: api/ventas/info
        [HttpGet("info")]
        public async Task<IActionResult> GetVentasWithInfo()
        {
            var ventas = await _ventaService.GetVentasWithInfo();
            return Ok(ventas);
        }
    }
}
