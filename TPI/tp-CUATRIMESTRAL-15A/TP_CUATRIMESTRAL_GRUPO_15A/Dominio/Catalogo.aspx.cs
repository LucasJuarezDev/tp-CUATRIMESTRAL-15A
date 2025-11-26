using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clases;
using Manager;

namespace Dominio
{
    public partial class Catalogo : System.Web.UI.Page
    {
        private ProductoManager productoManager = new ProductoManager();
        private CategoriaManager CategoriaManager = new CategoriaManager();
        private MarcaManager marcaManager = new MarcaManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarProductos();
                CargarFiltros();
            }
        }

        private void CargarProductos()
        {
            try
            {
                List<Producto> lista = productoManager.Listar(1);
                AplicarOrdenamiento(ref lista);
                rptProductos.DataSource = lista;
                rptProductos.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void CargarFiltros()
        {
            // Cargar marcas y categorías desde DB
            ddlCategoria.DataSource = CategoriaManager.Listar();
            ddlCategoria.DataTextField = "Nombre";
            ddlCategoria.DataValueField = "Id";
            ddlCategoria.DataBind();
            ddlCategoria.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Todas las categorías", ""));

            ddlMarca.DataSource = marcaManager.Listar();
            ddlMarca.DataTextField = "Nombre";
            ddlMarca.DataValueField = "Id";
            ddlMarca.DataBind();
            ddlMarca.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Todas las marcas", ""));
        }

        private void AplicarOrdenamiento(ref List<Producto> lista)
        {
            string orden = ddlOrdenar.SelectedValue;
            if (orden == "precio_desc")
                lista.Sort((x, y) => y.Precio.CompareTo(x.Precio));
            else if (orden == "precio_asc")
                lista.Sort((x, y) => x.Precio.CompareTo(y.Precio));
        }

        protected void btnVerDetalle_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            long id = Convert.ToInt64(btn.CommandArgument);
            Response.Redirect($"DetalleCatalogo.aspx?id={id}");
        }

        // Helper para truncar descripcion
        public string Truncate(object texto, int longitud)
        {
            string str = texto?.ToString() ?? "";
            return str.Length > longitud ? str.Substring(0, longitud) + "..." : str;
        }
    }
}