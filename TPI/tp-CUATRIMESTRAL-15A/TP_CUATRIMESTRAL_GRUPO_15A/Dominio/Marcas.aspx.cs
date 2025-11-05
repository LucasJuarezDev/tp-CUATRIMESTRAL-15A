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
    public partial class Marcas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MarcaManager marcaManager = new MarcaManager();
            DGVmarcas.DataSource = marcaManager.Listar();
            DGVmarcas.DataBind();
        }

        protected void DGVmarcas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                long id = Convert.ToInt64(e.CommandArgument);
                MarcaManager manager = new MarcaManager();

                try
                {
                    manager.EliminarLogico(id);
                    // Recargar la grilla despues de eliminar
                    DGVmarcas.DataSource = manager.Listar();
                    DGVmarcas.DataBind();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al eliminar la marca: " + ex.Message);
                }
            }
        }
    }
}