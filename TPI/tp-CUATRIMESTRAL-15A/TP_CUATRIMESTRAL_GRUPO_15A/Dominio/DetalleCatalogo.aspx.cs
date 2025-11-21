using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clases;
using Manager;

namespace Dominio
{
    public partial class DetalleCatalogo : System.Web.UI.Page
    {
        private readonly ProductoManager manager = new ProductoManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarProducto();
            }
        }

        private void CargarProducto()
        {
            try
            {
                if (!long.TryParse(Request.QueryString["id"], out long id))
                {
                    Response.Redirect("Catalogo.aspx");
                    return;
                }

                Producto producto = manager.BuscarPorId(id);

                if (producto == null || !producto.Estado)
                {
                    Response.Redirect("Catalogo.aspx");
                    return;
                }

                // ASIGNAR AL FORMVIEW
                fvProducto.DataSource = new List<Producto> { producto };
                fvProducto.DataBind();
            }
            catch
            {
                Response.Redirect("Catalogo.aspx");
            }
        }

        protected void btnAgregarCarrito_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            long id = Convert.ToInt64(btn.CommandArgument);
            Producto producto = manager.BuscarPorId(id);

            if (producto == null || producto.Stock <= 0)
                return;

            if (Session["Carrito"] == null)
                Session["Carrito"] = new List<ProductoCarrito>();

            var carrito = (List<ProductoCarrito>)Session["Carrito"];

            var existente = carrito.FirstOrDefault(x => x.IdProducto == id);

            if (existente == null)
            {
                carrito.Add(new ProductoCarrito
                {
                    IdProducto = producto.Id,
                    Nombre = producto.Nombre,
                    Precio = producto.Precio,
                    Cantidad = 1
                });
            }
            else
            {
                existente.Cantidad++;
            }

            Session["Carrito"] = carrito;

            MostrarExito($"'{producto.Nombre}' agregado al carrito");
        }



        private void MostrarExito(string msg)
        {
            string script = $@"Swal.fire({{icon:'success', title:'¡Listo!', text:'{msg}', timer:1500, showConfirmButton:false}})";
            ClientScript.RegisterStartupScript(this.GetType(), "exito", script, true);
        }
    }
}