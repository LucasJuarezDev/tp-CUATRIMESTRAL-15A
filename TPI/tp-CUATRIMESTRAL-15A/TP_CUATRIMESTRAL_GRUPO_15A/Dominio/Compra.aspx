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
    </style>
</head>

<body>
    <form id="form1" runat="server">

        <div class="container py-5">

            <h2 class="text-white mb-4 fw-bold">Finalizar Compra</h2>

            <div class="row g-4">

                <!-- FORMULARIO -->
                <div class="col-lg-8">
                    <div class="form-box">

                        <h4 class="mb-4 fw-bold">Datos de Pago</h4>

                        <!-- Tipo de pago -->
                        <div class="mb-3">
                            <label class="form-label">Tipo de Pago</label>
                            <asp:DropDownList ID="ddlPago" runat="server" CssClass="form-select"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlPago_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>

                        <!-- DATOS TARJETA (OCULTOS AL INICIO) -->
                        <div id="panelTarjeta" runat="server" visible="false">

                            <div class="mb-3">
                                <label class="form-label">Número de Tarjeta</label>
                                <asp:TextBox ID="txtNumeroTarjeta" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>

                            <div class="mb-3">
                                <label class="form-label">Vencimiento (MM/AA)</label>
                                <asp:TextBox ID="txtVencimiento" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>

                            <div class="mb-3">
                                <label class="form-label">Código de Seguridad (CVV)</label>
                                <asp:TextBox ID="txtCVV" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>

                        </div>

                        <!-- ENVIO -->
                        <div class="mb-3 mt-4">
                            <label class="form-label">Tipo de Entrega</label>

                            <asp:DropDownList ID="ddlEnvio" runat="server" CssClass="form-select"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlEnvio_SelectedIndexChanged">

                                <asp:ListItem Text="Retiro en local (gratis)" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Con envío (+$500)" Value="500"></asp:ListItem>

                            </asp:DropDownList>
                        </div>

                        <!-- Comentarios -->
                        <div class="mb-3">
                            <label class="form-label">Comentarios (opcional)</label>
                            <asp:TextBox ID="txtComentario" TextMode="MultiLine" Rows="3" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>

                        <asp:Button ID="btnConfirmar" runat="server"
                            Text="Confirmar Compra"
                            CssClass="btn btn-finalizar w-100 mt-3"
                            OnClick="btnConfirmar_Click" />

                    </div>
                </div>

                <!-- RESUMEN -->
                <div class="col-lg-4">
                    <div class="resumen-box">
                        <h4 class="fw-bold mb-3">Resumen</h4>

                        <asp:Repeater ID="repResumen" runat="server">
                            <ItemTemplate>
                                <div class="d-flex justify-content-between border-bottom border-secondary py-2">
                                    <span><%# Eval("Nombre") %> (x<%# Eval("Cantidad") %>)</span>
                                    <span>$ <%# Eval("Subtotal") %></span>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>

                        <div class="d-flex justify-content-between mt-4 pt-3 border-top border-secondary">
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


