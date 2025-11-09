using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clases;
using Manager;

namespace Dominio
{
    public partial class Marcas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) // para que solo cargue la lista la primera vez
            {
                CargarMarcas();
            }
        }

        private void CargarMarcas()
        {
            MarcaManager marcaManager = new MarcaManager();
            var lista = marcaManager.Listar();
            Session["listaMarcas"] = lista; // guardo la lista en sesion
            DGVmarcas.DataSource = lista;
            DGVmarcas.DataBind();
        }

        protected void DGVmarcas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var listaMarcas = (List<Marca>)Session["listaMarcas"];
            long id = Convert.ToInt64(e.CommandArgument); //sirve para obtener el ID del registro sobre el que se hizo clic en el GridView

            if (e.CommandName == "Eliminar") // el CommandName es el de las columnas que estan en el DGV
            {
                MarcaManager manager = new MarcaManager();

                try
                {
                    manager.EliminarLogico(id);
                    CargarMarcas();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al eliminar la marca: " + ex.Message);
                }
            }
            else if (e.CommandName == "Editar")
            {
                // se busca el objeto en la lista
                Marca seleccionada = listaMarcas.Find(x => x.Id == id);

                if (seleccionada != null)
                {
                    // Lo guardo en sesion para la otra pagina
                    Session["marcaSeleccionada"] = seleccionada;
                    Response.Redirect("ModificarMarca.aspx");
                }
            }
        }
    }
}
