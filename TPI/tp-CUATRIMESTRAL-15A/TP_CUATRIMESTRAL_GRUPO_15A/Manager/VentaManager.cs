using System;
using System.Collections.Generic;
using Manager;
using Clases;

namespace Manager
{
    public class VentaManager
    {
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
    }
}





