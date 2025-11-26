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
    public partial class DetalleCatalogo : System.Web.UI.Page
    {
        private readonly ProductoManager manager = new ProductoManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (long.TryParse(Request.QueryString["id"], out long id))
                {
                    Producto producto = manager.BuscarPorId(id);

                    if (producto == null || !producto.Estado)
                    {
                        Response.Redirect("Catalogo.aspx");
                        return;
                    }

                    // === CARGAR IMÁGENES EN LOS REPEATERS ===
                    var imagenes = producto.Imagenes ?? new List<ProductoImagen>();

                    // Si no tiene imágenes → placeholder
                    if (imagenes.Count == 0)
                    {
                        imagenes.Add(new ProductoImagen
                        {
                            UrlImagen = "https://via.placeholder.com/600x600/cccccc/666666?text=Sin+Imagen"
                        });
                    }

                    // BUSCAR LOS REPEATERS DENTRO DEL FORMVIEW
                    Repeater rptImagenes = (Repeater)fvProducto.FindControl("rptImagenes");
                    Repeater rptMiniaturas = (Repeater)fvProducto.FindControl("rptMiniaturas");

                    if (rptImagenes != null)
                    {
                        rptImagenes.DataSource = imagenes.Select(img => new
                        {
                            UrlImagen = ResolveUrl(img.UrlImagen) // ← IMPORTANTE: ResolveUrl para la ~
                        });
                        rptImagenes.DataBind();
                    }

                    if (rptMiniaturas != null)
                    {
                        rptMiniaturas.DataSource = imagenes.Select(img => new
                        {
                            UrlImagen = ResolveUrl(img.UrlImagen)
                        });
                        rptMiniaturas.DataBind();
                    }

                    // CARGAR EL PRODUCTO EN EL FORMVIEW
                    fvProducto.DataSource = new[] { producto };
                    fvProducto.DataBind();
                }
                else
                {
                    Response.Redirect("Catalogo.aspx");
                }
            }
        }

        private void CargarProducto()
        {
            try
            {
                if (!long.TryParse(Request.QueryString["id"], out long id))
                {
                    Response.Redirect("Catalogo.aspx");
                    return;
                }

                Producto producto = manager.BuscarPorId(id);

                if (producto == null || !producto.Estado)
                {
                    Response.Redirect("Catalogo.aspx");
                    return;
                }

                // ASIGNAR AL FORMVIEW
                fvProducto.DataSource = new List<Producto> { producto };
                fvProducto.DataBind();
            }
            catch
            {
                Response.Redirect("Catalogo.aspx");
            }
        }

        protected void btnAgregarCarrito_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            long id = Convert.ToInt64(btn.CommandArgument);
            Producto producto = manager.BuscarPorId(id);

            if (producto == null || producto.Stock <= 0)
                return;

            if (Session["Carrito"] == null)
                Session["Carrito"] = new List<ProductoCarrito>();

            var carrito = (List<ProductoCarrito>)Session["Carrito"];

            var existente = carrito.FirstOrDefault(x => x.IdProducto == id);

            if (existente == null)
            {
                carrito.Add(new ProductoCarrito
                {
                    IdProducto = producto.Id,
                    Nombre = producto.Nombre,
                    Precio = producto.Precio,
                    Cantidad = 1
                });
            }
            else
            {
                existente.Cantidad++;
            }

            Session["Carrito"] = carrito;

            MostrarExito($"'{producto.Nombre}' agregado al carrito");
        }



        private void MostrarExito(string msg)
        {
            string script = $@"Swal.fire({{icon:'success', title:'¡Listo!', text:'{msg}', timer:1500, showConfirmButton:false}})";
            ClientScript.RegisterStartupScript(this.GetType(), "exito", script, true);
        }
    }
}