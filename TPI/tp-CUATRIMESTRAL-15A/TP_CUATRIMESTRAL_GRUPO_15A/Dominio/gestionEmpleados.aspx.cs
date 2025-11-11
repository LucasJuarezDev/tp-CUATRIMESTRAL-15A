using Manager;
using Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dominio
{
    public partial class gestionEmpleados : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarEmpleados();
            }
        }

        private void CargarEmpleados()
        {
            try
            {
                EmpleadoManager empleadoManager = new EmpleadoManager();
                var lista = empleadoManager.Listar(); 

                // Filtro por búsqueda
                if (!string.IsNullOrEmpty(txtBuscar.Text.Trim()))
                {
                    string busqueda = txtBuscar.Text.Trim().ToLower();
                    lista = lista.Where(e =>
                        e.Nombre.ToLower().Contains(busqueda) ||
                        e.Apellido.ToLower().Contains(busqueda) ||
                        e.Telefono.Contains(busqueda)).ToList();
                }

                // Paginación
                gvEmpleados.PageSize = int.Parse(ddlPageSize.SelectedValue);
                gvEmpleados.DataSource = lista;
                gvEmpleados.DataBind();

                // Info
                int inicio = (gvEmpleados.PageIndex * gvEmpleados.PageSize) + 1;
                int fin = Math.Min(inicio + gvEmpleados.PageSize - 1, lista.Count);
                lblInfo.Text = $"Mostrando {inicio} - {fin} de {lista.Count}";
                lblRegistrosMostrados.Text = $"{inicio}-{fin}";
                lblTotalRegistros.Text = lista.Count.ToString();
            }
            catch (Exception ex)
            {
                // Mostrar error
            }
        }

        protected void gvEmpleados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvEmpleados.PageIndex = e.NewPageIndex;
            CargarEmpleados();
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvEmpleados.PageIndex = 0;
            CargarEmpleados();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            gvEmpleados.PageIndex = 0;
            CargarEmpleados();
        }

        protected void gvEmpleados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                long id = Convert.ToInt64(e.CommandArgument);
                Response.Redirect($"alterarEmpleado.aspx?id={id}");
            }
        }
    }
}