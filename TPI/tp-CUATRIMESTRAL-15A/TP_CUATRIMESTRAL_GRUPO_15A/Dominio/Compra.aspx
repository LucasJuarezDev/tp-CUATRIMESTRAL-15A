<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Compra.aspx.cs" Inherits="Dominio.Compra" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Finalizar Compra</title>

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />

    <style>
        body {
            background-color: #212529;
        }

        .form-box {
            background-color: #343a40;
            border-radius: 12px;
            color: white;
            padding: 30px;
            box-shadow: 0px 4px 12px rgba(0,0,0,0.3);
        }

        .resumen-box {
            background-color: #495057;
            border-radius: 12px;
            padding: 25px;
            color: white;
            box-shadow: 0px 4px 12px rgba(0,0,0,0.3);
        }

        .form-label {
            color: #e9ecef;
        }

        .btn-finalizar {
            background-color: #6c757d;
            color: white;
        }

        .btn-finalizar:hover {
            background-color: #5a6268;
        }

        .card-box {
            background-color: #3e444a;
            padding: 20px;
            border-radius: 10px;
            margin-top: 15px;
            box-shadow: inset 0px 0px 8px rgba(0,0,0,0.4);
        }

        .small-input {
            width: 120px;
        }
    </style>
</head>

<body>
<form id="form1" runat="server">

<div class="container py-5">

    <h2 class="text-white mb-4 fw-bold">Finalizar Compra</h2>

    <div class="row g-4">

        <div class="col-lg-8">
            <div class="form-box">

                <h4 class="mb-4 fw-bold">Datos de Pago</h4>

                <div class="mb-3">
                    <label class="form-label">Tipo de Pago</label>
                    <asp:DropDownList ID="ddlPago" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlPago_SelectedIndexChanged"></asp:DropDownList>
                </div>

                <asp:Panel ID="pnlTarjeta" runat="server" Visible="false" CssClass="card-box">

                    <h5 class="fw-bold mb-3">Datos de Tarjeta</h5>

                    <div class="mb-3">
                        <label class="form-label">Número de Tarjeta</label>
                        <asp:TextBox ID="txtNumeroTarjeta" runat="server" CssClass="form-control" MaxLength="16" placeholder="1234 5678 9012 3456"></asp:TextBox>
                    </div>

                    <div class="row">
                        <div class="col-md-8 mb-3">
                            <label class="form-label">Fecha de Vencimiento</label>
                            <asp:TextBox ID="txtVencimiento" runat="server" CssClass="form-control" placeholder="MM/AA"></asp:TextBox>
                        </div>

                        <div class="col-md-4 mb-3">
                            <label class="form-label">CVV</label>
                            <asp:TextBox ID="txtCVV" runat="server" CssClass="form-control small-input" MaxLength="4" placeholder="***"></asp:TextBox>
                        </div>
                    </div>

                </asp:Panel>

                <div class="mt-4">
                    <label class="form-label">Envío</label>
                    <asp:DropDownList ID="ddlEnvio" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlEnvio_SelectedIndexChanged">
                        <asp:ListItem Text="Retiro en tienda (Gratis)" Value="0" />
                        <asp:ListItem Text="Envío a domicilio ($500)" Value="500" />
                    </asp:DropDownList>
                </div>

                <div class="mb-3 mt-3">
                    <label class="form-label">Comentarios (opcional)</label>
                    <asp:TextBox ID="txtComentario" TextMode="MultiLine" Rows="3" CssClass="form-control" runat="server"></asp:TextBox>
                </div>

                <asp:Button ID="btnConfirmar" runat="server"
                    Text="Confirmar Compra"
                    CssClass="btn btn-finalizar w-100 mt-3"
                    OnClick="btnConfirmar_Click" />

            </div>
        </div>

        <div class="col-lg-4">
            <div class="resumen-box">
                <h4 class="fw-bold mb-3">Resumen</h4>

                <p class="mb-1 text-white-50">Productos:</p>

                <asp:Repeater ID="repResumen" runat="server">
                    <ItemTemplate>
                        <div class="d-flex justify-content-between border-bottom border-secondary py-2">
                            <span><%# Eval("Nombre") %> (x<%# Eval("Cantidad") %>)</span>

                            <!-- CORREGIDO: ELIMINO FORMATO DE MONEDA -->
                            <span>$ <%# String.Format("{0:N2}", Eval("Subtotal")) %></span>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>

                <div class="d-flex justify-content-between align-items-center mt-4 pt-3 border-top border-secondary">
                    <span class="fw-bold">TOTAL:</span>
                    <asp:Label ID="lblTotal" runat="server" CssClass="h4 fw-bold">$0.00</asp:Label>
                </div>
            </div>
        </div>

    </div>
</div>

</form>
</body>
</html>



