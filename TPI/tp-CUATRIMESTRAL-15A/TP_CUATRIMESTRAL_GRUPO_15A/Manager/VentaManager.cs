using Clases;
using System;
using System.Collections.Generic;
using System.Linq;

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
                ORDER BY v.ID ASC");

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

        // ===================== para las ventas que figuran en el dashboard =====================


        public int ContarVentas()
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("SELECT COUNT(*) FROM VENTA");
                return (int)datos.ejecutarEscalar();
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }

        public List<dynamic> ListarVentasDashboard()
        {
            List<dynamic> lista = new List<dynamic>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(@"
            SELECT 
                v.ID AS IdVenta,
                v.FECHAVENTA,
                u.NICKNAME + ' (' + u.EMAIL + ')' AS Cliente,
                p.NOMBRE AS Producto,
                dv.PRECIO_UNITARIO AS Precio,
                dv.CANTIDAD,
                (dv.PRECIO_UNITARIO * dv.CANTIDAD) AS Total
            FROM VENTA v
            INNER JOIN DETALLE_VENTA dv ON v.ID = dv.ID_VENTA
            INNER JOIN PRODUCTO p ON dv.ID_PRODUCTO = p.ID
            INNER JOIN CLIENTE c ON v.ID_CLIENTE = c.ID
            INNER JOIN USUARIO u ON c.ID_USUARIO = u.ID
            ORDER BY v.FECHAVENTA DESC");

                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    lista.Add(new
                    {
                        IdVenta = Convert.ToInt64(datos.Lector["IdVenta"]),
                        FechaVenta = Convert.ToDateTime(datos.Lector["FECHAVENTA"]),
                        Cliente = datos.Lector["Cliente"].ToString(),
                        Producto = datos.Lector["Producto"].ToString(),
                        Precio = Convert.ToDecimal(datos.Lector["Precio"]),
                        Cantidad = Convert.ToInt32(datos.Lector["CANTIDAD"]),
                        Total = Convert.ToDecimal(datos.Lector["Total"])
                    });
                }
            }
            finally
            {
                datos.CerrarConeccion();
            }

            return lista;
        }

        public List<dynamic> ListarVentasDashboard(DateTime? fechaInicio = null, DateTime? fechaFin = null, string cliente = null, long? idVenta = null)
        {
            List<dynamic> lista = new List<dynamic>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string query = @"
        SELECT 
            v.ID,
            v.FECHAVENTA,
            CONCAT(c.NOMBRE, ' ', c.APELLIDO) AS Cliente,
            p.NOMBRE AS Producto,
            dv.PRECIO_UNITARIO AS Precio,
            dv.CANTIDAD,
            (dv.PRECIO_UNITARIO * dv.CANTIDAD) AS Total
        FROM DETALLE_VENTA dv
        INNER JOIN VENTA v ON v.ID = dv.ID_VENTA
        INNER JOIN CLIENTE c ON c.ID = v.ID_CLIENTE
        INNER JOIN PRODUCTO p ON p.ID = dv.ID_PRODUCTO
        WHERE 1=1";  // Truco para ir agregando filtros dinamicos

                if (fechaInicio.HasValue)
                    query += " AND v.FECHAVENTA >= @fechaInicio";

                if (fechaFin.HasValue)
                    query += " AND v.FECHAVENTA <= @fechaFin";

                if (!string.IsNullOrEmpty(cliente))
                    query += " AND (c.NOMBRE LIKE @cliente OR c.APELLIDO LIKE @cliente OR c.ID IN (SELECT ID_USUARIO FROM USUARIO WHERE EMAIL LIKE @cliente))";

                if (idVenta.HasValue)
                    query += " AND v.ID = @idVenta";

                query += " ORDER BY v.FECHAVENTA DESC";

                datos.SetearConsulta(query);

                if (fechaInicio.HasValue) datos.SetearParametro("@fechaInicio", fechaInicio.Value);
                if (fechaFin.HasValue) datos.SetearParametro("@fechaFin", fechaFin.Value);
                if (!string.IsNullOrEmpty(cliente)) datos.SetearParametro("@cliente", "%" + cliente + "%");
                if (idVenta.HasValue) datos.SetearParametro("@idVenta", idVenta.Value);

                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    lista.Add(new
                    {
                        FechaVenta = Convert.ToDateTime(datos.Lector["FECHAVENTA"]),
                        Cliente = datos.Lector["Cliente"].ToString(),
                        Producto = datos.Lector["Producto"].ToString(),
                        Precio = Convert.ToDecimal(datos.Lector["Precio"]),
                        Cantidad = Convert.ToInt32(datos.Lector["CANTIDAD"]),
                        Total = Convert.ToDecimal(datos.Lector["Total"]),
                        IdVenta = Convert.ToInt64(datos.Lector["ID"])
                    });
                }
            }
            finally
            {
                datos.CerrarConeccion();
            }

            return lista;
        }


    }
}





