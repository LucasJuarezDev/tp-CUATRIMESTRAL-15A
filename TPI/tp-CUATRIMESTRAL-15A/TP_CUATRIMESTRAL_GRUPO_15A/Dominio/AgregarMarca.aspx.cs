using Manager;
using System;
using System.Web.UI;

namespace Dominio
{
    public partial class AgregarMarca : AuthenticationPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return; // Validación del RequiredFieldValidator

            string nombre = txtNombre.Text.Trim();
            string descripcion = txtDescripcion.Text.Trim();

            try
            {
                MarcaManager manager = new MarcaManager();
                manager.Agregar(nombre, descripcion);

                // SweetAlert de éxito + redirección
                string script = @"
                    Swal.fire({
                        icon: 'success',
                        title: '¡Marca agregada!',
                        text: 'La marca fue creada correctamente.',
                        timer: 2000,
                        showConfirmButton: false
                    }).then(() => {
                        window.location.href = 'Marcas.aspx';
                    });";

                ScriptManager.RegisterStartupScript(this, GetType(), "exito", script, true);
            }
            catch (Exception ex)
            {
                string scriptError = $@"Swal.fire({{icon: 'error', title: 'Error', text: '{ex.Message.Replace("'", "\\'")}'}});";
                ScriptManager.RegisterStartupScript(this, GetType(), "error", scriptError, true);
            }
        }
    }
}