using Clases;
using Manager;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dominio
{
    public partial class AgregarProducto : AuthenticationPage
    {
        private ProductoManager manager = new ProductoManager();
        private long? ProductoId => long.TryParse(Request.QueryString["id"], out long id) ? id : (long?)null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarCombos();
                if (ProductoId.HasValue)
                {
                    ModoEdicion();
                    CargarProducto(ProductoId.Value);
                }
                else
                {
                    ModoAgregar();
                }
            }
        }

        private void ModoAgregar()
        {
            lblTitulo.Text = "Agregar Nuevo Producto";
            btnGuardar.Text = "Guardar Producto";
            iconAgregar.Visible = true;
            iconModificar.Visible = false;
        }

        private void ModoEdicion()
        {
            lblTitulo.Text = "Modificar Producto";
            btnGuardar.Text = "Modificar";
            iconAgregar.Visible = false;
            iconModificar.Visible = true;
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

        private void CargarProducto(long id)
        {
            try
            {
                var prod = manager.BuscarPorId(id);
                if (prod == null)
                {
                    MostrarError("Producto no encontrado.");
                    Response.Redirect("Productos.aspx");
                    return;
                }
                
                txtNombre.Text = prod.Nombre;
                string precioSinMiles = prod.Precio.ToString("0.##", CultureInfo.GetCultureInfo("es-AR")).Replace(".", ""); 
                txtPrecio.Text = precioSinMiles; 
                txtDescripcionCorta.Text = prod.DescripcionCorta;
                txtDescripcionExtendida.Text = prod.DescripcionExtendida;
                txtStock.Text = prod.Stock.ToString();
                txtStockMinimo.Text = prod.StockMinimo.ToString();
                ddlMarca.SelectedValue = prod.Marca.Id.ToString();
                ddlCategoria.SelectedValue = prod.Categoria.Id.ToString();
            }
            catch (Exception ex)
            {
                MostrarError("Error al cargar producto: " + ex.Message);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                // === CREAR O MODIFICAR PRODUCTO ===
                var producto = new Producto
                {
                    Nombre = txtNombre.Text.Trim(),
                    Precio = decimal.Parse(txtPrecio.Text.Trim().Replace(".", ",")),
                    DescripcionCorta = txtDescripcionCorta.Text.Trim(),
                    DescripcionExtendida = txtDescripcionExtendida.Text.Trim(),
                    Stock = int.Parse(txtStock.Text),
                    StockMinimo = int.Parse(txtStockMinimo.Text),
                    Marca = new Marca { Id = long.Parse(ddlMarca.SelectedValue) },
                    Categoria = new Categoria { Id = long.Parse(ddlCategoria.SelectedValue) },
                    Estado = true
                };

                long idProducto;

                if (Request.QueryString["id"] != null)
                {
                    producto.Id = long.Parse(Request.QueryString["id"]);
                    manager.Modificar(producto);
                    idProducto = producto.Id;
                }
                else
                {
                    idProducto = manager.nuevoProducto(producto);
                }

                List<string> rutasGuardadas = new List<string>(); // ← ESTA LÍNEA FALTABA

                if (fuImagenes.HasFiles)
                {

                    string carpetaFisica = Server.MapPath("~/img/productos/");

                    if (!Directory.Exists(carpetaFisica))
                    {
                        Directory.CreateDirectory(carpetaFisica);
                    }

                    int contador = 0;
                    foreach (HttpPostedFile file in fuImagenes.PostedFiles)
                    {
                        contador++;

                        string extension = Path.GetExtension(file.FileName).ToLower();
                        if (!new[] { ".jpg", ".jpeg", ".png", ".webp" }.Contains(extension))
                        {
                            continue;
                        }

                        string nombreArchivo = $"{idProducto}_{DateTime.Now:yyyyMMddHHmmssfff}_{contador}{extension}";
                        string rutaCompleta = Path.Combine(carpetaFisica, nombreArchivo);

                        try
                        {
                            file.SaveAs(rutaCompleta);

                            if (File.Exists(rutaCompleta))
                            {
                                rutasGuardadas.Add("~/img/productos/" + nombreArchivo);
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"EXCEPCIÓN: {ex.Message}");
                        }
                    }
                }

                // === GUARDAR RUTAS EN BD ===
                if (rutasGuardadas.Count > 0)
                {
                    manager.AgregarImagenes(idProducto, rutasGuardadas);
                }

                Response.Redirect("Productos.aspx?exito=1");
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error: " + ex.Message;
                pnlMensaje.CssClass = "alert alert-danger";
                pnlMensaje.Visible = true;
            }
        }

        private void MostrarError(string mensaje)
        {
            pnlMensaje.CssClass = "alert alert-danger";
            lblMensaje.Text = mensaje;
            pnlMensaje.Visible = true;
        }

        private void MostrarExitoJS(string mensaje)
        {
            string script = $"mostrarExito('{mensaje}');";
            ClientScript.RegisterStartupScript(this.GetType(), "exito", script, true);
        }
    }
}