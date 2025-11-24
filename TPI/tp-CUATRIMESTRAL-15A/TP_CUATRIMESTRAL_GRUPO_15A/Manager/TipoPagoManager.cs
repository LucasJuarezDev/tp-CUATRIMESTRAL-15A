using Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class TipoPagoManager
    {
        public List<TipoPago> Listar()
        {
            List<TipoPago> lista = new List<TipoPago>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("SELECT ID, NOMBRE, DESCRIPCION FROM TIPO_PAGO");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    TipoPago aux = new TipoPago();
                    aux.Id = (byte)datos.Lector["ID"];
                    aux.Nombre = datos.Lector["NOMBRE"].ToString();
                    aux.Descripcion = datos.Lector["DESCRIPCION"].ToString();

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }
    }
}
