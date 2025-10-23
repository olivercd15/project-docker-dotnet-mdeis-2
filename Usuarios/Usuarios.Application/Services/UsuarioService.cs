using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usuarios.Application.Services
{
    public class UsuarioService
    {
        public IEnumerable<object> GetUsuarios()
        {
            return new[]
            {
                new { Id = 1, Nombre = "Oliver Carranza", Email = "ocarranza@novasoft.com" },
                new { Id = 2, Nombre = "David Ripalda", Email = "dripalda@novasoft.com" },
                new { Id = 3, Nombre = "Juan Pesoa", Email = "jpesoa@novasoft.com" },
                new { Id = 4, Nombre = "Cesar Mamani", Email = "cmamani@novasoft.com" },
                new { Id = 5, Nombre = "Jose Gonzales", Email = "jgonzales@novasoft.com" }
            };
        }
    }
}
