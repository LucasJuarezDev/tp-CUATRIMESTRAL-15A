using Clases;
using Manager;
using System;
using System.Web.UI;

namespace Dominio
{
    public partial class MisCompras : AuthenticationPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CargarCompras();
        }

        protected void btnCargarDetalle_Click(object sender, EventArgs e)
        {
            long idVenta = long.Parse(hfIdVentaDetalle.Value);
            CargarDetalleEnModal(idVenta); // tu lógica

            // Mostrar modal después del postback
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "$('#modalDetalleCompra').modal('show');", true);
        }


        private void CargarDetalleEnModal(long idVenta)
        {
            var ventaManager = new VentaManager();
            var venta = ventaManager.ObtenerVentaPorIdConDetalles(idVenta);

            if (venta == null)
            {
                // Mensaje error
                ScriptManager.RegisterStartupScript(this, GetType(), "error",
                    "Swal.fire('Error', 'No se encontró la compra', 'error');", true);
                return;
            }

            // Llenar labels
            lblIdCompraModal.Text = venta.Id.ToString();
            lblFechaModal.Text = venta.FechaVenta.ToString("dd/MM/yyyy HH:mm");
            lblTipoPagoModal.Text = venta.TipoPago.Nombre;
            lblEstadoPagoModal.Text = venta.EstadoPago.Nombre;
            lblEstadoPreparacionModal.Text = venta.EstadoPreparacion.Nombre;
            lblEstadoEnvioModal.Text = venta.EstadoEnvio.Nombre;
            lblNombreModal.Text = venta.Cliente.Nombre + " " + venta.Cliente.Apellido;
            lblEmailModal.Text = venta.Cliente.Usuario.Email;
            lblTelefonoModal.Text = venta.Cliente.Telefono ?? "No registrado";
            lblRazonSocialModal.Text = venta.Cliente.RazonSocial ?? "No registrada";
            lblTotalModal.Text = venta.MontoTotal.ToString("N0");

            // Productos
            gvProductosModal.DataSource = venta.Detalles;
            gvProductosModal.DataBind();

            // Comprobante
            if (!string.IsNullOrEmpty(venta.Comprobante))
            {
                imgComprobanteModal.ImageUrl = venta.Comprobante;
                lnkComprobanteModal.NavigateUrl = venta.Comprobante;
                phComprobanteModal.Visible = true;
                phSinComprobanteModal.Visible = false;
            }
            else
            {
                phComprobanteModal.Visible = false;
                phSinComprobanteModal.Visible = true;
            }
        }

        private void CargarCompras()
        {
            if (Session["cliente"] == null)
            {
                Response.Redirect("LoginCliente.aspx");
                return;
            }

            Cliente cli = (Cliente)Session["cliente"];
            VentaManager manager = new VentaManager();
            gvCompras.DataSource = manager.ObtenerVentasPorCliente(cli.Id);
            gvCompras.DataBind();
        }

        // COLORES - COMPATIBLE CON C# 7.3
        protected string GetBadgeClassPago(object idObj)
        {
            int id = Convert.ToInt32(idObj);
            switch (id)
            {
                case 1: return "bg-warning text-dark"; // Pendiente
                case 2: return "bg-success";           // Aprobado
                case 3: return "bg-danger";            // Rechazado
                case 4: return "bg-secondary";         // Pendiente comprobante
                default: return "bg-secondary";
            }
        }

        protected string GetBadgeClassPreparacion(object idObj)
        {
            int id = Convert.ToInt32(idObj);
            switch (id)
            {
                case 1: return "bg-secondary"; // No iniciado
                case 2: return "bg-info text-dark"; // En preparación
                case 3: return "bg-primary"; // Listo para envío
                case 4: return "bg-danger"; // Cancelado
                default: return "bg-secondary";
            }
        }

        protected string GetBadgeClassEnvio(object idObj)
        {
            int id = Convert.ToInt32(idObj);
            switch (id)
            {
                case 1: return "bg-secondary"; // No iniciado
                case 2: return "bg-warning text-dark"; // En camino
                case 3: return "bg-success"; // Entregado
                case 4: return "bg-danger"; // Devuelto
                case 5: return "bg-danger"; // Cancelado
                default: return "bg-secondary";
            }
        }
    }
}

