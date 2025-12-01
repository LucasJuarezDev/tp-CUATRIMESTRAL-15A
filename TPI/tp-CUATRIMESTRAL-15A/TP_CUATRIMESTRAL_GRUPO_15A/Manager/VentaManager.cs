using System;
using System.Collections.Generic;
using Manager;
using Clases;

namespace Manager
{
    public class VentaManager
    {
        private long SafeLong(object valor, string campo) => valor == DBNull.Value ? 0 : Convert.ToInt64(valor);
        private int SafeInt(object valor, string campo) => valor == DBNull.Value ? 0 : Convert.ToInt32(valor);
        private byte SafeByte(object valor, string campo) => valor == DBNull.Value ? (byte)0 : Convert.ToByte(valor);
        private decimal SafeDecimal(object valor, string campo) => valor == DBNull.Value ? 0m : Convert.ToDecimal(valor);
        private DateTime SafeDateTime(object valor, string campo) => valor == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(valor);
        private string SafeString(object valor, string campo) => valor == DBNull.Value ? "" : valor.ToString();
        public List<dynamic> ListarEstadosPago() => ListarEstados("SELECT ID, NOMBRE FROM ESTADO_PAGO ORDER BY ID");
        public List<dynamic> ListarEstadosPreparacion() => ListarEstados("SELECT ID, NOMBRE FROM ESTADO_PREPARACION ORDER BY ID");
        public List<dynamic> ListarEstadosEnvio() => ListarEstados("SELECT ID, NOMBRE FROM ESTADO_ENVIO ORDER BY ID");
        public long RegistrarVenta(List<ProductoCarrito> carrito, byte idTipoPago, long idCliente)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // 1) Insertar VENTA
                datos.SetearConsulta(@"
                    INSERT INTO VENTA (FECHAVENTA, MONTOTOTAL, ID_TIPO_PAGO, ID_CLIENTE, NUM_FACTURA)
                    VALUES (@fecha, @monto, @tipoPago, @cliente, @factura);
                    SELECT SCOPE_IDENTITY();
                ");

                decimal total = 0;
                foreach (var item in carrito)
                    total += item.Precio * item.Cantidad;

                datos.SetearParametro("@fecha", DateTime.Now);
                datos.SetearParametro("@monto", total);
                datos.SetearParametro("@tipoPago", idTipoPago);
                datos.SetearParametro("@cliente", idCliente);

                string nroFactura = "FAC-" + DateTime.Now.ToString("yyyyMMddHHmmss");
                datos.SetearParametro("@factura", nroFactura);

                long idVenta = Convert.ToInt64(datos.ejecutarEscalar());

                // 2) Insertar DETALLES
                foreach (var item in carrito)
                {
                    AccesoDatos det = new AccesoDatos();
                    det.SetearConsulta(@"
                        INSERT INTO DETALLE_VENTA (ID_VENTA, ID_PRODUCTO, CANTIDAD, PRECIO_UNITARIO)
                        VALUES (@venta, @prod, @cant, @precio)
                    ");

                    det.SetearParametro("@venta", idVenta);
                    det.SetearParametro("@prod", item.IdProducto);
                    det.SetearParametro("@cant", item.Cantidad);
                    det.SetearParametro("@precio", item.Precio);

                    det.ejecutarAccion();
                    det.CerrarConeccion();
                }

                // 3) LLAMADA AL SP para descontar stock 
                AccesoDatos stock = new AccesoDatos();
                stock.SetearConsulta("EXEC SP_ActualizarStockPorVenta @IdVenta");
                stock.SetearParametro("@IdVenta", idVenta);
                stock.ejecutarAccion();
                stock.CerrarConeccion();

                return idVenta;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar la venta: " + ex.Message);
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }

        private List<dynamic> ListarEstados(string consulta)
        {
            var lista = new List<dynamic>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(consulta);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    // USAMOS Convert.ToByte → NUNCA FALLA con TINYINT
                    byte id = Convert.ToByte(datos.Lector["ID"]);
                    string nombre = datos.Lector["NOMBRE"]?.ToString() ?? "Sin nombre";

                    lista.Add(new { Id = id, Nombre = nombre });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cargar estados: " + ex.Message, ex);
            }
            finally
            {
                datos.CerrarConeccion();
            }
            return lista;
        }
        public List<Venta> ListarTodasConDetalleYEstados()
        {
            var lista = new List<Venta>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(@"
            SELECT 
                v.ID,
                v.FECHAVENTA,
                v.MONTOTOTAL,
                v.NUM_FACTURA,
                v.ID_TIPO_PAGO,
                tp.NOMBRE AS NombreTipoPago,

                c.ID AS IdCliente,
                c.NOMBRE AS ClienteNombre,
                c.APELLIDO AS ClienteApellido,
                
                u.ID AS IdUsuario,
                u.EMAIL AS ClienteEmail,

                ep.ID AS IdEstadoPago,
                ep.NOMBRE AS NombreEstadoPago,

                eprep.ID AS IdEstadoPreparacion,
                eprep.NOMBRE AS NombreEstadoPreparacion,

                eenv.ID AS IdEstadoEnvio,
                eenv.NOMBRE AS NombreEstadoEnvio

            FROM VENTA v
            INNER JOIN TIPO_PAGO tp ON v.ID_TIPO_PAGO = tp.ID
            INNER JOIN CLIENTE c ON v.ID_CLIENTE = c.ID
            INNER JOIN USUARIO u ON c.ID_USUARIO = u.ID
            LEFT JOIN ESTADO_PAGO ep ON v.ID_ESTADO_PAGO = ep.ID
            LEFT JOIN ESTADO_PREPARACION eprep ON v.ID_ESTADO_PREPARACION = eprep.ID
            LEFT JOIN ESTADO_ENVIO eenv ON v.ID_ESTADO_ENVIO = eenv.ID
            ORDER BY v.FECHAVENTA DESC");

                datos.EjecutarLectura();

                int fila = 0;
                while (datos.Lector.Read())
                {
                    fila++;
                    try
                    {
                        Venta venta = new Venta
                        {
                            Id = SafeLong(datos.Lector["ID"], "ID Venta"),
                            FechaVenta = SafeDateTime(datos.Lector["FECHAVENTA"], "FechaVenta"),
                            MontoTotal = SafeDecimal(datos.Lector["MONTOTOTAL"], "MontoTotal"),
                            NumeroFactura = datos.Lector["NUM_FACTURA"]?.ToString() ?? "",

                            TipoPago = new TipoPago
                            {
                                Id = SafeByte(datos.Lector["ID_TIPO_PAGO"], "ID_TIPO_PAGO"),
                                Nombre = SafeString(datos.Lector["NombreTipoPago"], "NombreTipoPago")
                            },

                            Cliente = new Cliente
                            {
                                Id = SafeLong(datos.Lector["IdCliente"], "IdCliente"),
                                Nombre = SafeString(datos.Lector["ClienteNombre"], "ClienteNombre"),
                                Apellido = SafeString(datos.Lector["ClienteApellido"], "ClienteApellido"),
                                Usuario = new Usuario
                                {
                                    Id = SafeLong(datos.Lector["IdUsuario"], "IdUsuario"),
                                    Email = SafeString(datos.Lector["ClienteEmail"], "ClienteEmail") ?? "sin@email.com"
                                }
                            },

                            EstadoPago = new EstadoPago
                            {
                                Id = SafeInt(datos.Lector["IdEstadoPago"], "IdEstadoPago"),
                                Nombre = SafeString(datos.Lector["NombreEstadoPago"], "NombreEstadoPago")
                            },

                            EstadoPreparacion = new EstadoPreparacion
                            {
                                Id = SafeInt(datos.Lector["IdEstadoPreparacion"], "IdEstadoPreparacion"),
                                Nombre = SafeString(datos.Lector["NombreEstadoPreparacion"], "NombreEstadoPreparacion")
                            },

                            EstadoEnvio = new EstadoEnvio
                            {
                                Id = SafeInt(datos.Lector["IdEstadoEnvio"], "IdEstadoEnvio"),
                                Nombre = SafeString(datos.Lector["NombreEstadoEnvio"], "NombreEstadoEnvio")
                            }
                        };

                        lista.Add(venta);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error al procesar la fila {fila} de la venta ID={datos.Lector["ID"]}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error crítico al listar pedidos: " + ex.Message, ex);
            }
            finally
            {
                datos.CerrarConeccion();
            }

            return lista;
        }

        public void CambiarEstadoPago(long idVenta, int idNuevoEstadoPago)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("UPDATE VENTA SET ID_ESTADO_PAGO = @estado WHERE ID = @id");
                datos.SetearParametro("@estado", idNuevoEstadoPago);
                datos.SetearParametro("@id", idVenta);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar estado de pago: " + ex.Message);
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }

        public void CambiarEstadoPreparacion(long idVenta, int idNuevoEstadoPreparacion)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("UPDATE VENTA SET ID_ESTADO_PREPARACION = @estado WHERE ID = @id");
                datos.SetearParametro("@estado", idNuevoEstadoPreparacion);
                datos.SetearParametro("@id", idVenta);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar estado de preparación: " + ex.Message);
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }

        public void CambiarEstadoEnvio(long idVenta, int idNuevoEstadoEnvio)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("UPDATE VENTA SET ID_ESTADO_ENVIO = @estado WHERE ID = @id");
                datos.SetearParametro("@estado", idNuevoEstadoEnvio);
                datos.SetearParametro("@id", idVenta);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar estado de envío: " + ex.Message);
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }
    }
}





