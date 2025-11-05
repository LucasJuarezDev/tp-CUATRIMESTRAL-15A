using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class Compra
    {
        public long Id { get; set; }
        public DateTime FechaCompra { get; set; }
        public decimal MontoTotal { get; set; }
        public TipoPago TipoPago { get; set; }
        public Proveedor Proveedor { get; set; }
    }
}
