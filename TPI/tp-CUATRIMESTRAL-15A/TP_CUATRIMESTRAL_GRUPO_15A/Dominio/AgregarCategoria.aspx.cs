using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dominio
{
    public partial class AgregarCategoria : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            string descripcion = txtDescripcion.Text.Trim();

            try
            {
                CategoriaManager manager = new CategoriaManager();

                // Insertamos la nueva categoría
                manager.Agregar(nombre, descripcion);
                

                // Redirige al listado
                Response.Redirect("Categorias.aspx", false);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al guardar la categoría: " + ex.Message, ex);
            }
        }
    }
}
