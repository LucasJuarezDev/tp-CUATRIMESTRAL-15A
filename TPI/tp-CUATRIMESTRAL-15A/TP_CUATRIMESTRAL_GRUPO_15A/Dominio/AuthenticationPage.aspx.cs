using Clases;
using Manager;
using System;
using System.Web.UI;

namespace Dominio
{
    public class AuthenticationPage : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("~/LoginCliente.aspx");
                return;
            }

            var usuario = (UsuarioLogueado)Session["usuario"];  

            if (EsPaginaAdmin() && usuario.Rol.Id == 3) 
            {
                Response.Redirect("~/Catalogo.aspx");
            }

            base.OnLoad(e);
        }

        private bool EsPaginaAdmin()
        {
            string url = Request.Path.ToLower();
            return url.Contains("dashboard") ||
                   url.Contains("usuarios") ||
                   url.Contains("categorias") ||
                   url.Contains("marcas") ||
                   url.Contains("productos") ||
                   url.Contains("gestionempleados") ||
                   url.Contains("admin");
        }
    }
}