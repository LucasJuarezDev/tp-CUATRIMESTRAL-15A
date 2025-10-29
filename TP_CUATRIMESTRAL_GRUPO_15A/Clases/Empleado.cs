﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class Empleado
    {
        public long Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaIngreso { get; set; }
        public decimal Sueldo { get; set; }
        public Usuario Usuario { get; set; }
    }
}
