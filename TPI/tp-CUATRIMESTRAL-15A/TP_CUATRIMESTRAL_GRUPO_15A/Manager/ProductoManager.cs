using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clases; 

namespace Manager
{
    public class ProductoManager
    {
        public List<Producto> Listar()
        {
            List<Producto> lista = new List<Producto>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("SELECT * FROM vw_ProductosActivos");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Producto prod = new Producto
                    {
                        Id = Convert.ToInt64(datos.Lector["ID"]),
                        Nombre = datos.Lector["NOMBRE"].ToString(),
                        Precio = Convert.ToDecimal(datos.Lector["PRECIO"]),
                        Descripcion = datos.Lector["DESCRIPCION"].ToString(),
                        Stock = Convert.ToInt32(datos.Lector["STOCK"]),
                        StockMinimo = Convert.ToInt32(datos.Lector["STOCK_MINIMO"]),

                        // MARCA
                        Marca = new Marca
                        {
                            Id = Convert.ToInt64(datos.Lector["IdMarca"]),
                            Nombre = datos.Lector["NombreMarca"].ToString(),
                            Descripcion = datos.Lector["DescripcionMarca"].ToString()
                        },

                        // CATEGORÍA
                        Categoria = new Categoria
                        {
                            Id = Convert.ToInt64(datos.Lector["IdCategoria"]),
                            Nombre = datos.Lector["NombreCategoria"].ToString(),
                            Descripcion = datos.Lector["DescripcionCategoria"].ToString()
                        }
                    };

                    lista.Add(prod);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }

        public void Eliminar(long id)
        {
            AccesoDatos accesoDatos = new AccesoDatos();
            try
            {
                accesoDatos.SetearConsulta("UPDATE PRODUCTO SET ACTIVO = 0 WHERE ID = @Id");
                accesoDatos.SetearParametro("@Id", id);
                accesoDatos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                accesoDatos.CerrarConeccion();
            }
        }
    }
}
