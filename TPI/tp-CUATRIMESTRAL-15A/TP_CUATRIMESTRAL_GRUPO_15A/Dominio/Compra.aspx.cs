using Clases;
using Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;

namespace Dominio
{
    public partial class Compra : Page
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
            ddlPago.Items.Add(new System.Web.UI.WebControls.ListItem("Efectivo", "1"));
            ddlPago.Items.Add(new System.Web.UI.WebControls.ListItem("Transferencia", "2"));
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
            lblTotalFinal.Text = totalFinal.ToString("N0");
        }

        protected async void btnConfirmar_Click(object sender, EventArgs e)
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
            string rutaComprobante = null;

            if (idPago == 2 && fuComprobante.HasFile)
            {
                try
                {
                    string carpeta = Server.MapPath("~/comprobante/");
                    Directory.CreateDirectory(carpeta);

                    string ext = Path.GetExtension(fuComprobante.FileName).ToLower();
                    if (ext == ".jpg" || ext == ".jpeg" || ext == ".png")
                    {
                        string nombreArchivo = $"comp_{DateTime.Now:yyyyMMddHHmmssfff}{ext}";
                        string pathFisico = Path.Combine(carpeta, nombreArchivo);
                        fuComprobante.SaveAs(pathFisico);

                        rutaComprobante = "/comprobante/" + nombreArchivo;
                    }
                }
                catch { }
            }

            VentaManager manager = new VentaManager();
            long idVenta = manager.RegistrarVenta(carrito, idPago, cliente.Id, costoEnvio);

            if (idPago == 2)
            {
                manager.GuardarComprobante(idVenta, rutaComprobante);
                manager.CambiarEstadoPago(idVenta, 4);
            }

            try
            {
                decimal total = carrito.Sum(x => x.Precio * x.Cantidad) + costoEnvio;
                EmailManager emailManager = new EmailManager();

                await emailManager.EnviarMailVenta(
                    cliente.Usuario.Email,
                    cliente.Nombre,
                    idVenta,
                    total
                );
            }
            catch { }

            Session["Carrito"] = null;

            Response.Redirect("CompraExitosa.aspx?id=" + idVenta, false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
