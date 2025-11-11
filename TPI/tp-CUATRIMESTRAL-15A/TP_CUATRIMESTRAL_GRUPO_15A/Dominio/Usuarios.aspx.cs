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
    public partial class Usuarios : System.Web.UI.Page
    {
        private UsuarioManager manager = new UsuarioManager();

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

                if (eventTarget == "EliminarUsuario")
                {
                    long id = Convert.ToInt64(eventArgument);
                    EliminarUsuario(id);
                }
            }
        }

        private void CargarGrilla()
        {
            try
            {
                gvUsuarios.DataSource = manager.Listar();
                gvUsuarios.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void gvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                long id = Convert.ToInt64(e.CommandArgument);
                Response.Redirect($"RegistrarUsuario.aspx?id={id}");
            }
        }

        private void EliminarUsuario(long id)
        {
            try
            {
                manager.Eliminar(id);
                CargarGrilla();

                // SCRIPT CORRECTO: con @ + saltos de línea + ;
                string script = @"
                Swal.fire({
                    title: '¡Eliminado!',
                    text: 'El usuario ha sido eliminado.',
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
    }
}
