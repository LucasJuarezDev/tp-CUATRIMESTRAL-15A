using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clases;
using Manager;

namespace Dominio
{
    public partial class Marcas : AuthenticationPage  
    {
        private MarcaManager manager = new MarcaManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarMarcas();
            }
        }

        private void CargarMarcas()
        {
            try
            {
                string filtro = txtBuscar.Text.Trim();
                int pageSize = int.Parse(ddlPageSize.SelectedValue);

                var lista = manager.ListarConFiltro(filtro);

                DGVmarcas.PageSize = pageSize;
                DGVmarcas.DataSource = lista;
                DGVmarcas.DataBind();
            }
            catch (Exception ex)
            {
                // opcional: mostrar error
            }
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            DGVmarcas.PageIndex = 0;
            CargarMarcas();
        }

        protected void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            DGVmarcas.PageIndex = 0;
            CargarMarcas();
        }

        protected void DGVmarcas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DGVmarcas.PageIndex = e.NewPageIndex;
            CargarMarcas();
        }

        protected void DGVmarcas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            long id = Convert.ToInt64(e.CommandArgument);

            if (e.CommandName == "Editar")
            {
                Response.Redirect($"ModificarMarca.aspx?id={id}");
            }
            else if (e.CommandName == "Eliminar")
            {
                try
                {
                    manager.EliminarLogico(id);
                    CargarMarcas();

                    string script = "Swal.fire('¡Eliminada!', 'La marca ha sido eliminada.', 'success');";
                    ClientScript.RegisterStartupScript(this.GetType(), "eliminarOk", script, true);
                }
                catch (Exception ex)
                {
                    string script = $"Swal.fire('Error', 'No se pudo eliminar: {ex.Message.Replace("'", "\\'")}', 'error');";
                    ClientScript.RegisterStartupScript(this.GetType(), "eliminarError", script, true);
                }
            }
        }
    }
}
