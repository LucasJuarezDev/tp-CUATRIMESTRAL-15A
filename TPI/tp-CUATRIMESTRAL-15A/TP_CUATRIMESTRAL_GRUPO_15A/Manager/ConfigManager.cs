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

        public void Guardar(string clave, string valor)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"
            IF EXISTS (SELECT 1 FROM CONFIGURACION WHERE Clave = @clave)
                UPDATE CONFIGURACION SET Valor = @valor WHERE Clave = @clave
            ELSE
                INSERT INTO CONFIGURACION (Clave, Valor) VALUES (@clave, @valor)
        ");
                datos.SetearParametro("@clave", clave);
                datos.SetearParametro("@valor", valor);
                datos.ejecutarAccion();
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }

        public string ObtenerEmailAdmin()
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.SetearConsulta(@"
            SELECT u.EMAIL 
            FROM USUARIO u
            INNER JOIN ROL r ON u.ROLE_ID = r.ID
            WHERE r.ROL = 'ADMIN' AND u.ACTIVO = 1
        ");
                datos.EjecutarLectura();
                if (datos.Lector.Read())
                    return datos.Lector["EMAIL"]?.ToString() ?? "";
            }
            catch { }
            return "";
        }

        public void ActualizarEmailAdmin(string nuevoEmail)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"
            UPDATE USUARIO 
            SET EMAIL = @email 
            WHERE ROLE_ID = (SELECT ID FROM ROL WHERE ROL = 'ADMIN') 
            AND ACTIVO = 1
        ");
                datos.SetearParametro("@email", nuevoEmail);
                datos.ejecutarAccion();
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }
    }
}
