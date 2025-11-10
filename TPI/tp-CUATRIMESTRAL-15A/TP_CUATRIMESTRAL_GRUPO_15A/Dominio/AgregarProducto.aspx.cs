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
    public partial class AgregarProducto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarCombos();
            }
        }

        private void CargarCombos()
        {
            try
            {
                var marcaManager = new MarcaManager();
                var categoriaManager = new CategoriaManager();

                ddlMarca.DataSource = marcaManager.Listar();
                ddlMarca.DataTextField = "Nombre";
                ddlMarca.DataValueField = "Id";
                ddlMarca.DataBind();
                ddlMarca.Items.Insert(0, new ListItem("-- Seleccionar marca --", ""));

                ddlCategoria.DataSource = categoriaManager.Listar();
                ddlCategoria.DataTextField = "Nombre";
                ddlCategoria.DataValueField = "Id";
                ddlCategoria.DataBind();
                ddlCategoria.Items.Insert(0, new ListItem("-- Seleccionar categoría --", ""));
            }
            catch (Exception ex)
            {
                MostrarError("Error al cargar datos: " + ex.Message);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                var producto = new Producto
                {
                    Nombre = txtNombre.Text.Trim(),
                    Precio = decimal.Parse(txtPrecio.Text),
                    DescripcionCorta = txtDescripcionCorta.Text.Trim(),
                    DescripcionExtendida = txtDescripcionExtendida.Text.Trim(),
                    ImagenUrl = string.IsNullOrWhiteSpace(txtImagenUrl.Text) ? null : txtImagenUrl.Text.Trim(),
                    Stock = int.Parse(txtStock.Text),
                    StockMinimo = int.Parse(txtStockMinimo.Text),
                    Estado = true,
                    Marca = new Marca { Id = long.Parse(ddlMarca.SelectedValue) },
                    Categoria = new Categoria { Id = long.Parse(ddlCategoria.SelectedValue) }
                };

                var manager = new ProductoManager();
                manager.nuevoProducto(producto);

                // Mostrar popup y redirigir
                string script = "mostrarExito();";
                ClientScript.RegisterStartupScript(this.GetType(), "exito", script, true);
            }
            catch (Exception ex)
            {
                MostrarError("Error al guardar: " + ex.Message);
            }
        }

        private void MostrarError(string mensaje)
        {
            pnlMensaje.CssClass = "alert alert-danger";
            lblMensaje.Text = $"<i class='bi bi-exclamation-triangle'></i> {mensaje}";
            pnlMensaje.Visible = true;
        }
    }
}