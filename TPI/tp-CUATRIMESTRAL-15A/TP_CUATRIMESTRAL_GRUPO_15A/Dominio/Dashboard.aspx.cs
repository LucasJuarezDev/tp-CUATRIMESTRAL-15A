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
                    Response.Redirect("Productos.aspx");
                else
                    Response.Redirect("Catalogo.aspx");
            }

            if (!IsPostBack)
            {
                CargarEstadisticas();
                CargarTabla();
            }
        }

        private void CargarEstadisticas()
        {
            try
            {
                ProductoManager productoManager = new ProductoManager();
                UsuarioManager usuarioManager = new UsuarioManager();
                VentaManager ventaManager = new VentaManager();

                lblCantidadProductos.Text = productoManager.ContarProductosActivos().ToString();
                lblCantidadClientes.Text = usuarioManager.ContarClientes().ToString();
                lblCantidadVentas.Text = ventaManager.ContarVentas().ToString();
            }
            catch
            {
                lblCantidadProductos.Text = "Error";
                lblCantidadClientes.Text = "Error";
                lblCantidadVentas.Text = "Error";
            }
        }

        private void CargarTabla()
        {
            VentaManager ventaManager = new VentaManager();
            var lista = ventaManager.ListarVentasDashboard();
            gvVentas.DataSource = lista;
            gvVentas.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            VentaManager ventaManager = new VentaManager();

            DateTime? fechaInicio = string.IsNullOrEmpty(txtFechaInicio.Text)
                ? (DateTime?)null
                : Convert.ToDateTime(txtFechaInicio.Text);

            DateTime? fechaFin = string.IsNullOrEmpty(txtFechaFin.Text)
                ? (DateTime?)null
                : Convert.ToDateTime(txtFechaFin.Text);

            string cliente = txtCliente.Text.Trim();
            long? idVenta = long.TryParse(txtIdVenta.Text, out long id) ? id : (long?)null;

            var lista = ventaManager.ListarVentasDashboard(fechaInicio, fechaFin, cliente, idVenta);

            gvVentas.DataSource = lista;
            gvVentas.DataBind();

            lblCantidadVentas.Text = lista.Count.ToString();
        }
    }
}


