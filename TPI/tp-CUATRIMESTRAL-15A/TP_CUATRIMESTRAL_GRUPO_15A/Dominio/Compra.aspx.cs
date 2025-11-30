using Clases;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;

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
            ddlPago.Items.Clear();
            ddlPago.Items.Add(new System.Web.UI.WebControls.ListItem("Efectivo", "1"));
            ddlPago.Items.Add(new System.Web.UI.WebControls.ListItem("Transferencia", "2"));
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
            lblTotal.Text = totalBase.ToString("N2");
        }

        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            var carrito = Session["Carrito"] as List<ProductoCarrito>;
            if (carrito == null || carrito.Count == 0)
                return;

            byte idPago = byte.Parse(ddlPago.SelectedValue);

            var cliente = Session["cliente"] as Cliente;
            if (cliente == null)
            {
                Response.Redirect("LoginCliente.aspx");
                return;
            }

            long idCliente = cliente.Id;

            VentaManager manager = new VentaManager();
            manager.RegistrarVenta(carrito, idPago, idCliente);

            Session["Carrito"] = null;
            Response.Redirect("CompraExitosa.aspx");
        }
    }
}
