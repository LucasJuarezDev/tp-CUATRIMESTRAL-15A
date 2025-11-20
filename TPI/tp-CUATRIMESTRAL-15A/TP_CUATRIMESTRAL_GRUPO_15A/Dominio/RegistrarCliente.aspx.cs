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
    public partial class RegistrarUsuario : System.Web.UI.Page
    {
        ClienteManager ClienteManager = new ClienteManager();

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private void MostrarExitoYRedirigir()
        {
            string script = @"
                Swal.fire({
                    icon: 'success',
                    title: '¡Cuenta creada!',
                    text: 'Usuario registrado exitosamente',
                    confirmButtonText: 'Aceptar',
                    confirmButtonColor: '#28a745',
                    allowOutsideClick: false
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.href = 'LoginCliente.aspx';
                    }
                });";

            ClientScript.RegisterStartupScript(this.GetType(), "RegistroExitoso", script, true);
        }

        protected void btnRegistrarse_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return; // ← ¡Usamos los validadores ASP.NET! (más pro)

            try
            {
                Rol rolCliente = new Rol { Id = 3, Nombre = "CLIENTE" };

                // Usuario
                Usuario usuario = new Usuario
                {
                    Nickname = txtUsuario.Text.Trim(),
                    Contrasena = txtPassword.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Activo = true,
                    Rol = rolCliente
                };

                // Cliente completo
                Cliente nuevoCliente = new Cliente
                {
                    Nombre = txtNombre.Text.Trim(),
                    Apellido = txtApellido.Text.Trim(),
                    Telefono = txtTelefono.Text.Trim(),
                    Usuario = usuario,
                    Rol = rolCliente,
                    RazonSocial = ddlEsEmpresa.SelectedValue == "1" ? txtRazonSocial.Text.Trim() : null
                };

                // Registro
                ClienteManager.RegistrarCliente(nuevoCliente, txtPassword.Text.Trim());

                MostrarExitoYRedirigir();
            }
            catch (Exception ex)
            {
                MostrarErrorSwal(ex.Message);
            }
        }

        private void MostrarErrorSwal(string mensaje)
        {
            string mensajeLimpio = mensaje.Replace("'", "\\'").Replace("\r\n", " ").Replace("\n", " ");

            string script = $@"
                Swal.fire({{
                    icon: 'error',
                    title: 'Error',
                    text: '{mensajeLimpio}',
                    confirmButtonText: 'Aceptar',
                    confirmButtonColor: '#d33'
                }});";

            ClientScript.RegisterStartupScript(this.GetType(), "ErrorRegistro", script, true);
        }
    }
}