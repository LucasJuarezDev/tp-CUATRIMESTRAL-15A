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
    }
}