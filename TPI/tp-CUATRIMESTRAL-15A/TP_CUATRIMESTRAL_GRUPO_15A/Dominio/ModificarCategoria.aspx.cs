using System;
using System.Web.UI;
using Clases;
using Manager;

namespace Dominio
{
    public partial class ModificarCategoria : AuthenticationPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Categoria categoria = Session["categoriaSeleccionada"] as Categoria;

                if (categoria != null)
                {
                    txtNombre.Text = categoria.Nombre;
                    txtDescripcion.Text = categoria.Descripcion;
                }
                else
                {
                    // Si no hay una categoria seleccionada, vuelve a la lista
                    Response.Redirect("Categorias.aspx");
                }
            }
        }

        protected void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            try
            {
                Categoria categoria = Session["categoriaSeleccionada"] as Categoria;

                if (categoria != null)
                {
                    categoria.Nombre = txtNombre.Text.Trim();
                    categoria.Descripcion = txtDescripcion.Text.Trim();

                    CategoriaManager manager = new CategoriaManager();
                    manager.Modificar(categoria);

                    // Limpia la sesion y vuelve a la lista
                    Session.Remove("categoriaSeleccionada");
                    Response.Redirect("Categorias.aspx", false);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar la categoría: " + ex.Message);
            }
        }
    }
}
