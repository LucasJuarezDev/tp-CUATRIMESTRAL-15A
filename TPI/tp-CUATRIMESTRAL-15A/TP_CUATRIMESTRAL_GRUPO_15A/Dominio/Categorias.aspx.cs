using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clases;
using Manager;
using System.Linq;

namespace Dominio
{
    public partial class Categorias : AuthenticationPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarCategorias();
            }
            else
            {
                string eventTarget = Request["__EVENTTARGET"];
                string eventArgument = Request["__EVENTARGUMENT"];

                if (eventTarget == "eliminarCategoria")
                {
                    long id = Convert.ToInt64(eventArgument);
                    eliminarCategoria(id);
                }
            }
        }

        private void CargarCategorias()
        {
            CategoriaManager categoriaManager = new CategoriaManager();
            DGVcategorias.DataSource = categoriaManager.Listar();
            DGVcategorias.DataBind();
        }

        private void eliminarCategoria(long id)
        {
            CategoriaManager manager = new CategoriaManager();
            try
            {
                manager.EliminarLogico(id);
                CargarCategorias();
                string script = @"
                    Swal.fire({
                        title: '¡Eliminado!',
                        text: 'La categoria ha sido eliminado.',
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

        protected void DGVcategorias_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            CategoriaManager manager = new CategoriaManager();

            // ID de la categoria desde el CommandArgument
            long id = Convert.ToInt64(e.CommandArgument);
            if (e.CommandName == "Editar")
            {
                // Buscar la categoria en la lista
                Categoria seleccionada = manager.Listar().Find(x => x.Id == id);

                if (seleccionada != null)
                {
                    // Guardar en sesion y redirigir
                    Session["categoriaSeleccionada"] = seleccionada;
                    Response.Redirect("ModificarCategoria.aspx");
                }
            }
        }
    }
}

