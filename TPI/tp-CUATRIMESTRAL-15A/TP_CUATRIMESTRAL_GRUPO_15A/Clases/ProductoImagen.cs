using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class ProductoImagen
    {
        public long Id { get; set; }
        public long IdProducto { get; set; }
        public string UrlImagen { get; set; }
        public bool EsPrincipal { get; set; }
        public int Orden { get; set; }
    }
}
