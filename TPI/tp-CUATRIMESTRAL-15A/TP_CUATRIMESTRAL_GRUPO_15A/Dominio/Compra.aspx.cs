using Clases;
using Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dominio
{
    public partial class Compra : System.Web.UI.Page
    {
        private decimal costoEnvio = 0;
        private string whatsappAdmin = "";
        private ConfigManager configManager = new ConfigManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarConfiguracion();
                CargarTiposPago();
                CargarResumenCarrito();
            }
        }

        private void CargarConfiguracion()
        {
            costoEnvio = configManager.ObtenerDecimal("COSTO_ENVIO", 2500);
            whatsappAdmin = configManager.ObtenerString("WHATSAPP_ADMIN", "5491167152188");

            lblEnvio.Text = costoEnvio.ToString("N0");
            lblEnvioResumen.Text = costoEnvio.ToString("N0");
        }

        private void CargarTiposPago()
        {
            ddlPago.Items.Clear();
            ddlPago.Items.Add(new ListItem("Efectivo", "1"));
            ddlPago.Items.Add(new ListItem("Transferencia", "2"));
            ddlPago.Attributes.Add("onchange", "mostrarComprobante()");
        }

        private void CargarResumenCarrito()
        {
            var carrito = Session["Carrito"] as List<ProductoCarrito>;
            if (carrito == null || carrito.Count == 0)
            {
                Response.Redirect("Carrito.aspx");
                return;
            }

            var resumen = carrito.Select(x => new
            {
                x.Nombre,
                x.Cantidad,
                Subtotal = x.Cantidad * x.Precio
            }).ToList();

            repResumen.DataSource = resumen;
            repResumen.DataBind();

            decimal subtotal = resumen.Sum(x => x.Subtotal);
            decimal totalFinal = subtotal + costoEnvio;

            lblSubtotal.Text = subtotal.ToString("N0");
            lblEnvio.Text = costoEnvio.ToString("N0");
            lblEnvioResumen.Text = costoEnvio.ToString("N0");

            // ESTOS DOS SON LOS IMPORTANTES
            lblTotalFinal.Text = totalFinal.ToString("N0");
        }

        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            var carrito = Session["Carrito"] as List<ProductoCarrito>;
            if (carrito == null || carrito.Count == 0) return;

            var cliente = Session["cliente"] as Cliente;
            if (cliente == null)
            {
                Response.Redirect("LoginCliente.aspx");
                return;
            }

            byte idPago = byte.Parse(ddlPago.SelectedValue);

            // REGISTRAR VENTA
            VentaManager manager = new VentaManager();
            long idVenta = manager.RegistrarVenta(carrito, idPago, cliente.Id, costoEnvio);

            if (idPago == 2)
            {
                whatsappAdmin = configManager.ObtenerString("WHATSAPP_ADMIN", "5491167152188");

                string mensaje = $"HOLA! ACABO DE REALIZAR UNA COMPRA  " +
                                 $"Venta Nº: *{idVenta}  " +
                                 $"Cliente: *{cliente.Nombre} {cliente.Apellido}  " +
                                 $"Total: *${(carrito.Sum(x => x.Cantidad * x.Precio) + costoEnvio):N0}  " +
                                 $"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}   " +
                                 $"TE TRANSFERI EL TOTAL, ESTOY ATENTO A TU CONFIRMACION DE COMPRA.";

                string mensajeCodificado = HttpUtility.UrlEncode(mensaje);

                string urlWhatsApp = $"https://api.whatsapp.com/send?phone={whatsappAdmin}&text={mensajeCodificado}";
                Response.Redirect(urlWhatsApp);
            }

            Session["Carrito"] = null;
            Response.Redirect("CompraExitosa.aspx?id=" + idVenta);
        }
    }
}