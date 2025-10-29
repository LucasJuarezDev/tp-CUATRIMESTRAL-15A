using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dominio
{
    public partial class AgregarMarca : System.Web.UI.Page
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
                MarcaManager manager = new MarcaManager();
                manager.Agregar(nombre, descripcion);

                Response.Redirect("Marcas.aspx", false);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al guardar la marca: " + ex.Message, ex);
            }
        }
    }
}