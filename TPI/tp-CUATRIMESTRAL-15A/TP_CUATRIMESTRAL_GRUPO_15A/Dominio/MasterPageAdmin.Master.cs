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
        protected void Page_Load(object sender, EventArgs e)
        {
            // 1. BUSCAR EL USUARIO LOGUEADO (tu clave es "usuario" en minúscula)
            var usuarioLogueado = Session["usuario"] as UsuarioLogueado;

            if (usuarioLogueado == null)
            {
                Response.Redirect("~/LoginCliente.aspx", true);
                return;
            }

            // 2. OBTENER EL ROL (ahora SÍ tiene Rol cargado)
            int rolId = usuarioLogueado.Rol?.Id ?? 0;
            bool esAdmin = (rolId == 1);

            // 3. MOSTRAR MENÚ DE ADMIN
            var ph = FindControl("phMenuAdmin") as PlaceHolder;
            if (ph != null)
                ph.Visible = esAdmin;

            if (rolId == 1)
                Page.Title = "Panel Administrador";
            else if (rolId == 2)
                Page.Title = "Panel Empleado";
        }
    }
}