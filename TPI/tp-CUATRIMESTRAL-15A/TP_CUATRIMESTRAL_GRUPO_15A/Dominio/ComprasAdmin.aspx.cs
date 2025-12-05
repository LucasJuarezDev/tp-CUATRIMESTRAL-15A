using Clases;
using Manager;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dominio
{
    public partial class ComprasAdmin : AuthenticationPage
    {
        private readonly VentaManager ventaManager = new VentaManager();
        private readonly EstadoPagoManager estadoPagoManager = new EstadoPagoManager();
        private readonly EstadoPreparacionManager estadoPreparacionManager = new EstadoPreparacionManager();
        private readonly EstadoEnvioManager estadoEnvioManager = new EstadoEnvioManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ConfigurarPaginacion();
                CargarPedidos();
                CargarDropDowns();
                divComprobante.Visible = false; // Oculta el preview al inicio
            }
        }

        private void ConfigurarPaginacion()
        {
            if (ViewState["PageSize"] != null)
                gvPedidos.PageSize = (int)ViewState["PageSize"];
            else
                gvPedidos.PageSize = 10;

            ddlPageSize.SelectedValue = gvPedidos.PageSize.ToString();
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvPedidos.PageIndex = 0;
            ViewState["PageSize"] = int.Parse(ddlPageSize.SelectedValue);
            gvPedidos.PageSize = (int)ViewState["PageSize"];
            CargarPedidos();
        }

        protected void gvPedidos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPedidos.PageIndex = e.NewPageIndex;
            CargarPedidos();
        }

        private void CargarPedidos()
        {
            var lista = ventaManager.ListarTodasConDetalleYEstados();
            gvPedidos.DataSource = lista;
            gvPedidos.DataBind();

            lblTotal.Text = lista.Count.ToString();
            lblPaginaActual.Text = (gvPedidos.PageIndex + 1).ToString();
            lblTotalPaginas.Text = gvPedidos.PageCount.ToString();
            divPaginacion.Visible = gvPedidos.PageCount > 1;
        }

        private void CargarDropDowns()
        {
            CargarDDL(ddlEstadoPago, estadoPagoManager.Listar());
            CargarDDL(ddlEstadoPreparacion, estadoPreparacionManager.Listar());
            CargarDDL(ddlEstadoEnvio, estadoEnvioManager.Listar());
        }

        private void CargarDDL<T>(DropDownList ddl, List<T> lista)
        {
            ddl.DataValueField = "Id";
            ddl.DataTextField = "Nombre";
            ddl.DataSource = lista;
            ddl.DataBind();
        }

        // -------------------------------------------------------
        // Mostrar Comprobante
        // -------------------------------------------------------
        private void MostrarComprobante(long idVenta)
        {
            string ruta = ventaManager.ObtenerComprobantePorId(idVenta);

            if (!string.IsNullOrEmpty(ruta))
            {
                imgComprobante.ImageUrl = ruta;
                lnkDetalle.NavigateUrl = ruta;
                divComprobante.Visible = true;
            }
            else
            {
                divComprobante.Visible = false;
                ScriptManager.RegisterStartupScript(this, GetType(), "noimg",
                    "Swal.fire('Sin comprobante', 'Este pedido no tiene comprobante cargado', 'warning');", true);
            }
        }

        protected void btnGuardarEstados_Click(object sender, EventArgs e)
        {
            try
            {
                long idVenta = long.Parse(hfIdVenta.Value);
                int pago = int.Parse(ddlEstadoPago.SelectedValue);
                int prep = int.Parse(ddlEstadoPreparacion.SelectedValue);
                int envio = int.Parse(ddlEstadoEnvio.SelectedValue);

                ventaManager.CambiarEstadoPago(idVenta, pago);
                ventaManager.CambiarEstadoPreparacion(idVenta, prep);
                ventaManager.CambiarEstadoEnvio(idVenta, envio);

                CargarPedidos();
                ScriptManager.RegisterStartupScript(this, GetType(), "success",
                    "Swal.fire('¡Perfecto!', 'Estados actualizados correctamente', 'success');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error",
                    $"Swal.fire('Error', '{ex.Message}', 'error');", true);
            }
        }

        // -------------------------------------------------------
        // para el Boton Ver Comprobante
        // -------------------------------------------------------
        protected void gvPedidos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            long idVenta = Convert.ToInt64(e.CommandArgument);

            if (e.CommandName == "GenerarFactura")
            {
                Response.Redirect($"Factura.aspx?id={idVenta}");
            }
            else if (e.CommandName == "VerComprobante")
            {
                MostrarComprobante(idVenta);
            }
        }

        protected void btnCerrarComprobante_Click(object sender, EventArgs e)
        {
            divComprobante.Visible = false;
            imgComprobante.ImageUrl = "";
            lnkDetalle.NavigateUrl = "";
        }

    }
}

