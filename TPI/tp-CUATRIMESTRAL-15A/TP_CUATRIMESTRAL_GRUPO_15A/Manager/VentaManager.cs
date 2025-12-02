using System;
using System.Collections.Generic;
using Clases;

namespace Manager
{
    public class VentaManager
    {
        private readonly EstadoPagoManager estadoPagoManager = new EstadoPagoManager();
        private readonly EstadoPreparacionManager estadoPreparacionManager = new EstadoPreparacionManager();
        private readonly EstadoEnvioManager estadoEnvioManager = new EstadoEnvioManager();

        // ===================== REGISTRAR VENTA =====================
        public long RegistrarVenta(List<ProductoCarrito> carrito, byte idTipoPago, long idCliente, decimal costoEnvio = 0)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(@"
                    INSERT INTO VENTA (FECHAVENTA, MONTOTOTAL, ID_TIPO_PAGO, ID_CLIENTE, NUM_FACTURA)
                    VALUES (@fecha, @monto, @tipoPago, @cliente, @factura);
                    SELECT SCOPE_IDENTITY();
                ");

                decimal total = 0;
                foreach (var item in carrito)
                    total += item.Precio * item.Cantidad;

                decimal totalFinal = total + costoEnvio;

                datos.SetearParametro("@fecha", DateTime.Now);
                datos.SetearParametro("@monto", totalFinal);
                datos.SetearParametro("@tipoPago", idTipoPago);
                datos.SetearParametro("@cliente", idCliente);

                string nroFactura = "FAC-" + DateTime.Now.ToString("yyyyMMddHHmmss");
                datos.SetearParametro("@factura", nroFactura);

                long idVenta = Convert.ToInt64(datos.ejecutarEscalar());

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

                return idVenta;
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }

        // ===================== LISTAR PARA ADMIN =====================

        public List<Venta> ListarTodasConDetalleYEstados()
        {
            List<Venta> lista = new List<Venta>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(@"
                SELECT 
                    v.ID,
                    v.FECHAVENTA,
                    v.MONTOTOTAL,
                    v.ID_TIPO_PAGO,
                    tp.NOMBRE AS NombreTipoPago,
                    c.ID AS IdCliente,
                    c.NOMBRE AS ClienteNombre,
                    c.APELLIDO AS ClienteApellido,
                    u.ID AS IdUsuario,
                    u.EMAIL AS ClienteEmail,
                    v.ID_ESTADO_PAGO,
                    v.ID_ESTADO_PREPARACION,
                    v.ID_ESTADO_ENVIO
                FROM VENTA v
                INNER JOIN TIPO_PAGO tp ON tp.ID = v.ID_TIPO_PAGO
                INNER JOIN CLIENTE c ON c.ID = v.ID_CLIENTE
                INNER JOIN USUARIO u ON u.ID = c.ID_USUARIO
                ORDER BY v.FECHAVENTA DESC");

                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Venta venta = new Venta
                    {
                        Id = Convert.ToInt64(datos.Lector["ID"]),
                        FechaVenta = Convert.ToDateTime(datos.Lector["FECHAVENTA"]),
                        MontoTotal = Convert.ToDecimal(datos.Lector["MONTOTOTAL"]),
                        TipoPago = new TipoPago
                        {
                            Id = Convert.ToByte(datos.Lector["ID_TIPO_PAGO"]),
                            Nombre = datos.Lector["NombreTipoPago"].ToString()
                        },
                        Cliente = new Cliente
                        {
                            Id = Convert.ToInt64(datos.Lector["IdCliente"]),
                            Nombre = datos.Lector["ClienteNombre"].ToString(),
                            Apellido = datos.Lector["ClienteApellido"].ToString(),
                            Usuario = new Usuario
                            {
                                Id = Convert.ToInt64(datos.Lector["IdUsuario"]),
                                Email = datos.Lector["ClienteEmail"].ToString()
                            }
                        }
                    };

                    // Estados reales desde los managers
                    venta.EstadoPago = estadoPagoManager.ObtenerPorId(Convert.ToByte(datos.Lector["ID_ESTADO_PAGO"]));
                    venta.EstadoPreparacion = estadoPreparacionManager.ObtenerPorId(Convert.ToByte(datos.Lector["ID_ESTADO_PREPARACION"]));
                    venta.EstadoEnvio = estadoEnvioManager.ObtenerPorId(Convert.ToByte(datos.Lector["ID_ESTADO_ENVIO"]));

                    lista.Add(venta);
                }
            }
            finally
            {
                datos.CerrarConeccion();
            }

            return lista;
        }

        // ===================== PARA CLIENTE =====================

        public List<Venta> ObtenerVentasPorCliente(long idCliente)
        {
            List<Venta> lista = new List<Venta>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(@"
                SELECT 
                    v.ID,
                    v.FECHAVENTA,
                    v.MONTOTOTAL,
                    v.ID_TIPO_PAGO,
                    tp.NOMBRE AS NombreTipoPago,
                    v.ID_ESTADO_PAGO,
                    v.ID_ESTADO_PREPARACION,
                    v.ID_ESTADO_ENVIO
                FROM VENTA v
                INNER JOIN TIPO_PAGO tp ON tp.ID = v.ID_TIPO_PAGO
                WHERE v.ID_CLIENTE = @idCliente
                ORDER BY v.FECHAVENTA DESC");

                datos.SetearParametro("@idCliente", idCliente);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Venta venta = new Venta
                    {
                        Id = Convert.ToInt64(datos.Lector["ID"]),
                        FechaVenta = Convert.ToDateTime(datos.Lector["FECHAVENTA"]),
                        MontoTotal = Convert.ToDecimal(datos.Lector["MONTOTOTAL"]),
                        TipoPago = new TipoPago
                        {
                            Id = Convert.ToByte(datos.Lector["ID_TIPO_PAGO"]),
                            Nombre = datos.Lector["NombreTipoPago"].ToString()
                        }
                    };

                    venta.EstadoPago = estadoPagoManager.ObtenerPorId(Convert.ToByte(datos.Lector["ID_ESTADO_PAGO"]));
                    venta.EstadoPreparacion = estadoPreparacionManager.ObtenerPorId(Convert.ToByte(datos.Lector["ID_ESTADO_PREPARACION"]));
                    venta.EstadoEnvio = estadoEnvioManager.ObtenerPorId(Convert.ToByte(datos.Lector["ID_ESTADO_ENVIO"]));

                    lista.Add(venta);
                }
            }
            finally
            {
                datos.CerrarConeccion();
            }

            return lista;
        }

        // ===================== CAMBIAR ESTADOS =====================

        public void CambiarEstadoPago(long idVenta, int nuevo)
        {
            ActualizarEstado("ID_ESTADO_PAGO", nuevo, idVenta);
        }

        public void CambiarEstadoPreparacion(long idVenta, int nuevo)
        {
            ActualizarEstado("ID_ESTADO_PREPARACION", nuevo, idVenta);
        }

        public void CambiarEstadoEnvio(long idVenta, int nuevo)
        {
            ActualizarEstado("ID_ESTADO_ENVIO", nuevo, idVenta);
        }

        private void ActualizarEstado(string columna, int nuevoValor, long idVenta)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta($"UPDATE VENTA SET {columna} = @estado WHERE ID = @id");
                datos.SetearParametro("@estado", nuevoValor);
                datos.SetearParametro("@id", idVenta);
                datos.ejecutarAccion();
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }
    }
}





