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
    public partial class MiCuenta : System.Web.UI.Page
    {
        private readonly ClienteManager clienteManager = new ClienteManager();
        private readonly UsuarioManager usuarioManager = new UsuarioManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDatosCliente();
            }
        }

        private void CargarDatosCliente()
        {
            var usuarioLogueado = Session["usuario"] as UsuarioLogueado;
            if (usuarioLogueado == null || usuarioLogueado.Rol?.Id != 3)
            {
                Response.Redirect("Catalogo.aspx");
                return;
            }

            var cliente = clienteManager.ObtenerPorIdUsuario(usuarioLogueado.Id);
            if (cliente == null) return;

            // DATOS DEL CLIENTE
            txtNombre.Text = cliente.Nombre;
            txtApellido.Text = cliente.Apellido;
            txtTelefono.Text = cliente.Telefono ?? "";
            txtRazonSocial.Text = cliente.RazonSocial ?? "";

            // DATOS DEL USUARIO (desde el objeto anidado)
            txtEmail.Text = cliente.Usuario.Email ?? "";

            // Mostrar razón social si existe
            if (!string.IsNullOrEmpty(cliente.RazonSocial))
            {
                chkEmpresa.Checked = true;
                divRazonSocial.Visible = true;
            }
        }

        protected void chkEmpresa_CheckedChanged(object sender, EventArgs e)
        {
            divRazonSocial.Visible = chkEmpresa.Checked;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                    var usuarioLogueado = Session["usuario"] as UsuarioLogueado;
                    var cliente = clienteManager.ObtenerPorIdUsuario(usuarioLogueado.Id);

                    // ACTUALIZAR CLIENTE
                    cliente.Nombre = txtNombre.Text.Trim();
                    cliente.Apellido = txtApellido.Text.Trim();
                    cliente.Telefono = string.IsNullOrWhiteSpace(txtTelefono.Text) ? null : txtTelefono.Text.Trim();
                    cliente.RazonSocial = chkEmpresa.Checked ? txtRazonSocial.Text.Trim() : null;

                    clienteManager.Actualizar(cliente);

                    // ACTUALIZAR EMAIL EN USUARIO
                    if (cliente.Usuario.Email != txtEmail.Text.Trim())
                    {
                        clienteManager.ActualizarEmail(usuarioLogueado.Id, txtEmail.Text.Trim());
                        cliente.Usuario.Email = txtEmail.Text.Trim();
                        usuarioLogueado.Email = txtEmail.Text.Trim();
                        Session["usuario"] = usuarioLogueado;
                    }

                    // CAMBIO DE CONTRASEÑA
                    if (!string.IsNullOrEmpty(txtPassActual.Text))
                    {
                        if (txtPassNueva.Text != txtPassNueva2.Text)
                            throw new Exception("Las nuevas contraseñas no coinciden");

                        // ← LÍNEA CORRECTA (SIN EL ERROR)
                        clienteManager.CambiarContraseña(usuarioLogueado.Id, txtPassActual.Text, txtPassNueva.Text);

                        // Limpiar campos
                        txtPassActual.Text = "";
                        txtPassNueva.Text = "";
                        txtPassNueva2.Text = "";
                    }

                MostrarExito("¡Datos actualizados correctamente!");
            }
        catch (Exception ex)
            {
                MostrarError(ex.Message);
            }
        }

        private void MostrarExito(string mensaje)
        {
            string script = $@"Swal.fire({{ title: '¡Perfecto!', text: '{mensaje}', icon: 'success', confirmButtonText: 'Genial' }});";
            ScriptManager.RegisterStartupScript(this, GetType(), "exito", script, true);
        }

        private void MostrarError(string mensaje)
        {
            string script = $@"Swal.fire({{ title: 'Error', text: '{mensaje}', icon: 'error', confirmButtonText: 'Aceptar' }});";
            ScriptManager.RegisterStartupScript(this, GetType(), "error", script, true);
        }
    }
}