using Inventario.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Inventario.WebAPI.Controllers
{
    [ApiController]
    [Route("api/productos")]
    public class ProductoController : ControllerBase
    {
        private readonly ProductoService _productoService;

        public ProductoController(ProductoService productoService)
        {
            _productoService = productoService;
        }

        // GET: api/productos
        [HttpGet("")]
        public IActionResult GetProductos()
        {
            var productos = _productoService.GetProductos();
            return Ok(productos);
        }
    }
}
