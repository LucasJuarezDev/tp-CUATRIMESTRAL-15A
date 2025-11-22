<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageAdmin.Master" AutoEventWireup="true" CodeBehind="Productos.aspx.cs" Inherits="Dominio.Productos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid mt-4">
        <nav aria-label="breadcrumb">
            <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="Dashboard.aspx">Resumen</a></li>
                <li class="breadcrumb-item active">Productos</li>
            </ol>
        </nav>

        <div class="card">
            <div class="card-header d-flex justify-content-between align-items-center">
                <h5 class="mb-0">Lista de Productos</h5>
                <a href="AgregarProducto.aspx" class="btn btn-success">Crear Nuevo</a>
            </div>

            <div class="card-body">
                <!-- FILTROS Y PAGINACIÓN -->
                <div class="d-flex justify-content-between mb-3">
                    <div>
                        <label>Mostrar
                            <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" 
                                OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged"
                                CssClass="form-select form-select-sm d-inline w-auto">
                                <asp:ListItem Value="10" Selected="True">10</asp:ListItem>
                                <asp:ListItem Value="25">25</asp:ListItem>
                                <asp:ListItem Value="50">50</asp:ListItem>
                                <asp:ListItem Value="100">100</asp:ListItem>
                            </asp:DropDownList>
                            registros
                        </label>
                    </div>
                    <div>
                        <asp:TextBox ID="txtBuscar" runat="server" 
                            CssClass="form-control form-control-sm" 
                            placeholder="Buscar por nombre o marca..." 
                            onkeyup="filtrarProductos(this.value)">
                        </asp:TextBox>
                    </div>
                </div>

                <!-- GRIDVIEW -->
                <asp:GridView ID="gvProductos" runat="server" 
                    AutoGenerateColumns="False"
                    CssClass="table table-bordered table-hover table-striped align-middle"
                    HeaderStyle-CssClass="table-light"
                    AllowPaging="True"
                    OnPageIndexChanging="gvProductos_PageIndexChanging"
                    OnRowCommand="gvProductos_RowCommand"
                    DataKeyNames="Id"
                    EmptyDataText="No se encontraron productos.">
                    
                    <PagerStyle CssClass="pagination pagination-sm justify-content-center mt-3" 
                                HorizontalAlign="Center" />

                    <Columns>
                        <asp:TemplateField HeaderText="Producto">
                            <ItemTemplate>
                                <strong><%# Eval("Marca.Nombre") %> <%# Eval("Nombre") %></strong>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="DescripcionCorta" HeaderText="Descripción" />
                        <asp:BoundField DataField="Categoria.Nombre" HeaderText="Categoría" />
                        <asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="{0:C}" />
                        <asp:BoundField DataField="Stock" HeaderText="Stock" />
                        <asp:BoundField DataField="StockMinimo" HeaderText="Mínimo" />

                        <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEditar" runat="server" CommandName="Editar"
                                    CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-primary btn-sm me-1"
                                    ToolTip="Editar">
                                    <i class="bi bi-pencil-fill"></i>
                                </asp:LinkButton>

                                <asp:LinkButton ID="btnEliminar" runat="server" CommandName="Eliminar"
                                    CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-danger btn-sm btn-eliminar"
                                    data-id='<%# Eval("Id") %>' ToolTip="Eliminar">
                                    <i class="bi bi-trash-fill"></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

    <!-- SweetAlert para eliminar -->
    <script>
        let timer;
        function filtrarProductos(texto) {
            clearTimeout(timer);
            timer = setTimeout(function () {
                __doPostBack('filtrarProductos', texto);
            }, 500);
        }

        // También cuando borra con la X
        document.getElementById('<%= txtBuscar.ClientID %>').addEventListener('search', function () {
            filtrarProductos('');
        });
        document.addEventListener('DOMContentLoaded', function () {
            document.querySelectorAll('.btn-eliminar').forEach(btn => {
                btn.addEventListener('click', function (e) {
                    e.preventDefault();
                    const id = this.getAttribute('data-id');

                    Swal.fire({
                        title: '¿Eliminar producto?',
                        text: "Esta acción no se puede deshacer",
                        icon: 'warning',
                        showCancelButton: true,
                        confirmButtonColor: '#d33',
                        cancelButtonColor: '#3085d6',
                        confirmButtonText: 'Sí, eliminar',
                        cancelButtonText: 'Cancelar'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            __doPostBack('EliminarProducto', id);
                        }
                    });
                });
            });
        });
    </script>
</asp:Content>