using Clases;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Dominio
{
    public partial class ComprasAdmin : AuthenticationPage
    {
        private readonly VentaManager ventaManager = new VentaManager();
        private readonly EstadoPagoManager estadoPagoManager = new EstadoPagoManager();
        private readonly EstadoPreparacionManager estadoPreparacionManager = new EstadoPreparacionManager();
        private readonly EstadoEnvioManager estadoEnvioManager = new EstadoEnvioManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ConfigurarPaginacion();
                CargarPedidos();
                CargarDropDowns();
                divComprobante.Visible = false; // Oculta el preview al inicio
            }
        }

        private void ConfigurarPaginacion()
        {
            if (ViewState["PageSize"] != null)
                gvPedidos.PageSize = (int)ViewState["PageSize"];
            else
                gvPedidos.PageSize = 10;

            ddlPageSize.SelectedValue = gvPedidos.PageSize.ToString();
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvPedidos.PageIndex = 0;
            ViewState["PageSize"] = int.Parse(ddlPageSize.SelectedValue);
            gvPedidos.PageSize = (int)ViewState["PageSize"];
            CargarPedidos();
        }

        protected void gvPedidos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPedidos.PageIndex = e.NewPageIndex;
            CargarPedidos();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            gvPedidos.PageIndex = 0;
            CargarPedidos();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtFiltroId.Text = "";
            txtFechaDesde.Text = "";
            txtFechaHasta.Text = "";
            gvPedidos.PageIndex = 0;
            CargarPedidos();
        }

        private void CargarPedidos()
        {
            var lista = ventaManager.ListarTodasConDetalleYEstados();

            // FILTRO POR ID
            if (!string.IsNullOrEmpty(txtFiltroId.Text.Trim()))
            {
                if (long.TryParse(txtFiltroId.Text.Trim(), out long idFiltro))
                {
                    lista = lista.Where(v => v.Id == idFiltro).ToList();
                }
            }

            // FILTRO POR FECHAS
            if (!string.IsNullOrEmpty(txtFechaDesde.Text))
            {
                if (DateTime.TryParseExact(txtFechaDesde.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime desde))
                {
                    lista = lista.Where(v => v.FechaVenta.Date >= desde.Date).ToList();
                }
            }

            if (!string.IsNullOrEmpty(txtFechaHasta.Text))
            {
                if (DateTime.TryParseExact(txtFechaHasta.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime hasta))
                {
                    lista = lista.Where(v => v.FechaVenta.Date <= hasta.Date).ToList();
                }
            }

            // ORDEN POR ID DESCENDENTE (MÁS NUEVO ARRIBA)
            lista = lista.OrderByDescending(v => v.Id).ToList();

            // PAGINACIÓN BUILT-IN DEL GRIDVIEW
            gvPedidos.DataSource = lista;
            gvPedidos.DataBind();

            // ACTUALIZAR CONTADORES
            lblTotal.Text = lista.Count.ToString();
            lblPaginaActual.Text = (gvPedidos.PageIndex + 1).ToString();
            lblTotalPaginas.Text = gvPedidos.PageCount.ToString();
            divPaginacion.Visible = gvPedidos.PageCount > 1;
        }

        private void CargarDropDowns()
        {
            CargarDDL(ddlEstadoPago, estadoPagoManager.Listar());
            CargarDDL(ddlEstadoPreparacion, estadoPreparacionManager.Listar());
            CargarDDL(ddlEstadoEnvio, estadoEnvioManager.Listar());
        }

        private void CargarDDL<T>(DropDownList ddl, List<T> lista)
        {
            ddl.DataValueField = "Id";
            ddl.DataTextField = "Nombre";
            ddl.DataSource = lista;
            ddl.DataBind();
        }

        // -------------------------------------------------------
        // Mostrar Comprobante
        // -------------------------------------------------------
        private void MostrarComprobante(long idVenta)
        {
            string ruta = ventaManager.ObtenerComprobantePorId(idVenta);

            if (!string.IsNullOrEmpty(ruta))
            {
                imgComprobante.ImageUrl = ruta;
                lnkDetalle.NavigateUrl = ruta;
                divComprobante.Visible = true;
            }
            else
            {
                divComprobante.Visible = false;
                ScriptManager.RegisterStartupScript(this, GetType(), "noimg",
                    "Swal.fire('Sin comprobante', 'Este pedido no tiene comprobante cargado', 'warning');", true);
            }
        }

        protected void btnGuardarEstados_Click(object sender, EventArgs e)
        {
            // REGISTRAMOS LA TAREA ASÍNCRONA (esto es magia de WebForms)
            Page.RegisterAsyncTask(new PageAsyncTask(ActualizarEstadosAsync));
        }

        private async System.Threading.Tasks.Task ActualizarEstadosAsync()
        {
            try
            {
                // === TODA LA LÓGICA DENTRO DE UN TASK ===
                if (!long.TryParse(hfIdVenta.Value, out long idVenta) || idVenta <= 0)
                    throw new Exception("ID de venta inválido");

                if (!int.TryParse(ddlEstadoPago.SelectedValue, out int pago) || pago <= 0)
                    throw new Exception("Selecciona estado de pago");

                if (!int.TryParse(ddlEstadoPreparacion.SelectedValue, out int prep) || prep <= 0)
                    throw new Exception("Selecciona estado de preparación");

                if (!int.TryParse(ddlEstadoEnvio.SelectedValue, out int envio) || envio <= 0)
                    throw new Exception("Selecciona estado de envío");

                var venta = ventaManager.ObtenerVentaCompletaConCliente(idVenta);
                if (venta == null || string.IsNullOrWhiteSpace(venta.Cliente?.Usuario?.Email))
                    throw new Exception("No se encontró el email del cliente");

                string emailCliente = venta.Cliente.Usuario.Email;
                string nombreCliente = $"{venta.Cliente.Nombre} {venta.Cliente.Apellido}".Trim();

                int pagoAnterior = venta.EstadoPago?.Id ?? 0;
                int prepAnterior = venta.EstadoPreparacion?.Id ?? 0;
                int envioAnterior = venta.EstadoEnvio?.Id ?? 0;

                var emailManager = new EmailManager();

                // ENVIAR EMAILS (await funciona perfecto aquí)
                if (pago != pagoAnterior)
                {
                    if (pago == 2)
                        await emailManager.EnviarMailCambioEstadoPago(emailCliente, nombreCliente, idVenta, "Aprobado");
                    else if (pago == 3)
                        await emailManager.EnviarMailCambioEstadoPago(emailCliente, nombreCliente, idVenta, "Rechazado");
                    else if (pago == 4)
                        await emailManager.EnviarMailCambioEstadoPago(emailCliente, nombreCliente, idVenta, "Pendiente de comprobante");
                }

                if (prep != prepAnterior)
                {
                    if (prep == 2)
                    await emailManager.EnviarMailCambioPreparacion(emailCliente, nombreCliente, idVenta, "En preparación");
                    else if (prep == 3)
                        await emailManager.EnviarMailCambioPreparacion(emailCliente, nombreCliente, idVenta, "Listo para envío");
                    else if (prep == 4)
                        await emailManager.EnviarMailCambioPreparacion(emailCliente, nombreCliente, idVenta, "Rechazado");
                }

                if (envio != envioAnterior)
                {
                    if (envio == 2)
                        await emailManager.EnviarMailCambioEnvio(emailCliente, nombreCliente, idVenta, "En camino");
                    else if (envio == 3)
                        await emailManager.EnviarMailCambioEnvio(emailCliente, nombreCliente, idVenta, "Entregado");
                    else if (envio == 4)
                        await emailManager.EnviarMailCambioEnvio(emailCliente, nombreCliente, idVenta, "Devuelto");
                    else if (envio == 5)
                        await emailManager.EnviarMailCambioEnvio(emailCliente, nombreCliente, idVenta, "Cancelado");
                }

                // ACTUALIZAR BD
                ventaManager.CambiarEstadoPago(idVenta, pago);
                ventaManager.CambiarEstadoPreparacion(idVenta, prep);
                ventaManager.CambiarEstadoEnvio(idVenta, envio);

                // RECARGAR GRILLA
                CargarPedidos();

                // MENSAJE + CERRAR MODAL
                MostrarExitoYcerrarModal("Estados actualizados y cliente notificado");
            }
            catch (Exception ex)
            {
                MostrarError("Error: " + ex.Message);
            }
        }

        private void MostrarError(string mensaje)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "error",
                $"Swal.fire({{icon: 'error', title: 'Error', text: '{mensaje.Replace("'", "\\'")}'}});", true);
        }

        private void MostrarExitoYcerrarModal(string mensaje = "¡Estados actualizados correctamente!")
        {
            string script = $@"
        <script>
            Swal.fire({{
                icon: 'success',
                title: '¡Perfecto!',
                text: '{mensaje}',
                timer: 2000,
                showConfirmButton: false
            }}).then(() => {{
                $('#modalSeguimiento').modal('hide'); // Cierra el modal
            }});
        </script>";

            ScriptManager.RegisterStartupScript(this, GetType(), "exitoCerrar", script, false);
        }

        // -------------------------------------------------------
        // para el Boton Ver Comprobante
        // -------------------------------------------------------
        protected void gvPedidos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            long idVenta = Convert.ToInt64(e.CommandArgument);

            if (e.CommandName == "VerComprobante")
            {
                MostrarComprobante(idVenta);
            }
        }

        protected void btnCerrarComprobante_Click(object sender, EventArgs e)
        {
            divComprobante.Visible = false;
            imgComprobante.ImageUrl = "";
            lnkDetalle.NavigateUrl = "";
        }

    }
}

