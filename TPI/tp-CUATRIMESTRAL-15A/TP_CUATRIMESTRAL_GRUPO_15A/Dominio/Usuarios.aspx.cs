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
            if (e.CommandName == "Eliminar")
            {
                long id = Convert.ToInt64(e.CommandArgument);
                UsuarioManager manager = new UsuarioManager();

                try
                {
                    manager.Eliminar(id);
                    // Recargar la grilla despues de eliminar
                    gvUsuarios.DataSource = manager.Listar();
                    gvUsuarios.DataBind();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al eliminar la categoría: " + ex.Message);
                }
            }
        }
    }
}