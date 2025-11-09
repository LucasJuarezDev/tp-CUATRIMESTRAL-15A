using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clases;
using Manager;
using System.Linq;

namespace Dominio
{
    public partial class Categorias : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarCategorias();
            }
        }

        private void CargarCategorias()
        {
            CategoriaManager categoriaManager = new CategoriaManager();
            DGVcategorias.DataSource = categoriaManager.Listar();
            DGVcategorias.DataBind();
        }

        protected void DGVcategorias_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            CategoriaManager manager = new CategoriaManager();

            // ID de la categoria desde el CommandArgument
            long id = Convert.ToInt64(e.CommandArgument);

            if (e.CommandName == "Eliminar")
            {
                try
                {
                    manager.EliminarLogico(id);
                    CargarCategorias(); // Recarga la grilla
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al eliminar la categoría: " + ex.Message);
                }
            }
            else if (e.CommandName == "Editar")
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

