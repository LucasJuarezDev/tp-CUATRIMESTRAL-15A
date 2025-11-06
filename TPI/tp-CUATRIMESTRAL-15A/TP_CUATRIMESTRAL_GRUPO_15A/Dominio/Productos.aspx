<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageAdmin.Master" AutoEventWireup="true" CodeBehind="Productos.aspx.cs" Inherits="Dominio.Productos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid mt-4">

        <!-- Breadcrumb -->
        <nav aria-label="breadcrumb">
            <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="#">Resumen</a></li>
                <li class="breadcrumb-item active" aria-current="page">Productos</li>
            </ol>
        </nav>

        <!-- Card principal -->
        <div class="card">
            <div class="card-header d-flex justify-content-between align-items-center">
                <h5 class="mb-0"><i class="bi bi-box-seam"></i> Lista de Productos</h5>
                <a href="#" class="btn btn-success">Crear Nuevo</a>
            </div>

            <div class="card-body">

                <div class="d-flex justify-content-between mb-3">
                    <div>
                        <label>Mostrar
                            <select class="form-select form-select-sm d-inline w-auto">
                                <option>10</option>
                                <option>25</option>
                                <option>50</option>
                                <option>100</option>
                            </select> registros
                        </label>
                    </div>
                    <div>
                        <input type="text" class="form-control form-control-sm" placeholder="Buscar...">
                    </div>
                </div>

                <asp:GridView ID="gvProductos" runat="server"
                    AutoGenerateColumns="False"
                    CssClass="table table-bordered table-hover table-striped align-middle"
                    HeaderStyle-CssClass="table-light"
                    AllowPaging="True"
                    PageSize="10"
                    OnRowCommand="gvProductos_RowCommand"
                    DataKeyNames="Id">
    
                    <Columns>
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />

                        <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />

                        <asp:BoundField DataField="Marca.Nombre" HeaderText="Marca" />

                        <asp:BoundField DataField="Categoria.Nombre" HeaderText="Categoría" />

                        <asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="{0:C}" />

                        <asp:BoundField DataField="Stock" HeaderText="Stock" />

                        <asp:TemplateField HeaderText="Editar" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEditar" runat="server"
                                    CommandName="Editar"
                                    CommandArgument='<%# Eval("Id") %>'
                                    CssClass="btn btn-primary btn-sm"
                                    ToolTip="Editar">
                                    <i class="bi bi-pencil-fill"></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Eliminar" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEliminar" runat="server"
                                    CommandName="Eliminar"
                                    CommandArgument='<%# Eval("Id") %>'
                                    CssClass="btn btn-danger btn-sm"
                                    ToolTip="Eliminar"
                                    OnClientClick="return confirm('¿Eliminar producto?');">
                                    <i class="bi bi-trash-fill"></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                    <PagerStyle CssClass="pagination pagination-sm mb-0" />
                </asp:GridView>

                <div class="d-flex justify-content-between mt-2">
                    <div>Mostrando 1 a 3 de 3 registros</div>
                    <nav>
                        <ul class="pagination pagination-sm mb-0">
                            <li class="page-item disabled"><a class="page-link" href="#">Anterior</a></li>
                            <li class="page-item active"><a class="page-link" href="#">1</a></li>
                            <li class="page-item"><a class="page-link" href="#">Siguiente</a></li>
                        </ul>
                    </nav>
                </div>

            </div>
        </div>
    </div>

</asp:Content>


