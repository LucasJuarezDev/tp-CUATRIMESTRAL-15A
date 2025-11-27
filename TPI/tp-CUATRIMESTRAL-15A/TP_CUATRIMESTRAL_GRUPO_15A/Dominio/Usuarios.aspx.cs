using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clases;
using Manager;

namespace Dominio
{
    public partial class Usuarios : AuthenticationPage
    {
        private readonly UsuarioManager manager = new UsuarioManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            // PROTECCIÓN DE ACCESO (Solo Admin)
            var usuarioLogueado = Session["usuario"] as UsuarioLogueado;
            if (usuarioLogueado?.Rol?.Id != 1)
            {
                Response.Redirect(usuarioLogueado?.Rol?.Id == 2 ? "Productos.aspx" : "Catalogo.aspx");
            }

            if (!IsPostBack)
            {
                ddlPageSize.SelectedValue = "10"; // valor inicial
                gvUsuarios.PageSize = 10;
                AplicarFiltroYPagina();
            }
            else
            {
                // Manejo de eliminación por __doPostBack
                string eventTarget = Request["__EVENTTARGET"];
                string eventArgument = Request["__EVENTARGUMENT"];
                if (eventTarget == "EliminarUsuario" && long.TryParse(eventArgument, out long idEliminar))
                {
                    EliminarUsuario(idEliminar);
                }
            }
        }

        private void AplicarFiltroYPagina()
        {
            var listaCompleta = manager.Listar(); // ← tu método viejo que devuelve TODOS

            string filtro = txtBuscar.Text.Trim().ToLower();

            var listaFiltrada = string.IsNullOrEmpty(filtro)
                ? listaCompleta
                : listaCompleta.Where(u =>
                    u.Nickname.ToLower().Contains(filtro) ||
                    (!string.IsNullOrEmpty(u.Email) && u.Email.ToLower().Contains(filtro)) ||
                    u.Rol.Nombre.ToLower().Contains(filtro)
                  ).ToList();

            // ← CLAVE: ACTUALIZAR PageSize ANTES del DataBind
            gvUsuarios.PageSize = int.Parse(ddlPageSize.SelectedValue);

            gvUsuarios.DataSource = listaFiltrada;  // ← TODA la lista filtrada
            gvUsuarios.DataBind();                  // ← GridView pagina SOLO

            int totalPaginas = (int)Math.Ceiling((double)listaFiltrada.Count / gvUsuarios.PageSize);
            lblInfo.Text = $"Página {gvUsuarios.PageIndex + 1} de {totalPaginas} — Mostrando {listaFiltrada.Count} usuarios";
        }

        protected void gvUsuarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUsuarios.PageIndex = e.NewPageIndex;
            AplicarFiltroYPagina();  // ← vuelve a cargar con la nueva página
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvUsuarios.PageIndex = 0;  // volver a página 1
            AplicarFiltroYPagina();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            gvUsuarios.PageIndex = 0;
            AplicarFiltroYPagina();
        }

        protected void gvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                long id = Convert.ToInt64(e.CommandArgument);
                Response.Redirect($"RegistrarUsuario.aspx?id={id}");
            }
        }

        private void EliminarUsuario(long id)
        {
            try
            {
                manager.Eliminar(id);
                AplicarFiltroYPagina(); // Volver a aplicar filtro y página

                string script = @"
                    Swal.fire({
                        icon: 'success',
                        title: '¡Eliminado!',
                        text: 'El usuario ha sido eliminado correctamente.',
                        timer: 2000,
                        showConfirmButton: false
                    });";
                ClientScript.RegisterStartupScript(this.GetType(), "eliminarOk", script, true);
            }
            catch (Exception ex)
            {
                string msg = ex.Message.Replace("'", @"\'");
                string script = $@"
                    Swal.fire({{
                        icon: 'error',
                        title: 'Error',
                        text: 'No se pudo eliminar: {msg}'
                    }});";
                ClientScript.RegisterStartupScript(this.GetType(), "eliminarError", script, true);
            }
        }
    }
}