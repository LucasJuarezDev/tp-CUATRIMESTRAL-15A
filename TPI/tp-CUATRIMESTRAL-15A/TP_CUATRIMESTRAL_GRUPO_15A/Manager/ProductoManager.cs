using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
                        DescripcionCorta = datos.Lector["DESCRIPCION_CORTA"].ToString(),
                        DescripcionExtendida = datos.Lector["DESCRIPCION_EXTENDIDA"].ToString(),
                        Stock = Convert.ToInt32(datos.Lector["STOCK"]),
                        StockMinimo = Convert.ToInt32(datos.Lector["STOCK_MINIMO"]),
                        ImagenUrl = datos.Lector["IMAGEN_URL"]?.ToString(),

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

        public Producto BuscarPorId(long id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("SELECT * FROM vw_ProductosActivos WHERE ID = @Id");
                datos.SetearParametro("@Id", id);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    return new Producto
                    {
                        Id = Convert.ToInt64(datos.Lector["ID"]),
                        Nombre = datos.Lector["NOMBRE"]?.ToString(),
                        Precio = Convert.ToDecimal(datos.Lector["PRECIO"]),
                        DescripcionCorta = datos.Lector["DESCRIPCION_CORTA"].ToString(),
                        DescripcionExtendida = datos.Lector["DESCRIPCION_EXTENDIDA"].ToString(),
                        Stock = Convert.ToInt32(datos.Lector["STOCK"]),
                        StockMinimo = Convert.ToInt32(datos.Lector["STOCK_MINIMO"]),
                        ImagenUrl = datos.Lector["IMAGEN_URL"]?.ToString(),
                        Marca = new Marca { Id = Convert.ToInt64(datos.Lector["IdMarca"]), Nombre = datos.Lector["NombreMarca"]?.ToString() },
                        Categoria = new Categoria { Id = Convert.ToInt64(datos.Lector["IdCategoria"]), Nombre = datos.Lector["NombreCategoria"]?.ToString() },
                        Estado = Convert.ToBoolean(datos.Lector["ACTIVO"])
                    };
                }
                return null;
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

        public void nuevoProducto(Producto obj)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"
                INSERT INTO PRODUCTO (
                    NOMBRE, PRECIO, 
                    DESCRIPCION_CORTA, DESCRIPCION_EXTENDIDA,
                    IMAGEN_URL,
                    STOCK, STOCK_MINIMO,
                    ID_MARCA, ID_CATEGORIA,
                    ACTIVO
                ) VALUES (
                    @Nombre, @Precio,
                    @DescCorta, @DescExtendida,
                    @ImagenUrl,
                    @Stock, @StockMinimo,
                    @IdMarca, @IdCategoria,
                    @Activo
                )");

                datos.SetearParametro("@Nombre", obj.Nombre);
                datos.SetearParametro("@Precio", obj.Precio);
                datos.SetearParametro("@DescCorta", obj.DescripcionCorta);
                datos.SetearParametro("@DescExtendida", obj.DescripcionExtendida);
                datos.SetearParametro("@ImagenUrl",string.IsNullOrWhiteSpace(obj.ImagenUrl) ? (object)DBNull.Value : obj.ImagenUrl);
                datos.SetearParametro("@Stock", obj.Stock);
                datos.SetearParametro("@StockMinimo", obj.StockMinimo);
                datos.SetearParametro("@IdMarca", obj.Marca.Id);
                datos.SetearParametro("@IdCategoria", obj.Categoria.Id);
                datos.SetearParametro("@Activo", obj.Estado);

                datos.ejecutarAccion();
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

        public int ContarProductosActivos()
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("SELECT COUNT(*) FROM PRODUCTO WHERE ACTIVO = 1");
                object resultado = datos.ejecutarEscalar();
                return Convert.ToInt32(resultado);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al contar productos activos: " + ex.Message);
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }
    }
}
