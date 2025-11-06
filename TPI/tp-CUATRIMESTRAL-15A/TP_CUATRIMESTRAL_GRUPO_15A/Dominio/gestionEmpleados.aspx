<%@ Page Title="Gestión de Empleados" Language="C#" MasterPageFile="~/MasterPageAdmin.Master" AutoEventWireup="true" CodeBehind="gestionEmpleados.aspx.cs" Inherits="Dominio.gestionEmpleados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid mt-4">
        <div class="card shadow-sm">
            <div class="card-header bg-primary text-white d-flex justify-content-between align-items-center">
                <h5 class="mb-0">
                    <i class="bi bi-person-badge-fill me-2"></i> Gestión de Empleados
                </h5>
                <a href="AgregarEmpleado.aspx" class="btn btn-success btn-sm">
                    <i class="bi bi-plus-circle me-1"></i> Nuevo Empleado
                </a>
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
                                         placeholder="Buscar por nombre, apellido o teléfono..." />
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" 
                                        CssClass="btn btn-outline-primary" OnClick="btnBuscar_Click" />
                        </div>
                    </div>
                </div>

                <!-- GridView de Empleados -->
                <div class="table-responsive">
                    <asp:GridView ID="gvEmpleados" runat="server"
                        AutoGenerateColumns="False"
                        CssClass="table table-hover table-bordered align-middle"
                        GridLines="None"
                        AllowPaging="True"
                        PageSize="25"
                        OnPageIndexChanging="gvEmpleados_PageIndexChanging"
                        OnRowCommand="gvEmpleados_RowCommand"
                        PagerStyle-CssClass="pagination pagination-sm mb-0"
                        EmptyDataText="No hay empleados registrados.">
                        
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="ID" 
                                            ItemStyle-Width="60" ItemStyle-HorizontalAlign="Center" />

                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />

                            <asp:BoundField DataField="Apellido" HeaderText="Apellido" />

                            <asp:BoundField DataField="Telefono" HeaderText="Teléfono" 
                                            ItemStyle-HorizontalAlign="Center" />

                            <asp:BoundField DataField="FechaIngreso" HeaderText="Fecha Ingreso" 
                                            DataFormatString="{0:dd/MM/yyyy}" 
                                            ItemStyle-HorizontalAlign="Center" />

                            <asp:BoundField DataField="Sueldo" HeaderText="Sueldo" 
                                            DataFormatString="{0:C0}" 
                                            ItemStyle-HorizontalAlign="Right" />

                            <asp:TemplateField HeaderText="Estado" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <span class='<%# Convert.ToBoolean(Eval("Estado")) ? "badge bg-success" : "badge bg-secondary" %>'>
                                        <%# Convert.ToBoolean(Eval("Estado")) ? "Activo" : "Inactivo" %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="120">
                                <ItemTemplate>
                                    <div class="btn-group btn-group-sm" role="group">
                                        <asp:LinkButton ID="btnEditar" runat="server"
                                            CommandName="Editar"
                                            CommandArgument='<%# Eval("Id") %>'
                                            CssClass="btn btn-outline-primary"
                                            ToolTip="Editar empleado">
                                            <i class="bi bi-pencil-fill"></i>
                                        </asp:LinkButton>

                                        <asp:LinkButton ID="btnEliminar" runat="server"
                                            CommandName="Eliminar"
                                            CommandArgument='<%# Eval("Id") %>'
                                            CssClass="btn btn-outline-danger"
                                            ToolTip="Eliminar empleado"
                                            OnClientClick="return confirm('¿Estás seguro de eliminar este empleado?');">
                                            <i class="bi bi-trash-fill"></i>
                                        </asp:LinkButton>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>

                        <PagerSettings Mode="NumericFirstLast" 
                                       FirstPageText="Primero" 
                                       LastPageText="Último" 
                                       PageButtonCount="5" />
                    </asp:GridView>
                </div>

                <!-- Información de paginación -->
                <div class="d-flex justify-content-between align-items-center mt-3 text-muted small">
                    <div>
                        <asp:Label ID="lblInfo" runat="server" Text="" />
                    </div>
                    <div>
                        Mostrando 
                        <asp:Label ID="lblRegistrosMostrados" runat="server" Font-Bold="true" />
                        de 
                        <asp:Label ID="lblTotalRegistros" runat="server" Font-Bold="true" /> registros
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>