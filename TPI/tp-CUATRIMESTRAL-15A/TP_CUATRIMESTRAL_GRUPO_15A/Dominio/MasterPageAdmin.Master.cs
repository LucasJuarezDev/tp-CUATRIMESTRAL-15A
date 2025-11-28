using Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dominio
{
    public partial class MasterPageAdmin : System.Web.UI.MasterPage
    {
        // PROPIEDAD PÚBLICA PARA USAR EN EL ASPX
        public UsuarioLogueado UsuarioActual => Session["usuario"] as UsuarioLogueado;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (UsuarioActual == null)
                {
                    Response.Redirect("~/LoginCliente.aspx", true);
                    return;
                }

                int rolId = UsuarioActual.Rol?.Id ?? 0;
                bool esAdmin = (rolId == 1);

                var ph = FindControl("phMenuAdmin") as PlaceHolder;
                if (ph != null)
                    ph.Visible = esAdmin;

                Page.Title = rolId == 1 ? "Panel Administrador" : "Panel Empleado";

                // ACTIVA EL DATABINDING PARA QUE FUNCIONE <%# %>
                this.DataBind();
            }
        }
    }
}