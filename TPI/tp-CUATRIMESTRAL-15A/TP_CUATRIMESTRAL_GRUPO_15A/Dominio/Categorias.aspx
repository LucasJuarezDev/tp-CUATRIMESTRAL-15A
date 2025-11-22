<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageAdmin.Master" AutoEventWireup="true" CodeBehind="Categorias.aspx.cs" Inherits="Dominio.Categorias" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid mt-4">
        <div class="card shadow-sm">
            <div class="card-header d-flex justify-content-between align-items-center">
                <h6 class="mb-0">
                    <i class="bi bi-bookmark-fill me-2"></i>Lista de Categorías
                </h6>
            </div>

            <div class="card-body">
                <!-- Boton crear nuevo -->
                <div class="mb-3">
                    <a href="AgregarCategoria.aspx" class="btn btn-success">
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
                            CssClass="form-control form-control-sm" 
                            placeholder="Buscar por nombre..."
                            onkeyup="filtrarCategorias(this.value)">
                        </asp:TextBox>
                    </div>
                </div>

                <!-- GridView -->
                <div>
                    <asp:GridView ID="DGVcategorias" runat="server"
                        AutoGenerateColumns="false"
                        CssClass="table table-bordered table-hover align-middle"
                        AllowPaging="True"
                        OnPageIndexChanging="DGVcategorias_PageIndexChanging"
                        OnRowCommand="DGVcategorias_RowCommand"
                        DataKeyNames="Id"
                        PagerSettings-Mode="NumericFirstLast"
                        PagerSettings-FirstPageText="Anterior"
                        PagerSettings-LastPageText="Siguiente"
                        PagerSettings-PageButtonCount="10">

                        <PagerStyle CssClass="pagination pagination-sm justify-content-center mt-3" 
                                    HorizontalAlign="Center" />

                        <Columns>
                            <asp:BoundField HeaderText="ID" DataField="Id" />
                            <asp:BoundField HeaderText="Nombre" DataField="Nombre" />
                            <asp:BoundField HeaderText="Descripción" DataField="Descripcion" />
        
                            <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEditar" runat="server"
                                        CssClass="btn btn-primary btn-sm me-1"
                                        CommandName="Editar"
                                        CommandArgument='<%# Eval("Id") %>'>
                                        <i class="bi bi-pencil-fill"></i>
                                    </asp:LinkButton>

                                    <asp:LinkButton ID="btnEliminar" runat="server"
                                        CommandName="Eliminar"
                                        CommandArgument='<%# Eval("Id") %>'
                                        CssClass="btn btn-danger btn-sm btn-eliminar"
                                        data-id='<%# Eval("Id") %>'>
                                        <i class="bi bi-trash-fill"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

            </div>
        </div>
    </div>

   <script>
    let timerCat;
    function filtrarCategorias(texto) {
        clearTimeout(timerCat);
        timerCat = setTimeout(function () {
            __doPostBack('filtrarCategorias', texto);
        }, 500);
    }

    document.getElementById('<%= txtBuscar.ClientID %>').addEventListener('search', function () {
        filtrarCategorias('');
    });
    document.addEventListener('DOMContentLoaded', function () {
        document.querySelectorAll('.btn-eliminar').forEach(btn => {
            btn.addEventListener('click', function (e) {
                e.preventDefault(); 

                const id = this.getAttribute('data-id');
                const form = this.closest('form');

                Swal.fire({
                    title: '¿Eliminar categoria?',
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#d33',
                    cancelButtonColor: '#3085d6',
                    confirmButtonText: 'Ok',
                    cancelButtonText: 'Cancelar'
                }).then((result) => {
                    if (result.isConfirmed) {
                        __doPostBack('eliminarCategoria', id);
                    }
                });
            });
        });
    });
   </script>
</asp:Content>


