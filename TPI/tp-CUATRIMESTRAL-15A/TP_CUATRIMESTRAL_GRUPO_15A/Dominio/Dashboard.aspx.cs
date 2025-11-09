using System;
using System.Web.UI;
using Manager;

namespace Dominio
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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

