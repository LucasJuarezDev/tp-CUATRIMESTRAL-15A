using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class ConfigManager
    {
            public decimal ObtenerDecimal(string clave, decimal valorPorDefecto = 0)
            {
                try
                {
                    AccesoDatos datos = new AccesoDatos();
                    datos.SetearConsulta("SELECT Valor FROM CONFIGURACION WHERE Clave = @clave");
                    datos.SetearParametro("@clave", clave);
                    datos.EjecutarLectura();
                    if (datos.Lector.Read())
                        return Convert.ToDecimal(datos.Lector["Valor"]);
                }
                catch { }
                return valorPorDefecto;
            }

            public string ObtenerString(string clave, string valorPorDefecto = "")
            {
                try
                {
                    AccesoDatos datos = new AccesoDatos();
                    datos.SetearConsulta("SELECT Valor FROM CONFIGURACION WHERE Clave = @clave");
                    datos.SetearParametro("@clave", clave);
                    datos.EjecutarLectura();
                    if (datos.Lector.Read())
                        return datos.Lector["Valor"].ToString();
                }
                catch { }
                return valorPorDefecto;
            }
     
    }
}
