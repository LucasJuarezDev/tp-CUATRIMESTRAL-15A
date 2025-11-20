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
                gvProductos.DataSource = manager.Listar();
                gvProductos.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void eliminarProducto(long id)
        {
            try
            {
                manager.Eliminar(id);
                CargarGrilla();
                string script = @"
                    Swal.fire({
                        title: '¡Eliminado!',
                        text: 'El producto ha sido eliminado.',
                        icon: 'success',
                        timer: 2000,
                        showConfirmButton: false
                    });";

                ClientScript.RegisterStartupScript(this.GetType(), "eliminarExito", script, true);
            }
            catch (Exception ex)
            {
                string mensajeError = ex.Message.Replace("'", @"\'");

                string script = $@"
                    Swal.fire({{
                        title: 'Error',
                        text: 'No se pudo eliminar: {mensajeError}',
                        icon: 'error'
                    }});";

                ClientScript.RegisterStartupScript(this.GetType(), "eliminarError", script, true);
            }
        }

        protected void gvProductos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                long id = Convert.ToInt64(e.CommandArgument);
                Response.Redirect($"AgregarProducto.aspx?id={id}");
            }
        }
    }
}