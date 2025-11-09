using System;
using Clases;
using Manager;

namespace Dominio
{
    public partial class ModificarMarca : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Recupero la marca seleccionada desde la sesion
                Marca marca = Session["marcaSeleccionada"] as Marca;

                if (marca != null)
                {
                    txtNombre.Text = marca.Nombre;
                    txtDescripcion.Text = marca.Descripcion;
                }
                else
                {
                    // Si no hay marca en sesion, volver a la pagina principal
                    Response.Redirect("Marcas.aspx");
                }
            }
        }

        protected void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            try
            {
                Marca marca = Session["marcaSeleccionada"] as Marca;

                if (marca != null)
                {
                    marca.Nombre = txtNombre.Text.Trim();
                    marca.Descripcion = txtDescripcion.Text.Trim();

                    MarcaManager manager = new MarcaManager();
                    manager.Modificar(marca);

                    // Limpiar la sesion y redirigir sin lanzar excepcion
                    Session.Remove("marcaSeleccionada");
                    Response.Redirect("Marcas.aspx", false);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar la marca: " + ex.Message);
            }
        }
    }
}
