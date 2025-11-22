using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clases;
using Manager;
using System.Linq;

namespace Dominio
{
    public partial class Categorias : AuthenticationPage
    {
        private CategoriaManager manager = new CategoriaManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["PageSize"] = 10;
                CargarCategorias();
            }
            else
            {
                // FILTRO REACTIVO
                if (Request["__EVENTTARGET"] == "filtrarCategorias")
                {
                    string texto = Request["__EVENTARGUMENT"] ?? "";
                    txtBuscar.Text = texto;
                    DGVcategorias.PageIndex = 0;
                    CargarCategorias();
                    return;
                }

                // ELIMINAR
                if (Request["__EVENTTARGET"] == "eliminarCategoria")
                {
                    long id = Convert.ToInt64(Request["__EVENTARGUMENT"]);
                    eliminarCategoria(id);
                }
            }
        }

        private void CargarCategorias()
        {
            try
            {
                string filtro = txtBuscar.Text.Trim();

                // GUARDAR EL PageSize en ViewState para que no se pierda
                if (ViewState["PageSize"] != null)
                {
                    DGVcategorias.PageSize = (int)ViewState["PageSize"];
                }
                else
                {
                    DGVcategorias.PageSize = 10;
                }

                var lista = manager.ListarConFiltro(filtro);

                DGVcategorias.DataSource = lista;
                DGVcategorias.DataBind();
            }
            catch (Exception ex)
            {
                // opcional: mostrar error
            }
        }

        // CAMBIO DE CANTIDAD POR PÁGINA
        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["PageSize"] = int.Parse(ddlPageSize.SelectedValue);
            DGVcategorias.PageIndex = 0;
            CargarCategorias();
        }

        // PAGINACIÓN
        protected void DGVcategorias_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DGVcategorias.PageIndex = e.NewPageIndex;
            CargarCategorias();
        }

        // EDITAR
        protected void DGVcategorias_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            long id = Convert.ToInt64(e.CommandArgument);

            if (e.CommandName == "Editar")
            {
                Response.Redirect($"ModificarCategoria.aspx?id={id}");
            }
        }

        // ELIMINAR
        private void eliminarCategoria(long id)
        {
            try
            {
                manager.EliminarLogico(id);
                CargarCategorias();

                string script = "Swal.fire({title: '¡Eliminada!', text: 'La categoría ha sido eliminada.', icon: 'success', timer: 2000, showConfirmButton: false});";
                ClientScript.RegisterStartupScript(this.GetType(), "eliminarOk", script, true);
            }
            catch (Exception ex)
            {
                string msg = ex.Message.Replace("'", @"\'");
                string script = $"Swal.fire({{title: 'Error', text: 'No se pudo eliminar: {msg}', icon: 'error'}});";
                ClientScript.RegisterStartupScript(this.GetType(), "eliminarError", script, true);
            }
        }
    }
}

