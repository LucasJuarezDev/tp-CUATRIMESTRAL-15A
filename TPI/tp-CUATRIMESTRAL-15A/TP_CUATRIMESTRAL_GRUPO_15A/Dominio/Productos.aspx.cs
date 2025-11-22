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
    public partial class Productos : AuthenticationPage
    {
        private ProductoManager manager = new ProductoManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarGrilla();
            }
            else
            {
                // CAPTURA EL FILTRO EN VIVO
                if (Request["__EVENTTARGET"] == "filtrarProductos")
                {
                    string texto = Request["__EVENTARGUMENT"];
                    txtBuscar.Text = texto;
                    gvProductos.PageIndex = 0;
                    CargarGrilla();
                }

                // TU CÓDIGO DE ELIMINAR CON __doPostBack (lo dejás igual)
                string eventTarget = Request["__EVENTTARGET"];
                string eventArgument = Request["__EVENTARGUMENT"];
                if (eventTarget == "eliminarProducto")
                {
                    long id = Convert.ToInt64(eventArgument);
                    eliminarProducto(id);
                }
            }
        }

        private void CargarGrilla()
        {
            try
            {
                string filtro = txtBuscar.Text.Trim();
                int pageSize = int.Parse(ddlPageSize.SelectedValue);

                var lista = manager.ListarConFiltro(filtro);  // ← NUEVO MÉTODO

                gvProductos.PageSize = pageSize;
                gvProductos.DataSource = lista;
                gvProductos.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // CAMBIO DE CANTIDAD DE REGISTROS
        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvProductos.PageIndex = 0;
            CargarGrilla();
        }

        // BUSCADOR
        protected void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            gvProductos.PageIndex = 0;
            CargarGrilla();
        }

        // PAGINACIÓN
        protected void gvProductos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProductos.PageIndex = e.NewPageIndex;
            CargarGrilla();
        }

        // EDITAR
        protected void gvProductos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                long id = Convert.ToInt64(e.CommandArgument);
                Response.Redirect($"AgregarProducto.aspx?id={id}");
            }
        }

        // ELIMINAR
        private void eliminarProducto(long id)
        {
            try
            {
                manager.Eliminar(id);
                CargarGrilla();

                string script = @"
                Swal.fire({
                    title: '¡Eliminado!',
                    text: 'El producto ha sido eliminado correctamente.',
                    icon: 'success',
                    timer: 2000,
                    showConfirmButton: false
                });";
                ClientScript.RegisterStartupScript(this.GetType(), "eliminarExito", script, true);
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message.Replace("'", @"\'");
                string script = @"
                Swal.fire({
                    title: 'Error',
                    text: 'No se pudo eliminar: {mensaje}',
                    icon: 'error'
                });";
                ClientScript.RegisterStartupScript(this.GetType(), "eliminarError", script, true);
            }
        }
    }
}