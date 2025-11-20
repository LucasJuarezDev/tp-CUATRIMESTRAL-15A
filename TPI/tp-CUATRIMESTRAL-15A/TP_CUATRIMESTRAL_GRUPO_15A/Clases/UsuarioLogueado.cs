using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class UsuarioLogueado
    {
        public long Id { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public Rol Rol { get; set; }
        public Cliente Cliente { get; set; }
        public Empleado Empleado { get; set; }
    }
}
