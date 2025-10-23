using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventario.Application.Services
{
    public class ProductoService
    {
        public IEnumerable<object> GetProductos()
        {
            return new[]
            {
                new { Id = 1, Nombre = "Laptop Dell Inspiron", Precio = 4200.50m },
                new { Id = 2, Nombre = "Mouse Logitech G Pro", Precio = 320.99m },
                new { Id = 3, Nombre = "Teclado Mecánico Redragon", Precio = 480.75m },
                new { Id = 4, Nombre = "Monitor Samsung 27 pulg.", Precio = 1590.00m },
                new { Id = 5, Nombre = "Auriculares HyperX Cloud II", Precio = 890.40m }
            };
        }
    }
}
