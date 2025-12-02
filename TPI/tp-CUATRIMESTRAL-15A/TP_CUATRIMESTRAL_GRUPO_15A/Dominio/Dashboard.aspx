<%@ Page Title="Dashboard" Language="C#" 
    MasterPageFile="~/MasterPageAdmin.Master" 
    AutoEventWireup="true" 
    CodeBehind="Dashboard.aspx.cs" 
    Inherits="Dominio.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Panel de estadisticas -->
    <div class="row mb-4">

        <div class="col-md-4">
            <div class="card text-white bg-success mb-3">
                <div class="card-body d-flex justify-content-between align-items-center">
                    <div>
                        <h6 class="card-title">Cantidad Clientes:</h6>
                        <h3><asp:Label ID="lblCantidadClientes" runat="server" Text="0"></asp:Label></h3>
                    </div>
                    <i class="bi bi-people-fill" style="font-size:2rem;"></i>
                </div>
            </div>
        </div>

        <div class="col-md-4">
            <div class="card text-dark bg-warning mb-3">
                <div class="card-body d-flex justify-content-between align-items-center">
                    <div>
                        <h6 class="card-title">Cantidad Ventas:</h6>
                        <h3><asp:Label ID="lblCantidadVentas" runat="server" Text="0"></asp:Label></h3>
                    </div>
                    <i class="bi bi-bag-fill" style="font-size:2rem;"></i>
                </div>
            </div>
        </div>

        <div class="col-md-4">
            <div class="card text-white bg-secondary mb-3">
                <div class="card-body d-flex justify-content-between align-items-center">
                    <div>
                        <h6 class="card-title">Cantidad Productos:</h6>
                        <h3><asp:Label ID="lblCantidadProductos" runat="server" Text="0"></asp:Label></h3>
                    </div>
                    <i class="bi bi-box-seam" style="font-size:2rem;"></i>
                </div>
            </div>
        </div>

    </div>

    <!-- FILTROS -->
    <div class="card mb-3">
        <div class="card-body">
            <div class="row g-3 align-items-end">

                <div class="col-md-3">
                    <label class="form-label">Fecha Inicio</label>
                    <asp:TextBox ID="txtFechaInicio" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                </div>

                <div class="col-md-3">
                    <label class="form-label">Fecha Fin</label>
                    <asp:TextBox ID="txtFechaFin" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                </div>

                <div class="col-md-3">
                    <label class="form-label">ID Venta</label>
                    <asp:TextBox ID="txtIdVenta" runat="server" CssClass="form-control"></asp:TextBox>
                </div>

                <div class="col-md-3">
                    <label class="form-label">Cliente</label>
                    <asp:TextBox ID="txtCliente" runat="server" CssClass="form-control" Placeholder="Mail o nombre"></asp:TextBox>
                </div>

                <div class="col-md-3 mt-3">
                    <asp:Button ID="btnBuscar" runat="server" CssClass="btn btn-primary w-100" Text="Buscar" OnClick="btnBuscar_Click" />
                </div>

            </div>
        </div>
    </div>

    <!-- TABLA DE RESULTADOS -->
    <div class="card">
        <div class="card-body">

            <asp:GridView ID="gvVentas" runat="server" AutoGenerateColumns="false"
                CssClass="table table-bordered table-hover text-center">

                <Columns>
                    <asp:BoundField DataField="FechaVenta" HeaderText="Fecha Venta" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="Cliente" HeaderText="Cliente" />
                    <asp:BoundField DataField="Producto" HeaderText="Producto" />
                    <asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="${0:N2}" />
                    <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                    <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="${0:N2}" />
                    <asp:BoundField DataField="IdVenta" HeaderText="ID Transacción" />
                </Columns>

            </asp:GridView>

        </div>
    </div>

</asp:Content>


