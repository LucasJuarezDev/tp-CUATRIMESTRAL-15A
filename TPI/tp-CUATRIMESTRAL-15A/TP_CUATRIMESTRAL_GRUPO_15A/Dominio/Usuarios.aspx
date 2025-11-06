<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageAdmin.Master" AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs" Inherits="Dominio.Usuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="container-fluid mt-4">
            <div class="card">
                <div class="card-header d-flex justify-content-between align-items-center">
                    <h5 class="mb-0"><i class="bi bi-people-fill"></i> Lista de Usuarios</h5>
                    <a href="RegistrarUsuario.aspx" class="btn btn-success btn-sm">Crear Nuevo</a>
                </div>
                <div class="card-body">
                    <!-- Filtros -->
                    <div class="d-flex justify-content-between mb-3">
                        <div>
                            <label>Mostrar
                                <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" CssClass="form-select form-select-sm d-inline w-auto">
                                    <asp:ListItem Value="10">10</asp:ListItem>
                                    <asp:ListItem Value="25">25</asp:ListItem>
                                    <asp:ListItem Value="50">50</asp:ListItem>
                                    <asp:ListItem Value="100">100</asp:ListItem>
                                </asp:DropDownList> registros
                            </label>
                        </div>
                        <div>
                            <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control form-control-sm" placeholder="Buscar..."></asp:TextBox>
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary btn-sm mt-1"  />
                        </div>
                    </div>

                    <!-- GridView con estilos Bootstrap -->
                    <asp:GridView ID="gvUsuarios" runat="server"
                        AutoGenerateColumns="False"
                        CssClass="table table-bordered table-hover table-striped align-middle"
                        OnRowCommand="gvUsuarios_RowCommand"
                       >
                        
                        <Columns>
                            <asp:BoundField DataField="Nickname" HeaderText="Usuario" />
                            <asp:BoundField DataField="Email" HeaderText="Email" />
                            <asp:BoundField DataField="Rol.Nombre" HeaderText="Rol" />
    
                            <asp:TemplateField HeaderText="Activo">
                                <ItemTemplate>
                                    <span class='<%# Convert.ToBoolean(Eval("Activo")) ? "badge bg-success" : "badge bg-danger" %>'>
                                        <%# Convert.ToBoolean(Eval("Activo")) ? "Sí" : "No" %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEditar" runat="server" 
                                        CommandName="Editar" 
                                        CommandArgument='<%# Eval("Id") %>'
                                        CssClass="btn btn-primary btn-sm" 
                                        ToolTip="Editar">
                                        <i class="bi bi-pencil-fill"></i>
                                    </asp:LinkButton>
            
                                    <asp:LinkButton ID="btnEliminar" runat="server" 
                                        CommandName="Eliminar" 
                                        CommandArgument='<%# Eval("Id") %>'
                                        CssClass="btn btn-danger btn-sm" 
                                        ToolTip="Eliminar"
                                        OnClientClick="return confirm('¿Eliminar usuario?');">
                                        <i class="bi bi-trash-fill"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>

                        <PagerStyle CssClass="pagination pagination-sm mb-0" />
                    </asp:GridView>

                    <!-- Paginación manual (opcional, ya está en GridView) -->
                    <div class="d-flex justify-content-between mt-3">
                        <div>
                            <asp:Label ID="lblInfo" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
</asp:Content>

