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
    public partial class Productos : System.Web.UI.Page
    {
        private ProductoManager manager = new ProductoManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarGrilla();
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

        protected void gvProductos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                long id = Convert.ToInt64(e.CommandArgument);
                ProductoManager manager = new ProductoManager();

                try
                {
                    manager.Eliminar(id);
                    // Recargar la grilla despues de eliminar
                    gvProductos.DataSource = manager.Listar();
                    gvProductos.DataBind();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al eliminar la categoría: " + ex.Message);
                }
            }
        }
    }
}