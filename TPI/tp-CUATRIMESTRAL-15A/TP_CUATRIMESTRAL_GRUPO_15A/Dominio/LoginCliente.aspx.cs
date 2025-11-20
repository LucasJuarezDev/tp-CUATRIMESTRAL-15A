using Clases;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dominio
{
    public partial class LoginCliente : System.Web.UI.Page
    {
        LoginManager loginmanager = new LoginManager();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                var usuario = loginmanager.Login(txtUsuario.Text.Trim(), txtPassword.Text.Trim());

                if (usuario == null)
                {
                    MostrarError("Usuario o contraseña incorrectos");
                    return;
                }

                // Guardar en sesión (ahora es un objeto fuerte)
                Session["usuario"] = usuario;

                // Redirección según rol
                if (usuario.Rol.Id == 3) // Cliente
                {
                    Response.Redirect("Catalogo.aspx");
                }
                else // Admin o empleado
                {
                    Response.Redirect("Dashboard.aspx");
                }
            }
            catch (Exception ex)
            {
                MostrarError("Error del sistema: " + ex.Message);
            }
        }

        // MÉTODOS SEGUROS
        private void MostrarError(string mensaje)
        {
            string safe = mensaje.Replace("'", "\\'").Replace("\n", "").Replace("\r", "");
            string script = $@"Swal.fire({{
        icon: 'error',
        title: 'Error',
        text: '{safe}',
        confirmButtonText: 'Aceptar'
    }});";
            ClientScript.RegisterStartupScript(this.GetType(), "swal_error", script, true);
        }

        private void MostrarExitoYRedirigir(string pagina)
        {
            string script = $@"Swal.fire({{
        icon: 'success',
        title: '¡Bienvenido!',
        text: 'Sesión iniciada correctamente',
        timer: 1500,
        showConfirmButton: false
    }}).then(() => {{ 
        window.location.href = '{pagina}'; 
    }});";
            ClientScript.RegisterStartupScript(this.GetType(), "swal_exito", script, true);
        }
    }
}