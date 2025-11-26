using Clases;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace Dominio
{
    public partial class Compra : System.Web.UI.Page
    {
        private decimal totalBase
        {
            get { return ViewState["totalBase"] != null ? (decimal)ViewState["totalBase"] : 0; }
            set { ViewState["totalBase"] = value; }
        }

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

            var resumen = carrito.Select(x => new
            {
                x.Nombre,
                x.Cantidad,
                Subtotal = x.Cantidad * x.Precio
            });

            repResumen.DataSource = resumen;
            repResumen.DataBind();

            totalBase = carrito.Sum(x => x.Cantidad * x.Precio);
            ActualizarTotal();
        }

        private void ActualizarTotal()
        {
            decimal envio = decimal.Parse(ddlEnvio.SelectedValue);
            decimal final = totalBase + envio;

            lblTotal.Text = final.ToString("N2");
        }

        protected void ddlPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            string metodo = ddlPago.SelectedItem.Text.ToUpper();

            pnlTarjeta.Visible = (metodo.Contains("DEBITO") || metodo.Contains("CREDITO"));
        }

        protected void ddlEnvio_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarTotal();
        }

        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            var carrito = Session["Carrito"] as List<ProductoCarrito>;
            if (carrito == null || carrito.Count == 0)
                return;

            byte idPago = byte.Parse(ddlPago.SelectedValue);

            VentaManager manager = new VentaManager();
            manager.RegistrarVenta(carrito, idPago);

            Session["Carrito"] = null;
            Response.Redirect("CompraExitosa.aspx");
        }
    }
}


