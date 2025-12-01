using Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dominio
{
    public partial class MasterPageCliente : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] != null)
            {
                phLogueado.Visible = true;
                phNoLogueado.Visible = false;
                ActualizarBadgeCarrito();
            }
            else
            {
                phLogueado.Visible = false;
                phNoLogueado.Visible = true;
            }
        }

        private void ActualizarBadgeCarrito()
        {
            var carrito = Session["Carrito"] as List<ProductoCarrito>;
            int total = carrito?.Sum(x => x.Cantidad) ?? 0;

            string script = $@"
        <script>
            document.addEventListener('DOMContentLoaded', function() {{
                const badge = document.getElementById('badgeCarrito');
                if ({total} > 0) {{
                    badge.textContent = {total};
                    badge.style.display = 'block';
                }}
            }});
        </script>";

            Page.ClientScript.RegisterStartupScript(this.GetType(), "initCarrito", script, false);
        }
    }
}