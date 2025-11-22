<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageAdmin.Master" AutoEventWireup="true" CodeBehind="Marcas.aspx.cs" Inherits="Dominio.Marcas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid mt-4">

        <!-- contenedor principal -->
        <div class="card shadow-sm">
            <div class="card-header d-flex justify-content-between align-items-center">
                <h6 class="mb-0">
                    <i class="bi bi-bookmark-fill me-2"></i>Lista de Marcas
                </h6>
            </div>
            <div class="card-body">

                <!-- Boton crear nuevo -->
                <div class="mb-3">
                    <a href="AgregarMarca.aspx" class="btn btn-success">
                        <i class="bi bi-plus-circle me-1"></i> Crear Nuevo
                    </a>
                </div>

                <!-- Controles superiores -->
                <div class="d-flex justify-content-between align-items-center mb-3 flex-wrap gap-2">
                    <div>
                        <label class="form-label mb-0">
                            Mostrar
                            <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" 
                                OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged"
                                CssClass="form-select form-select-sm d-inline w-auto mx-1">
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
                            placeholder="Buscar por nombre..." 
                            CssClass="form-control form-control-sm" 
                            AutoPostBack="true"
                            OnTextChanged="txtBuscar_TextChanged">
                        </asp:TextBox>
                    </div>
                </div>

                <asp:GridView ID="DGVmarcas" runat="server" 
                    AutoGenerateColumns="false" 
                    CssClass="table table-bordered table-hover align-middle"
                    AllowPaging="True"
                    PageSize="10"
                    OnPageIndexChanging="DGVmarcas_PageIndexChanging"
                    OnRowCommand="DGVmarcas_RowCommand"
                    DataKeyNames="ID">
    
                    <PagerStyle CssClass="pagination pagination-sm justify-content-center mt-3" />

                    <Columns>
                        <asp:BoundField HeaderText="Nombre" DataField="Nombre" />
                        <asp:BoundField HeaderText="Descripción" DataField="Descripcion" />
        
                        <asp:TemplateField HeaderText="Modificar">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEditar" runat="server" CssClass="btn btn-primary btn-sm" 
                                    CommandName="Editar" CommandArgument='<%# Eval("ID") %>'>
                                    <i class="bi bi-pencil-fill"></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
        
                        <asp:TemplateField HeaderText="Eliminar">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEliminar" runat="server" CssClass="btn btn-danger btn-sm btn-eliminar" 
                                    CommandName="Eliminar" CommandArgument='<%# Eval("ID") %>'
                                    data-id='<%# Eval("ID") %>'>
                                    <i class="bi bi-trash-fill"></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                </div>

            </div>
        </div>
    </div>
    <script>
        // Esto hace que filtre apenas dejás de escribir 600ms (sin postback molesto)
        let timer;
        document.getElementById('<%= txtBuscar.ClientID %>').addEventListener('keyup', function () {
        clearTimeout(timer);
        timer = setTimeout(function () {
            __doPostBack('<%= txtBuscar.UniqueID %>', '');
        }, 600);
    });
    </script>
</asp:Content>


