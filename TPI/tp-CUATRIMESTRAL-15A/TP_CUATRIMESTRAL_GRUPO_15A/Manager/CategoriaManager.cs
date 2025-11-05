using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clases;


namespace Manager
{
    public class CategoriaManager
    {

        public List<Categoria> Listar()
        {
            List<Categoria> lista = new List<Categoria>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("SELECT ID, NOMBRE, DESCRIPCION FROM CATEGORIA WHERE ACTIVO = 1");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Categoria aux = new Categoria();
                    aux.Id = Convert.ToInt64(datos.Lector["ID"]);
                    aux.Nombre = datos.Lector["NOMBRE"].ToString();
                    aux.Descripcion = datos.Lector["DESCRIPCION"] == DBNull.Value ? "" : datos.Lector["DESCRIPCION"].ToString();
                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar categorías: " + ex.Message);
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
                datos.SetearConsulta("INSERT INTO CATEGORIA (NOMBRE, DESCRIPCION) VALUES (@Nombre, @Descripcion)");
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
                datos.SetearConsulta("UPDATE CATEGORIA SET ACTIVO = 0 WHERE ID = @Id");
                datos.SetearParametro("@Id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar categoría: " + ex.Message);
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }



    }
}
