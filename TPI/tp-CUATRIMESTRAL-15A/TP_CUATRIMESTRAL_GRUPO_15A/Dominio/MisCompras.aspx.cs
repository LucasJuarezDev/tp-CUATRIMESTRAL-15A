using Clases;
using Manager;
using System;
using System.Web.UI;

namespace Dominio
{
    public partial class MisCompras : AuthenticationPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CargarCompras();
        }

        private void CargarCompras()
        {
            if (Session["cliente"] == null)
            {
                Response.Redirect("LoginCliente.aspx");
                return;
            }

            Cliente cli = (Cliente)Session["cliente"];
            VentaManager manager = new VentaManager();
            gvCompras.DataSource = manager.ObtenerVentasPorCliente(cli.Id);
            gvCompras.DataBind();
        }

        // COLORES - COMPATIBLE CON C# 7.3
        protected string GetBadgeClassPago(object idObj)
        {
            int id = Convert.ToInt32(idObj);
            switch (id)
            {
                case 1: return "bg-warning text-dark"; // Pendiente
                case 2: return "bg-success";           // Aprobado
                case 3: return "bg-danger";            // Rechazado
                case 4: return "bg-secondary";         // Pendiente comprobante
                default: return "bg-secondary";
            }
        }

        protected string GetBadgeClassPreparacion(object idObj)
        {
            int id = Convert.ToInt32(idObj);
            switch (id)
            {
                case 1: return "bg-secondary"; // No iniciado
                case 2: return "bg-info text-dark"; // En preparación
                case 3: return "bg-primary"; // Listo para envío
                case 4: return "bg-danger"; // Cancelado
                default: return "bg-secondary";
            }
        }

        protected string GetBadgeClassEnvio(object idObj)
        {
            int id = Convert.ToInt32(idObj);
            switch (id)
            {
                case 1: return "bg-secondary"; // No iniciado
                case 2: return "bg-warning text-dark"; // En camino
                case 3: return "bg-success"; // Entregado
                case 4: return "bg-danger"; // Devuelto
                case 5: return "bg-danger"; // Cancelado
                default: return "bg-secondary";
            }
        }
    }
}

