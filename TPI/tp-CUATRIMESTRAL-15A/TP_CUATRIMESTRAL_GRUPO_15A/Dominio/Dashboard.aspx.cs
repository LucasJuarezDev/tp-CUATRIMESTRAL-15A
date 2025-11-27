using Clases;
using Manager;
using System;
using System.Web.UI;

namespace Dominio
{
    public partial class Dashboard : AuthenticationPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var usuarioLogueado = Session["usuario"] as UsuarioLogueado;
            if (usuarioLogueado?.Rol?.Id != 1)
            {
                if (usuarioLogueado?.Rol?.Id == 2)
                {
                    Response.Redirect("Productos.aspx");
                }
                else
                {
                    Response.Redirect("Catalogo.aspx");
                }
            }
            if (!IsPostBack)
            {
                CargarEstadisticas();
            }
        }

        private void CargarEstadisticas()
        {
            try
            {
                // Productos
                ProductoManager productoManager = new ProductoManager();
                int cantidadProductos = productoManager.ContarProductosActivos();
                lblCantidadProductos.Text = cantidadProductos.ToString();

                // Clientes
                UsuarioManager usuarioManager = new UsuarioManager();
                int cantidadClientes = usuarioManager.ContarClientes();
                lblCantidadClientes.Text = cantidadClientes.ToString();
            }
            catch (Exception)
            {
                lblCantidadProductos.Text = "Error";
                lblCantidadClientes.Text = "Error";
            }
        }
    }
}

