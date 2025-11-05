using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clases;
using Manager;

namespace Dominio
{
    public partial class Categorias : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CategoriaManager categoriaManager = new CategoriaManager();
            DGVcategorias.DataSource = categoriaManager.Listar();
            DGVcategorias.DataBind();
        }

        protected void DGVcategorias_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                long id = Convert.ToInt64(e.CommandArgument);
                CategoriaManager manager = new CategoriaManager();

                try
                {
                    manager.EliminarLogico(id);
                    // Recargar la grilla despues de eliminar
                    DGVcategorias.DataSource = manager.Listar();
                    DGVcategorias.DataBind();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al eliminar la categoría: " + ex.Message);
                }
            }
        }



    }
}
