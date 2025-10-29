using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class Venta
    {
        public long Id { get; set; }
        public DateTime FechaVenta { get; set; }
        public decimal MontoTotal { get; set; }
        public TipoPago TipoPago { get; set; }
        public Cliente Cliente { get; set; }
        public string NumeroFactura { get; set; }
        public Empleado Empleado { get; set; }
    }
}
