<%@ Page Title="Gestión de Clientes" Language="C#" MasterPageFile="~/MasterPageAdmin.Master"
         AutoEventWireup="true" CodeBehind="gestionClientes.aspx.cs" Inherits="Dominio.gestionClientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid mt-4">
        <div class="card shadow-sm">
            <div class="card-header bg-success text-white d-flex justify-content-between align-items-center">
                <h5 class="mb-0">
                    <i class="bi bi-people-fill me-2"></i> Gestión de Clientes
                </h5>
            </div>

            <div class="card-body">
                <!-- Filtros y búsqueda -->
                <div class="row mb-3 g-3 align-items-end">
                    <div class="col-md-3">
                        <label class="form-label small text-muted">Mostrar</label>
                        <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true"
                                          CssClass="form-select form-select-sm" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                            <asp:ListItem Value="10">10</asp:ListItem>
                            <asp:ListItem Value="25" Selected="True">25</asp:ListItem>
                            <asp:ListItem Value="50">50</asp:ListItem>
                            <asp:ListItem Value="100">100</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-4 offset-md-5">
                        <div class="input-group input-group-sm">
                            <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control"
                                         placeholder="Buscar por nombre, apellido, email, nickname o teléfono..." />
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar"
                                        CssClass="btn btn-outline-success" OnClick="btnBuscar_Click" />
                        </div>
                    </div>
                </div>

                <!-- GridView de Clientes -->
                <div class="table-responsive">
                    <asp:GridView ID="gvClientes" runat="server"
                                  AutoGenerateColumns="false"
                                  CssClass="table table-hover table-bordered align-middle"
                                  GridLines="None"
                                  AllowPaging="true"
                                  PageSize="25"
                                  OnPageIndexChanging="gvClientes_PageIndexChanging"
                                  OnRowCommand="gvClientes_RowCommand"
                                  EmptyDataText="No hay clientes registrados.">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="ID" 
                                            ItemStyle-Width="70" ItemStyle-HorizontalAlign="Center" />

                            <asp:TemplateField HeaderText="Cliente">
                                <ItemTemplate>
                                    <strong><%# Eval("Nombre") %> <%# Eval("Apellido") %></strong>
                                    <%# !string.IsNullOrEmpty(Eval("RazonSocial")?.ToString()) 
                                        ? $"<br><small class='text-muted'>Empresa: {Eval("RazonSocial")}</small>" 
                                        : "" %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Contacto">
                                <ItemTemplate>
                                    <%# string.IsNullOrWhiteSpace(Eval("Telefono")?.ToString()) ? "-" : Eval("Telefono") %><br />
                                    <small class="text-muted">
                                        <%# Eval("Usuario") != null && !string.IsNullOrEmpty(Eval("Usuario.Email")?.ToString()) 
                                            ? Eval("Usuario.Email") : "-" %>
                                    </small>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Nickname">
                                <ItemTemplate>
                                    <%# Eval("Usuario") != null && !string.IsNullOrEmpty(Eval("Usuario.Nickname")?.ToString()) 
                                        ? Eval("Usuario.Nickname") : "<em class='text-muted'>Sin usuario</em>" %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="FechaRegistro" HeaderText="Fecha Registro"
                                            DataFormatString="{0:dd/MM/yyyy}"
                                            ItemStyle-HorizontalAlign="Center" ItemStyle-Width="130" />

                            <asp:TemplateField HeaderText="Estado" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <span class='<%# Convert.ToBoolean(Eval("Activo")) ? "badge bg-success" : "badge bg-danger" %>'>
                                        <%# Convert.ToBoolean(Eval("Activo")) ? "Activo" : "Inactivo" %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="120">
                                <ItemTemplate>
                                    <div class="btn-group btn-group-sm" role="group">
                                        <asp:LinkButton runat="server" CommandName="ToggleEstado"
                                                        CommandArgument='<%# Eval("Id") %>'
                                                        CssClass='<%# Convert.ToBoolean(Eval("Activo")) ? "btn btn-outline-warning btn-sm" : "btn btn-outline-success btn-sm" %>'
                                                        ToolTip='<%# Convert.ToBoolean(Eval("Activo")) ? "Desactivar" : "Activar" %>'>
                                            <i class='<%# Convert.ToBoolean(Eval("Activo")) ? "bi bi-person-x-fill" : "bi bi-person-check-fill" %>'></i>
                                        </asp:LinkButton>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle CssClass="pagination pagination-sm justify-content-center mt-3" />
                    </asp:GridView>
                </div>

                <div class="text-muted small mt-3 text-center">
                    Mostrando <strong><%# gvClientes.Rows.Count %></strong> 
                    de <strong><%# gvClientes.DataSource != null ? ((IEnumerable<object>)gvClientes.DataSource).Count() : 0 %></strong> clientes
                </div>
            </div>
        </div>
    </div>
</asp:Content>