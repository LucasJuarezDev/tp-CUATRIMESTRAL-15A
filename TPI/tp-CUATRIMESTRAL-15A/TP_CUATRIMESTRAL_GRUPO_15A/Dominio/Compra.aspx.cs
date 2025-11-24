using Clases;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dominio
{
    public partial class Compra : System.Web.UI.Page
    {
        private decimal totalCarrito;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarTiposPago();
                CargarResumenCarrito();
            }
        }

        private void CargarTiposPago()
        {
            TipoPagoManager manager = new TipoPagoManager();

            ddlPago.DataSource = manager.Listar();
            ddlPago.DataTextField = "Nombre";
            ddlPago.DataValueField = "Id";
            ddlPago.DataBind();
        }

        private void CargarResumenCarrito()
        {
            var carrito = Session["Carrito"] as List<ProductoCarrito>;
            if (carrito == null) return;

            totalCarrito = carrito.Sum(x => x.Precio * x.Cantidad);

            var resumen = carrito.Select(x => new
            {
                x.Nombre,
                x.Cantidad,
                Subtotal = x.Precio * x.Cantidad
            });

            repResumen.DataSource = resumen;
            repResumen.DataBind();

            ActualizarTotal();
        }

        private void ActualizarTotal()
        {
            decimal envio = decimal.Parse(ddlEnvio.SelectedValue);
            decimal final = totalCarrito + envio;
            lblTotal.Text = final.ToString("C");
        }

        protected void ddlPago_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlPago.SelectedItem.Text.ToUpper().Contains("TARJETA"))
                panelTarjeta.Visible = true;
            else
                panelTarjeta.Visible = false;
        }

        protected void ddlEnvio_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarTotal();
        }

        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            // Registrar venta 
            Session["Carrito"] = null;
            Response.Redirect("CompraExitosa.aspx");
        }
    }
}


