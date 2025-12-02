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
        public List<Producto> Listar(int fromCatalogo = 0)
        {
            List<Producto> lista = new List<Producto>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                if(fromCatalogo == 1)
                {
                    datos.SetearConsulta("SELECT * FROM vw_ProductosActivos ORDER BY NOMBRE");
                }
                else
                {
                    datos.SetearConsulta("SELECT * FROM vw_ProductosActivos ORDER BY ID DESC");
                }
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
                        ImagenPrincipal = datos.Lector["ImagenPrincipal"].ToString(), // ← Viene de la vista

                        Marca = new Marca
                        {
                            Id = Convert.ToInt64(datos.Lector["IdMarca"]),
                            Nombre = datos.Lector["NombreMarca"].ToString()
                        },
                        Categoria = new Categoria
                        {
                            Id = Convert.ToInt64(datos.Lector["IdCategoria"]),
                            Nombre = datos.Lector["NombreCategoria"].ToString()
                        },
                        Estado = true
                    };

                    lista.Add(prod);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar productos: " + ex.Message);
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
                    var prod = new Producto
                    {
                        Id = Convert.ToInt64(datos.Lector["ID"]),
                        Nombre = datos.Lector["NOMBRE"].ToString(),
                        Precio = Convert.ToDecimal(datos.Lector["PRECIO"]),
                        DescripcionCorta = datos.Lector["DESCRIPCION_CORTA"].ToString(),
                        DescripcionExtendida = datos.Lector["DESCRIPCION_EXTENDIDA"].ToString(),
                        Stock = Convert.ToInt32(datos.Lector["STOCK"]),
                        StockMinimo = Convert.ToInt32(datos.Lector["STOCK_MINIMO"]),
                        ImagenPrincipal = datos.Lector["ImagenPrincipal"].ToString(),

                        Marca = new Marca
                        {
                            Id = Convert.ToInt64(datos.Lector["IdMarca"]),
                            Nombre = datos.Lector["NombreMarca"].ToString()
                        },
                        Categoria = new Categoria
                        {
                            Id = Convert.ToInt64(datos.Lector["IdCategoria"]),
                            Nombre = datos.Lector["NombreCategoria"].ToString()
                        },
                        Estado = Convert.ToBoolean(datos.Lector["ACTIVO"])
                    };

                    // Cargar todas las imágenes (para el carrusel)
                    prod.Imagenes = ListarImagenes(prod.Id);

                    return prod;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar producto: " + ex.Message);
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }

        public long nuevoProducto(Producto obj)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"
                INSERT INTO PRODUCTO (
                    NOMBRE, PRECIO, DESCRIPCION_CORTA, DESCRIPCION_EXTENDIDA,
                    STOCK, STOCK_MINIMO, ID_MARCA, ID_CATEGORIA, ACTIVO
                ) VALUES (
                    @Nombre, @Precio, @DescCorta, @DescExtendida,
                    @Stock, @StockMinimo, @IdMarca, @IdCategoria, @Activo
                );
                SELECT SCOPE_IDENTITY();");

                datos.SetearParametro("@Nombre", obj.Nombre);
                datos.SetearParametro("@Precio", obj.Precio);
                datos.SetearParametro("@DescCorta", obj.DescripcionCorta);
                datos.SetearParametro("@DescExtendida", obj.DescripcionExtendida);
                datos.SetearParametro("@Stock", obj.Stock);
                datos.SetearParametro("@StockMinimo", obj.StockMinimo);
                datos.SetearParametro("@IdMarca", obj.Marca.Id);
                datos.SetearParametro("@IdCategoria", obj.Categoria.Id);
                datos.SetearParametro("@Activo", obj.Estado);

                return Convert.ToInt64(datos.ejecutarEscalar());
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear producto: " + ex.Message);
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }

        public void Modificar(Producto obj)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"
            UPDATE PRODUCTO SET
                NOMBRE = @Nombre,
                PRECIO = @Precio,
                DESCRIPCION_CORTA = @DescCorta,
                DESCRIPCION_EXTENDIDA = @DescExtendida,
                STOCK = @Stock,
                STOCK_MINIMO = @StockMinimo,
                ID_MARCA = @IdMarca,
                ID_CATEGORIA = @IdCategoria
            WHERE ID = @Id");

                datos.SetearParametro("@Id", obj.Id);
                datos.SetearParametro("@Nombre", obj.Nombre);
                datos.SetearParametro("@Precio", obj.Precio);
                datos.SetearParametro("@DescCorta", obj.DescripcionCorta);
                datos.SetearParametro("@DescExtendida", obj.DescripcionExtendida);
                datos.SetearParametro("@Stock", obj.Stock);
                datos.SetearParametro("@StockMinimo", obj.StockMinimo);
                datos.SetearParametro("@IdMarca", obj.Marca.Id);
                datos.SetearParametro("@IdCategoria", obj.Categoria.Id);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar producto: " + ex.Message);
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

        public List<Producto> ListarConFiltro(string filtro = "")
        {
            List<Producto> lista = new List<Producto>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = @"
            SELECT p.*, m.NOMBRE AS NombreMarca, c.NOMBRE AS NombreCategoria
            FROM PRODUCTO p
            INNER JOIN MARCA m ON p.ID_MARCA = m.ID
            INNER JOIN CATEGORIA c ON p.ID_CATEGORIA = c.ID
            WHERE p.ACTIVO = 1
              AND (@filtro = '' 
                   OR p.NOMBRE LIKE @filtro 
                   OR m.NOMBRE LIKE @filtro)";

                datos.SetearConsulta(consulta);
                datos.SetearParametro("@filtro", string.IsNullOrEmpty(filtro) ? "" : $"%{filtro}%");
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

                        Marca = new Marca
                        {
                            Id = Convert.ToInt64(datos.Lector["ID_MARCA"]),
                            Nombre = datos.Lector["NombreMarca"].ToString()
                        },
                        Categoria = new Categoria
                        {
                            Id = Convert.ToInt64(datos.Lector["ID_CATEGORIA"]),
                            Nombre = datos.Lector["NombreCategoria"].ToString()
                        }
                    };
                    lista.Add(prod);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar productos: " + ex.Message);
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }

        public void GuardarImagenes(long idProducto, List<string> rutasImagenes)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                // Primero eliminamos las antiguas (opcional, o solo insertamos nuevas)
                datos.SetearConsulta("DELETE FROM PRODUCTO_IMAGENES WHERE ID_PRODUCTO = @IdProducto");
                datos.SetearParametro("@IdProducto", idProducto);
                datos.ejecutarAccion();
                datos.CerrarConeccion();

                if (rutasImagenes == null || !rutasImagenes.Any()) return;

                for (int i = 0; i < rutasImagenes.Count; i++)
                {
                    datos = new AccesoDatos();
                    datos.SetearConsulta(@"
                INSERT INTO PRODUCTO_IMAGENES (ID_PRODUCTO, URL_IMAGEN, ES_PRINCIPAL, ORDEN)
                VALUES (@IdProducto, @Url, @Principal, @Orden)");
                    datos.SetearParametro("@IdProducto", idProducto);
                    datos.SetearParametro("@Url", rutasImagenes[i]);
                    datos.SetearParametro("@Principal", i == 0); // la primera es principal
                    datos.SetearParametro("@Orden", i);
                    datos.ejecutarAccion();
                    datos.CerrarConeccion();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar imágenes: " + ex.Message);
            }
        }

        public List<ProductoImagen> ListarImagenes(long idProducto)
        {
            List<ProductoImagen> lista = new List<ProductoImagen>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("SELECT ID, URL_IMAGEN, ES_PRINCIPAL, ORDEN FROM PRODUCTO_IMAGENES WHERE ID_PRODUCTO = @Id ORDER BY ORDEN");
                datos.SetearParametro("@Id", idProducto);
                datos.EjecutarLectura();
                while (datos.Lector.Read())
                {
                    lista.Add(new ProductoImagen
                    {
                        Id = Convert.ToInt64(datos.Lector["ID"]),
                        IdProducto = idProducto,
                        UrlImagen = datos.Lector["URL_IMAGEN"].ToString(),
                        EsPrincipal = Convert.ToBoolean(datos.Lector["ES_PRINCIPAL"]),
                        Orden = Convert.ToInt32(datos.Lector["ORDEN"])
                    });
                }
            }
            finally { datos.CerrarConeccion(); }
            return lista;
        }

        public void AgregarImagenes(long idProducto, List<string> rutasNuevas)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                // Obtener el máximo orden actual
                datos.SetearConsulta("SELECT ISNULL(MAX(ORDEN), -1) FROM PRODUCTO_IMAGENES WHERE ID_PRODUCTO = @Id");
                datos.SetearParametro("@Id", idProducto);
                int ultimoOrden = Convert.ToInt32(datos.ejecutarEscalar());
                datos.CerrarConeccion();

                int ordenActual = ultimoOrden + 1;

                foreach (string ruta in rutasNuevas)
                {
                    datos = new AccesoDatos();
                    datos.SetearConsulta(@"
                INSERT INTO PRODUCTO_IMAGENES (ID_PRODUCTO, URL_IMAGEN, ES_PRINCIPAL, ORDEN)
                VALUES (@IdProducto, @Url, @EsPrincipal, @Orden)");

                    datos.SetearParametro("@IdProducto", idProducto);
                    datos.SetearParametro("@Url", ruta);
                    datos.SetearParametro("@EsPrincipal", ordenActual == 0); // la primera del producto será principal
                    datos.SetearParametro("@Orden", ordenActual++);
                    datos.ejecutarAccion();
                    datos.CerrarConeccion();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar imágenes: " + ex.Message);
            }
        }

        public List<Producto> ListarConFiltros(string categoriaId = "", string marcaId = "", decimal? precioDesde = null, decimal? precioHasta = null)
        {
            List<Producto> lista = new List<Producto>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta = @"
            SELECT  p.ID, 
                    p.NOMBRE, 
                    p.DESCRIPCION_CORTA AS DescripcionCorta,
                    p.DESCRIPCION_EXTENDIDA,
                    p.PRECIO, 
                    p.STOCK,
                    p.ID_MARCA,
                    p.ID_CATEGORIA,
                    c.NOMBRE AS CategoriaNombre,
                    m.NOMBRE AS MarcaNombre,
                    -- Traemos la imagen principal (la que tiene ES_PRINCIPAL = 1, o la primera si no hay)
                    ISNULL((
                        SELECT TOP 1 URL_IMAGEN 
                        FROM PRODUCTO_IMAGENES 
                        WHERE ID_PRODUCTO = p.ID AND ES_PRINCIPAL = 1
                    ), (
                        SELECT TOP 1 URL_IMAGEN 
                        FROM PRODUCTO_IMAGENES 
                        WHERE ID_PRODUCTO = p.ID 
                        ORDER BY ORDEN, ID
                    )) AS ImagenPrincipal
            FROM PRODUCTO p
            INNER JOIN CATEGORIA c ON p.ID_CATEGORIA = c.ID
            INNER JOIN MARCA m ON p.ID_MARCA = m.ID
            WHERE p.ACTIVO = 1";

                List<string> condiciones = new List<string>();
                if (!string.IsNullOrEmpty(categoriaId))
                    condiciones.Add("p.ID_CATEGORIA = @catId");
                if (!string.IsNullOrEmpty(marcaId))
                    condiciones.Add("p.ID_MARCA = @marcaId");
                if (precioDesde.HasValue)
                    condiciones.Add("p.PRECIO >= @precioDesde");
                if (precioHasta.HasValue)
                    condiciones.Add("p.PRECIO <= @precioHasta");

                if (condiciones.Count > 0)
                    consulta += " AND " + string.Join(" AND ", condiciones);

                datos.SetearConsulta(consulta);

                if (!string.IsNullOrEmpty(categoriaId))
                    datos.SetearParametro("@catId", categoriaId);
                if (!string.IsNullOrEmpty(marcaId))
                    datos.SetearParametro("@marcaId", marcaId);
                if (precioDesde.HasValue)
                    datos.SetearParametro("@precioDesde", precioDesde.Value);
                if (precioHasta.HasValue)
                    datos.SetearParametro("@precioHasta", precioHasta.Value);

                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Producto p = new Producto
                    {
                        Id = (long)datos.Lector["ID"],
                        Nombre = datos.Lector["NOMBRE"].ToString(),
                        DescripcionCorta = datos.Lector["DescripcionCorta"]?.ToString() ?? "",
                        DescripcionExtendida = datos.Lector["DESCRIPCION_EXTENDIDA"]?.ToString() ?? "",
                        Precio = (decimal)datos.Lector["PRECIO"],
                        Stock = (int)datos.Lector["STOCK"],
                        ImagenPrincipal = datos.Lector["ImagenPrincipal"]?.ToString() ?? "https://via.placeholder.com/400x300/cccccc/666666?text=Sin+Imagen",
                        Categoria = new Categoria
                        {
                            Id = (long)datos.Lector["ID_CATEGORIA"],
                            Nombre = datos.Lector["CategoriaNombre"].ToString()
                        },
                        Marca = new Marca
                        {
                            Id = (long)datos.Lector["ID_MARCA"],
                            Nombre = datos.Lector["MarcaNombre"].ToString()
                        }
                    };
                    lista.Add(p);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cargar productos con filtros: " + ex.Message);
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }

        //para el carrito

        public int ObtenerStockPorId(long idProducto)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("SELECT STOCK FROM PRODUCTO WHERE ID = @id");
                datos.SetearParametro("@id", idProducto);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                    return Convert.ToInt32(datos.Lector["STOCK"]);
                else
                    return 0;
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }

    }
}
