using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Ventas.Domain.Interfaces;

namespace Ventas.Application.Services
{
    public class VentaService
    {
        private readonly IAzureServiceBusClient _bus;
        private readonly string _usuariosQueue;
        private readonly string _inventarioQueue;

        public VentaService(IAzureServiceBusClient bus, string usuariosQueue, string inventarioQueue)
        {
            _bus = bus;
            _usuariosQueue = usuariosQueue;
            _inventarioQueue = inventarioQueue;
        }

        public IEnumerable<object> GetVentas()
        {
            return new[]
            {
                new { Id = 1, UsuarioId = 1, ProductoId = 1 },
                new { Id = 2, UsuarioId = 2, ProductoId = 2 },
                new { Id = 3, UsuarioId = 3, ProductoId = 3 },
                new { Id = 4, UsuarioId = 4, ProductoId = 4 },
                new { Id = 5, UsuarioId = 5, ProductoId = 5 }
            };
        }

        public async Task<IEnumerable<object>> GetVentasWithInfo()
        {
            var ventas = new[]
            {
                new { Id = 1, UsuarioId = 1, ProductoId = 1 },
                new { Id = 2, UsuarioId = 2, ProductoId = 2 },
                new { Id = 3, UsuarioId = 3, ProductoId = 3 },
                new { Id = 4, UsuarioId = 4, ProductoId = 4 },
                new { Id = 5, UsuarioId = 5, ProductoId = 5 }
            };

            await _bus.SendMessageAsync(_usuariosQueue, new { Action = "GetUsuarios" });
            var usuariosResponse = await _bus.ReceiveMessageAsync();

            await _bus.SendMessageAsync(_inventarioQueue, new { Action = "GetProductos" });
            var productosResponse = await _bus.ReceiveMessageAsync();

            var usuarios = usuariosResponse != null
                ? JsonSerializer.Deserialize<List<UsuarioDto>>(usuariosResponse)!
                : new List<UsuarioDto>();

            var productos = productosResponse != null
                ? JsonSerializer.Deserialize<List<ProductoDto>>(productosResponse)!
                : new List<ProductoDto>();


            var resultado = new List<object>();
            foreach (var v in ventas)
            {
                var usuario = usuarios.Find(u => (int)u.Id == v.UsuarioId);
                var producto = productos.Find(p => (int)p.Id == v.ProductoId);

                resultado.Add(new
                {
                    v.Id,
                    Usuario = usuario?.Nombre ?? "Error Usuario",
                    Producto = producto?.Nombre ?? "Error Producto",
                    Precio = producto?.Precio ?? 0
                });
            }

            return resultado;
        }
    }


    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Email { get; set; } = "";
    }

    public class ProductoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public decimal Precio { get; set; }
    }
}
