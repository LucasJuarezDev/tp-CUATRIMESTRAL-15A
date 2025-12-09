using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class DetalleVenta
    {
        public long IdVenta { get; set; }
        public long IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public Producto Producto { get; set; }
        public decimal Subtotal => Cantidad * PrecioUnitario;
    }
}
