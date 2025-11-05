using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class Usuario
    {
        public long Id { get; set; }
        public string Nickname { get; set; }
        public string Contrasena { get; set; }
        public string Email { get; set; }
        public Rol Rol { get; set; }
    }
}
