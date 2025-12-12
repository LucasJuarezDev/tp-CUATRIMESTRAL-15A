using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dominio
{
    public partial class AgregarCategoria : AuthenticationPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return; // Esto ya valida el RequiredFieldValidator

            string nombre = txtNombre.Text.Trim();
            string descripcion = txtDescripcion.Text.Trim();

            try
            {
                CategoriaManager manager = new CategoriaManager();
                manager.Agregar(nombre, descripcion);

                // SweetAlert de éxito + redirección
                string script = @"
            Swal.fire({
                icon: 'success',
                title: '¡Categoría agregada!',
                text: 'La categoría fue creada correctamente.',
                timer: 2000,
                showConfirmButton: false
            }).then(() => {
                window.location.href = 'Categorias.aspx';
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
