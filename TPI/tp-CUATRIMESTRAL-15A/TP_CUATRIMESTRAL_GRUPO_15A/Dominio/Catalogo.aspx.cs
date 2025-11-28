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
                CargarFiltros();
                CargarProductos();
            }
        }

        private void CargarProductos()
        {
            try
            {
                List<Producto> lista = productoManager.ListarConFiltros(
                    categoriaId: ddlCategoria.SelectedValue,
                    marcaId: ddlMarca.SelectedValue,
                    precioDesde: string.IsNullOrEmpty(txtPrecioDesde.Text) ? (decimal?)null : decimal.Parse(txtPrecioDesde.Text),
                    precioHasta: string.IsNullOrEmpty(txtPrecioHasta.Text) ? (decimal?)null : decimal.Parse(txtPrecioHasta.Text)
                );

                // Aplicar ordenamiento
                string orden = ddlOrdenar.SelectedValue;
                if (orden == "precio_desc")
                    lista = lista.OrderByDescending(p => p.Precio).ToList();
                else if (orden == "precio_asc")
                    lista = lista.OrderBy(p => p.Precio).ToList();

                rptProductos.DataSource = lista;
                rptProductos.DataBind();
            }
            catch (Exception ex)
            {
                // Podés mostrar un SweetAlert o Label si querés
                ScriptManager.RegisterStartupScript(this, GetType(), "error",
                    $"alert('Error al cargar productos: {ex.Message}');", true);
            }
        }

        private void CargarFiltros()
        {
            // Categorías
            var categorias = CategoriaManager.Listar();
            ddlCategoria.DataSource = categorias;
            ddlCategoria.DataTextField = "Nombre";
            ddlCategoria.DataValueField = "Id";
            ddlCategoria.DataBind();
            ddlCategoria.Items.Insert(0, new ListItem("Todas las categorías", ""));

            // Marcas
            var marcas = marcaManager.Listar();
            ddlMarca.DataSource = marcas;
            ddlMarca.DataTextField = "Nombre";
            ddlMarca.DataValueField = "Id";
            ddlMarca.DataBind();
            ddlMarca.Items.Insert(0, new ListItem("Todas las marcas", ""));
        }

        // FILTROS DEL MODAL
        protected void btnAplicarFiltro_Click(object sender, EventArgs e)
        {
            CargarProductos();

            // ESTA ES LA FORMA CORRECTA Y QUE NUNCA FALLA
            string script = "<script type='text/javascript'>setTimeout(function(){ $('#filtroModal').modal('hide'); }, 150);</script>";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "cerrarModal", script);
        }

        // ORDENAMIENTO
        protected void ddlOrdenar_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarProductos();
        }

        // VER DETALLE
        protected void btnVerDetalle_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            long id = Convert.ToInt64(btn.CommandArgument);
            Response.Redirect($"DetalleCatalogo.aspx?id={id}");
        }

        // Helper para truncar descripción
        public string Truncate(object texto, int longitud = 60)
        {
            string str = texto?.ToString() ?? "";
            return str.Length > longitud ? str.Substring(0, longitud) + "..." : str;
        }
    }
}