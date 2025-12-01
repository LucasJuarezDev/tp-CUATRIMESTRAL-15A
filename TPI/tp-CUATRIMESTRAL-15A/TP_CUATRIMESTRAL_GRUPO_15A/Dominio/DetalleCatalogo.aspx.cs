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
            int totalItems = carrito.Sum(x => x.Cantidad);

            // === SCRIPT MÁGICO QUE HACE TODO ===
            string script = @"
        <script>
            // Actualizar badge del header
            const badge = document.getElementById('badgeCarrito');
            const totalItems = " + carrito.Sum(x => x.Cantidad) + @";
            badge.textContent = totalItems;
            badge.style.display = totalItems > 0 ? 'block' : 'none';

            // Mostrar popup redondo
            const popup = document.getElementById('popupCarrito');
            const popupCant = document.getElementById('popupCantidad');
            popupCant.textContent = totalItems;

            popup.style.opacity = '0';
            popup.style.display = 'flex';
            
            // Forzar reflow para que la animación funcione
            void popup.offsetWidth;
            
            popup.style.animation = 'popup 0.6s ease-out forwards, fadeout 0.6s 2.4s forwards';
        </script>";

            ClientScript.RegisterStartupScript(this.GetType(), "carritoUpdate", script, false);

            // Mensajito lindo con SweetAlert (opcional, queda pro)
            MostrarExito($"'{producto.Nombre}' agregado al carrito");
        }



        private void MostrarExito(string msg)
        {
            string script = $@"Swal.fire({{icon:'success', title:'¡Listo!', text:'{msg}', timer:1500, showConfirmButton:false}})";
            ClientScript.RegisterStartupScript(this.GetType(), "carritoUpdate_" + DateTime.Now.Ticks, script, false);
        }
    }
}