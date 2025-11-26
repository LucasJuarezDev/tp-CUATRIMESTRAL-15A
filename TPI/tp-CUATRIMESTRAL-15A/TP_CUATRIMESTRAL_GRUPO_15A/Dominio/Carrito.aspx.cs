using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clases;

namespace Dominio
{
    public partial class Carrito : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CargarCarrito();
        }

        private void CargarCarrito()
        {
            var carrito = Session["Carrito"] as List<ProductoCarrito>;

            if (carrito == null || carrito.Count == 0)
            {
                pnlVacio.Visible = true;
                pnlCarrito.Visible = false;
                lblTotal.Text = "$0.00";
                return;
            }

            pnlVacio.Visible = false;
            pnlCarrito.Visible = true;

            gvCarrito.DataSource = carrito;
            gvCarrito.DataBind();

            decimal total = carrito.Sum(x => x.Precio * x.Cantidad);
            lblTotal.Text = total.ToString("C");
        }

        protected void gvCarrito_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var carrito = Session["Carrito"] as List<ProductoCarrito>;
            if (carrito == null) return;

            long id = Convert.ToInt64(e.CommandArgument);

            var item = carrito.FirstOrDefault(x => x.IdProducto == id);
            if (item == null) return;

            switch (e.CommandName)
            {
                case "Sumar":
                    item.Cantidad++;
                    break;

                case "Restar":
                    item.Cantidad--;
                    if (item.Cantidad <= 0)
                        carrito.Remove(item);
                    break;

                case "Eliminar":
                    carrito.Remove(item);
                    break;
            }

            Session["Carrito"] = carrito;
            CargarCarrito();
        }

        protected void btnContinuar_Click(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("LoginCliente.aspx");
                return;
            }

            Response.Redirect("Compra.aspx");
        }


    }
}
