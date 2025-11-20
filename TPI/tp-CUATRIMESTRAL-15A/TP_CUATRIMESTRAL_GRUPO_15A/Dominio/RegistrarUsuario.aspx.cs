using Clases;
using Manager;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dominio
{
    public partial class RegistrarUsuario1 : AuthenticationPage
    {
        private UsuarioManager usuarioManager = new UsuarioManager();
        public long? UsuarioId => long.TryParse(Request.QueryString["id"], out long id) ? id : (long?)null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarRoles();

                if (UsuarioId.HasValue)
                {
                    ModoEdicion();
                    CargarUsuario(UsuarioId.Value);
                }
                else
                {
                    ModoCrear();
                }
            }

            if (Session["Exito"] != null)
            {
                string mensaje = Session["Exito"].ToString();
                Session.Remove("Exito");
                MostrarExitoJS(mensaje);
            }
        }

        private void ModoCrear()
        {
            lblTitulo.InnerText = "Crear cuenta";
            btnRegistrarse.Text = "CREAR USUARIO";
        }

        private void ModoEdicion()
        {
            lblTitulo.InnerText = "Modificar Usuario";
            btnRegistrarse.Text = "MODIFICAR USUARIO";
            btnRegistrarse.CssClass = "btn btn-warning w-100 fw-bold py-2"; 
        }

        private void CargarRoles()
        {
            ddlRol.Items.Clear();
            ddlRol.Items.Add(new ListItem("-- Seleccionar rol --", ""));
            ddlRol.Items.Add(new ListItem("Empleado", "2"));
            ddlRol.Items.Add(new ListItem("Cliente", "3"));
        }

        private void CargarUsuario(long id)
        {
            try
            {
                var usuario = usuarioManager.buscarPorId(id);
                if (usuario == null)
                {
                    MostrarMensaje("Usuario no encontrado.", "alert-danger");
                    Response.Redirect("Usuarios.aspx");
                    return;
                }

                txtNickname.Text = usuario.Nickname;
                txtEmail.Text = usuario.Email;
                ddlRol.SelectedValue = usuario.Rol.Id.ToString();

                //txtPassword.Text = usuario.Contrasena;
                //txtRepetirPassword.Text = usuario.Contrasena;
                //txtPassword.Text = "";
                //txtRepetirPassword.Text = "";
                //txtPassword.Attributes.Add("placeholder", "Dejar vacío para mantener actual");
                //txtRepetirPassword.Attributes.Add("placeholder", "Repetir nueva contraseña");
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al cargar: " + ex.Message, "alert-danger");
            }
        }

        protected void btnRegistrarse_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid && !UsuarioId.HasValue) return;

            try
            {
                var usuario = new Usuario
                {
                    Nickname = txtNickname.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Rol = new Rol { Id = byte.Parse(ddlRol.SelectedValue) },
                    Activo = true
                };


                if (UsuarioId.HasValue)
                {
                    usuario.Id = UsuarioId.Value;
                    usuarioManager.Modificar(usuario);
                    Session["Exito"] = "Usuario modificado correctamente.";
                }
                else
                {
                    if (string.IsNullOrEmpty(txtPassword.Text))
                    {
                        MostrarMensaje("La contraseña es obligatoria al crear.", "alert-danger");
                        return;
                    }
                    if (txtPassword.Text != txtRepetirPassword.Text)
                    {
                        MostrarMensaje("Las contraseñas no coinciden.", "alert-danger");
                        return;
                    }

                    usuario.Contrasena = txtPassword.Text;
                    long id = usuarioManager.Agregar(usuario);
                    Session["Exito"] = $"Usuario creado. ID: {id}";
                }

                Response.Redirect(Request.RawUrl, false);
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error: " + ex.Message, "alert-danger");
            }
        }

        private void MostrarMensaje(string texto, string tipo)
        {
            lblMensaje.Text = texto;
            lblMensaje.CssClass = $"alert {tipo} d-block text-center";
            lblMensaje.Visible = true;
        }

        private void MostrarExitoJS(string mensaje)
        {
            string script = $@"
                <script src='https://cdn.jsdelivr.net/npm/sweetalert2@11'></script>
                <script>
                    Swal.fire({{
                        title: 'Éxito',
                        text: '{mensaje}',
                        icon: 'success',
                        confirmButtonText: 'Aceptar'
                    }}).then(() => {{
                        window.location.href = 'Usuarios.aspx';
                    }});
                </script>";
            ClientScript.RegisterStartupScript(this.GetType(), "exito", script, false);
        }

        private void LimpiarFormulario()
        {
            txtNickname.Text = "";
            txtEmail.Text = "";
            txtPassword.Text = "";
            txtRepetirPassword.Text = "";
            ddlRol.SelectedIndex = 0;
        }
    }
}