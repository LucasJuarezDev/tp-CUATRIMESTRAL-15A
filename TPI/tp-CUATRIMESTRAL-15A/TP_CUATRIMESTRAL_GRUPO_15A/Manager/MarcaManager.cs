using Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class MarcaManager
    {
        public List<Marca> Listar()
        {
            List<Marca> lista = new List<Marca>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("SELECT ID, NOMBRE, DESCRIPCION FROM MARCA WHERE ACTIVO = 1");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Marca aux = new Marca();
                    aux.Id = Convert.ToInt64(datos.Lector["ID"]);
                    aux.Nombre = datos.Lector["NOMBRE"].ToString();
                    aux.Descripcion = datos.Lector["DESCRIPCION"] == DBNull.Value ? "" : datos.Lector["DESCRIPCION"].ToString();
                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar marcas: " + ex.Message);
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }

        public void Agregar(string nombre, string descripcion)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("INSERT INTO MARCA (NOMBRE, DESCRIPCION) VALUES (@Nombre, @Descripcion)");
                datos.SetearParametro("@Nombre", nombre);
                datos.SetearParametro("@Descripcion", descripcion);
                datos.ejecutarAccion();
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

        public void EliminarLogico(long id)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("UPDATE MARCA SET ACTIVO = 0 WHERE ID = @Id");
                datos.SetearParametro("@Id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar marca: " + ex.Message);
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }

    }
}
