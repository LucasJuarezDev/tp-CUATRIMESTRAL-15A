using System;
using System.Collections.Generic;
using Manager;
using Clases;

namespace Manager
{
    public class VentaManager
    {
        // Firma con costoEnvio opcional
        public long RegistrarVenta(List<ProductoCarrito> carrito, byte idTipoPago, long idCliente, decimal costoEnvio = 0m)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // 1) Calcular total (productos) + envio
                decimal total = 0m;
                foreach (var item in carrito)
                    total += item.Precio * item.Cantidad;

                total += costoEnvio; // <-- aca se suma el envio

                // 2) Insertar VENTA con total ya sumado
                datos.SetearConsulta(@"
                    INSERT INTO VENTA (FECHAVENTA, MONTOTOTAL, ID_TIPO_PAGO, ID_CLIENTE, NUM_FACTURA)
                    VALUES (@fecha, @monto, @tipoPago, @cliente, @factura);
                    SELECT SCOPE_IDENTITY();
                ");

                datos.SetearParametro("@fecha", DateTime.Now);
                datos.SetearParametro("@monto", total);           // <-- total ya incluye envio
                datos.SetearParametro("@tipoPago", idTipoPago);
                datos.SetearParametro("@cliente", idCliente);

                string nroFactura = "FAC-" + DateTime.Now.ToString("yyyyMMddHHmmss");
                datos.SetearParametro("@factura", nroFactura);

                long idVenta = Convert.ToInt64(datos.ejecutarEscalar());
                datos.CerrarConeccion();

                // 3) Insertar DETALLES
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

                // 4) Llamada al SP para actualizar stock 
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




