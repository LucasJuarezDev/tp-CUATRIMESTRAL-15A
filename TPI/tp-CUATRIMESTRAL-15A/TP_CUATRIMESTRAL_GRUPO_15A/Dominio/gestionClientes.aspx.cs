using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clases;
using Manager;

namespace Dominio
{
    public partial class gestionClientes : AuthenticationPage
    {
        private readonly ClienteManager clienteManager = new ClienteManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            var usuarioLogueado = Session["usuario"] as UsuarioLogueado;
            if (usuarioLogueado?.Rol?.Id != 1)
            {
                if (usuarioLogueado?.Rol?.Id == 2)
                {
                    Response.Redirect("Productos.aspx");
                }
                else
                {
                    Response.Redirect("Catalogo.aspx");
                }
            }
            if (!IsPostBack)
            {
                CargarClientes();
            }
        }

        private void CargarClientes(string filtro = "")
        {
            List<Cliente> lista = clienteManager.Listar(filtro);

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                string f = filtro.ToLower();
                lista = lista.Where(c =>
                    (c.Nombre?.ToLower().Contains(f) ?? false) ||
                    (c.Apellido?.ToLower().Contains(f) ?? false) ||
                    (c.Telefono?.Contains(f) ?? false) ||
                    (c.Usuario?.Nickname?.ToLower().Contains(f) ?? false) ||
                    (c.Usuario?.Email?.ToLower().Contains(f) ?? false) ||
                    (c.RazonSocial?.ToLower().Contains(f) ?? false)
                ).ToList();
            }

            gvClientes.DataSource = lista;
            gvClientes.DataBind();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarClientes(txtBuscar.Text.Trim());
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvClientes.PageSize = int.Parse(ddlPageSize.SelectedValue);
            CargarClientes(txtBuscar.Text.Trim());
        }

        protected void gvClientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvClientes.PageIndex = e.NewPageIndex;
            CargarClientes(txtBuscar.Text.Trim());
        }

        protected void gvClientes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            long id = Convert.ToInt64(e.CommandArgument);

            switch (e.CommandName)
            {
                case "Ver":
                    Response.Redirect($"DetalleCliente.aspx?id={id}");
                    break;
                case "Editar":
                    Response.Redirect($"ModificarCliente.aspx?id={id}");
                    break;
                case "ToggleEstado":
                    clienteManager.CambiarEstado(id);
                    CargarClientes(txtBuscar.Text.Trim());
                    break;
            }
        }
    }
}