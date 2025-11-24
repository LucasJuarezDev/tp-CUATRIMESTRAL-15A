<%@ Page Title="Carrito" Language="C#" MasterPageFile="~/MasterPageCliente.Master" AutoEventWireup="true" CodeBehind="Carrito.aspx.cs" Inherits="Dominio.Carrito" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        /*  productos */
        .carrito-box {
            height: 500px;              
            overflow-y: auto;           
            overflow-x: hidden;
        }

        /*  resumen */
        .resumen-box {
            height: 500px;              
        }
    </style>

    <div class="container py-5">
        <div class="row g-4">

            <!-- PANEL CUANDO ESTA VACIO -->
            <asp:Panel ID="pnlVacio" runat="server" Visible="true" CssClass="col-lg-12">
                <div class="card text-white border-0 rounded-3 shadow-sm" style="background-color: #495057;">
                    <div class="card-body text-center py-5">
                        <i class="bi bi-cart3 display-1 text-white mb-3"></i>
                        <h3 class="fw-bold mb-2">Aún no hay ítems en el carrito!</h3>
                        <p class="text-white-50 small">Agrega primero un producto para poder visualizar el carrito</p>
                    </div>
                </div>
            </asp:Panel>

            <!-- PANEL CUANDO HAY PRODUCTOS -->
            <asp:Panel ID="pnlCarrito" runat="server" Visible="false" CssClass="col-lg-12">

                <div class="row">

                    <!-- LISTADO DE PRODUCTOS -->
                    <div class="col-lg-8">

                        <div class="card border-0 shadow-sm rounded-3 mb-4" style="background-color: #343a40;">

                            <div class="card-body carrito-box">

                                <h4 class="text-white mb-3">Productos en tu carrito</h4>

                                <asp:GridView ID="gvCarrito" runat="server" AutoGenerateColumns="False"
                                    CssClass="table table-dark table-striped"
                                    OnRowCommand="gvCarrito_RowCommand">

                                    <Columns>

                                        <asp:BoundField DataField="Nombre" HeaderText="Producto" />
                                        <asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="{0:C}" />

                                       
                                        <asp:TemplateField HeaderText="Cantidad">
                                            <ItemTemplate>

                                                <asp:LinkButton runat="server"
                                                    CssClass="btn btn-sm btn-secondary"
                                                    CommandName="Restar"
                                                    CommandArgument='<%# Eval("IdProducto") %>'>
                                                    -
                                                </asp:LinkButton>

                                                <span class="mx-2"><%# Eval("Cantidad") %></span>

                                                <asp:LinkButton runat="server"
                                                    CssClass="btn btn-sm btn-secondary"
                                                    CommandName="Sumar"
                                                    CommandArgument='<%# Eval("IdProducto") %>'>
                                                    +
                                                </asp:LinkButton>

                                            </ItemTemplate>
                                        </asp:TemplateField>

                                      
                                        <asp:TemplateField HeaderText="Eliminar">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server"
                                                    CssClass="btn btn-danger btn-sm"
                                                    CommandName="Eliminar"
                                                    CommandArgument='<%# Eval("IdProducto") %>'>
                                                    X
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>

                                </asp:GridView>

                            </div>
                        </div>
                    </div>

                    <div class="col-lg-4">
                        <div class="card text-white border-0 rounded-3 shadow-sm resumen-box" style="background-color: #495057;">
                            <div class="card-body d-flex flex-column">

                                <h5 class="fw-bold mb-4">RESUMEN DE PEDIDO</h5>

                                <div class="d-flex justify-content-between align-items-center mb-4">
                                    <span class="fw-bold">TOTAL:</span>
                                    <asp:Label ID="lblTotal" runat="server" CssClass="h4 fw-bold">$0.00</asp:Label>
                                </div>

                                <button type="button"
                                        class="btn btn-secondary w-100 mt-auto"
                                        runat="server"
                                        onserverclick="btnContinuar_Click">
                                     CONTINUAR
                                </button>

                            </div>
                        </div>
                    </div>

                </div>

            </asp:Panel>

        </div>
    </div>

</asp:Content>



