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
    public partial class alterarEmpleado : AuthenticationPage
    {
        private EmpleadoManager empleadoManager = new EmpleadoManager();
        public long? EmpleadoId => long.TryParse(Request.QueryString["id"], out long id) ? id : (long?)null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!EmpleadoId.HasValue)
                {
                    MostrarMensaje("ID inválido.", "alert-danger");
                    Response.Redirect("gestionEmpleados.aspx");
                    return;
                }
                CargarEmpleado();
            }
        }

        private void CargarEmpleado()
        {
            try
            {
                var empleado = empleadoManager.BuscarPorId(EmpleadoId.Value);
                if (empleado == null)
                {
                    MostrarMensaje("Empleado no encontrado.", "alert-danger");
                    Response.Redirect("gestionEmpleados.aspx");
                    return;
                }

                txtNombre.Text = empleado.Nombre ?? "";
                txtApellido.Text = empleado.Apellido ?? "";
                txtTelefono.Text = empleado.Telefono ?? "";
                txtSueldo.Text = empleado.Sueldo.ToString("F0");

            }
            catch (Exception ex)
            {
                MostrarMensaje("Error: " + ex.Message, "alert-danger");
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                var empleado = new Empleado
                {
                    Id = EmpleadoId.Value,
                    Nombre = txtNombre.Text.Trim(),
                    Apellido = txtApellido.Text.Trim(),
                    Telefono = string.IsNullOrWhiteSpace(txtTelefono.Text) ? null : txtTelefono.Text.Trim(),
                    Sueldo = decimal.Parse(txtSueldo.Text)
                };

                empleadoManager.Modificar(empleado);
                MostrarMensaje("Empleado actualizado correctamente.", "alert-success");
                Response.AddHeader("REFRESH", "2;URL=gestionEmpleados.aspx");
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
    }
}