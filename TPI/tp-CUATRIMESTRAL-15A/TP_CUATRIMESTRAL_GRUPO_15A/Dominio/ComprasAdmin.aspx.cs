using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dominio
{
    public partial class ComprasAdmin : System.Web.UI.Page
    {
        private readonly VentaManager ventaManager = new VentaManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarPedidos();
                CargarDropDowns();
            }
        }

        private void CargarPedidos()
        {
            var lista = ventaManager.ListarTodasConDetalleYEstados();
            gvPedidos.DataSource = lista;
            gvPedidos.DataBind();
            lblTotal.Text = lista.Count.ToString();
        }

        private void CargarDropDowns()
        {
            CargarDDL(ddlEstadoPago, ventaManager.ListarEstadosPago());
            CargarDDL(ddlEstadoPreparacion, ventaManager.ListarEstadosPreparacion());
            CargarDDL(ddlEstadoEnvio, ventaManager.ListarEstadosEnvio());
        }

        private void CargarDDL(DropDownList ddl, List<object> lista)
        {
            ddl.DataValueField = "Id";
            ddl.DataTextField = "Nombre";
            ddl.DataSource = lista;
            ddl.DataBind();
        }

        protected void btnGuardarEstados_Click(object sender, EventArgs e)
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

        protected void gvPedidos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "GenerarFactura")
            {
                long idVenta = Convert.ToInt64(e.CommandArgument);
                Response.Redirect($"Factura.aspx?id={idVenta}");
            }
        }
    }
}