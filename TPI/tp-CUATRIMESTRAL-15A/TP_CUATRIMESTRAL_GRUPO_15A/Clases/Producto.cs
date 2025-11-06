using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class Producto
    {
        public long Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public string DescripcionCorta { get; set; }       
        public string DescripcionExtendida { get; set; }
        public int Stock { get; set; }
        public int StockMinimo { get; set; }
        public string ImagenUrl { get; set; }
        public Marca Marca { get; set; }
        public Categoria Categoria { get; set; }

        public bool Estado { get; set; }
    }
}
