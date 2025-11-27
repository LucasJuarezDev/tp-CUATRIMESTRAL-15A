<%@ Page Title="Gestión de Usuarios" Language="C#" MasterPageFile="~/MasterPageAdmin.Master" 
         AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs" Inherits="Dominio.Usuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid mt-4">
        <div class="card shadow-sm border-0">
            <div class="card-header bg-primary text-white d-flex justify-content-between align-items-center">
                <h5 class="mb-0">
                    <i class="bi bi-people-fill me-2"></i> Gestión de Usuarios
                </h5>
                <a href="RegistrarUsuario.aspx" class="btn btn-light btn-sm">
                    <i class="bi bi-person-plus-fill"></i> Nuevo Usuario
                </a>
            </div>

            <div class="card-body">
                <div class="row mb-4 g-3 align-items-end">
                    <div class="col-md-4">
                        <label class="form-label small text-muted">Mostrar</label>
                        <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true"
                                          OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged"
                                          CssClass="form-select form-select-sm">
                            <asp:ListItem Value="10">10</asp:ListItem>
                            <asp:ListItem Value="25" Selected="True">25</asp:ListItem>
                            <asp:ListItem Value="50">50</asp:ListItem>
                            <asp:ListItem Value="100">100</asp:ListItem>
                        </asp:DropDownList>
                        <span class="text-muted small ms-2">registros</span>
                    </div>

                    <div class="col-md-8 text-md-end">
                        <div class="input-group input-group-sm" style="max-width: 350px; margin-left: auto;">
                            <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control"
                                         placeholder="Buscar por usuario, email o rol..." />
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar"
                                        CssClass="btn btn-outline-success" OnClick="btnBuscar_Click" />
                        </div>
                    </div>
                </div>

                <div class="table-responsive">
                    <asp:GridView ID="gvUsuarios" runat="server"
                                  AutoGenerateColumns="false"
                                  CssClass="table table-hover align-middle"
                                  GridLines="None"
                                  AllowPaging="True"
                                  PageSize="10"
                                  OnPageIndexChanging="gvUsuarios_PageIndexChanging"
                                  OnRowCommand="gvUsuarios_RowCommand"
                                  EmptyDataText="No se encontraron usuarios."
                                  PagerStyle-CssClass="pagination pagination-sm mb-0"
                                    >
                        <PagerStyle CssClass="pagination pagination-sm justify-content-center mt-4" />
                        <PagerSettings Mode="NumericFirstLast" 
                                       FirstPageText="Primero" 
                                       LastPageText="Último" 
                                       PageButtonCount="10" />
                        
                        <Columns>
                            <asp:BoundField DataField="Nickname" HeaderText="Usuario" />
                            <asp:BoundField DataField="Email" HeaderText="Email" NullDisplayText="-" />
                            <asp:BoundField DataField="Rol.Nombre" HeaderText="Rol" />

                            <asp:TemplateField HeaderText="Estado" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <span class='<%# Convert.ToBoolean(Eval("Activo")) ? "badge bg-success" : "badge bg-danger" %>'>
                                        <%# Convert.ToBoolean(Eval("Activo")) ? "Activo" : "Inactivo" %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <div class="btn-group btn-group-sm">
                                        <asp:LinkButton runat="server" CommandName="Editar"
                                                        CommandArgument='<%# Eval("Id") %>'
                                                        CssClass="btn btn-outline-primary btn-sm" ToolTip="Editar">
                                            <i class="bi bi-pencil-fill"></i>
                                        </asp:LinkButton>

                                        <button type="button" class="btn btn-outline-danger btn-sm btn-eliminar"
                                                data-id='<%# Eval("Id") %>' title="Eliminar">
                                            <i class="bi bi-trash-fill"></i>
                                        </button>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

                <div class="text-center text-muted small mt-3">
                    <asp:Label ID="lblInfo" runat="server" />
                </div>
            </div>
        </div>
    </div>

    <script>
        document.addEventListener('DOMContentLoaded', function () {
            document.querySelectorAll('.btn-eliminar').forEach(btn => {
                btn.addEventListener('click', function (e) {
                    e.preventDefault();
                    const id = this.dataset.id;
                    Swal.fire({
                        title: '¿Eliminar usuario?',
                        text: "Esta acción no se puede deshacer",
                        icon: 'warning',
                        showCancelButton: true,
                        confirmButtonColor: '#d33',
                        cancelButtonColor: '#3085d6',
                        confirmButtonText: 'Sí, eliminar',
                        cancelButtonText: 'Cancelar'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            __doPostBack('EliminarUsuario', id);
                        }
                    });
                });
            });
        });
    </script>
</asp:Content>

