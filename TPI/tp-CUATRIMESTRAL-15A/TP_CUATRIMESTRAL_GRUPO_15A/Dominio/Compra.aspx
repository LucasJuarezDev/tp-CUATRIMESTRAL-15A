<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="Compra.aspx.cs" Inherits="Dominio.Compra" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Finalizar Compra</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.0/font/bootstrap-icons.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.11.7/dist/umd/popper.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.min.js"></script>

    <script>
        function mostrarComprobante() {
            const ddl = document.getElementById('<%= ddlPago.ClientID %>');
            const div = document.getElementById('divComprobante');
            div.style.display = ddl.value === "2" ? "block" : "none";
        }

        function previewComprobante(input) {
            const imagen = document.getElementById("imgPreviewComprobante");
            if (input.files && input.files[0]) {
                imagen.style.display = "block";
                imagen.src = URL.createObjectURL(input.files[0]);
            } else {
                imagen.style.display = "none";
                imagen.src = "";
            }
        }

        function mostrarModalComprobante() {
            var modal = new bootstrap.Modal(document.getElementById('modalComprobante'));
            modal.show();
        }
    </script>

    <style>
        body { background-color: #212529; }
        .form-box, .resumen-box {
            background-color: #343a40;
            border-radius: 12px;
            color: white;
            padding: 30px;
            box-shadow: 0px 4px 12px rgba(0,0,0,0.3);
        }
        .form-label { color: #e9ecef; }
        .btn-finalizar {
            background-color: #6c757d;
            color: white;
        }
        .btn-finalizar:hover { background-color: #5a6268; }
        #imgPreviewComprobante {
            max-width: 100%;
            height: auto;
            border: 2px solid #444;
            border-radius: 10px;
            margin-top: 10px;
            display: none;
        }
    </style>

</head>
<body onload="mostrarComprobante()">
<form id="form1" runat="server">
<div class="container py-5">
    <h2 class="text-white mb-4 fw-bold">Finalizar Compra</h2>
    <div class="row g-4">

        <div class="col-lg-8">
            <div class="form-box">
                <h4 class="mb-4 fw-bold">Datos de Pago</h4>

                <div class="mb-4">
                    <label class="form-label fw-bold">Método de Pago</label>
                    <asp:DropDownList ID="ddlPago" runat="server"
                        CssClass="form-select form-select-lg"
                        onchange="mostrarComprobante()">
                        <asp:ListItem Value="1" Text="Efectivo (pago al recibir)"></asp:ListItem>
                        <asp:ListItem Value="2" Text="Transferencia bancaria"></asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div id="divComprobante" class="mb-4 p-3 bg-dark rounded" style="display:none;">
                    <label class="form-label fw-bold text-warning">Subir comprobante de transferencia</label>
                    <asp:FileUpload ID="fuComprobante" runat="server"
                        CssClass="form-control" accept="image/*"
                        onchange="previewComprobante(this)" />
                    <small class="text-muted">Acepta: JPG, PNG — Máx: 10 MB</small>
                    <img id="imgPreviewComprobante" />
                </div>

                <div class="mb-4 p-4 bg-dark rounded">
                    <div class="d-flex justify-content-between mb-2">
                        <span>Subtotal:</span>
                        <span>$ <asp:Label ID="lblSubtotal" runat="server" Font-Bold="true" /></span>
                    </div>
                    <div class="d-flex justify-content-between text-success fw-bold mb-3">
                        <span>Envío:</span>
                        <span>$ <asp:Label ID="lblEnvio" runat="server" Text="0" /></span>
                    </div>
                    <hr class="border-secondary" />
                    <div class="d-flex justify-content-between fs-3 fw-bold">
                        <span>TOTAL A PAGAR:</span>
                        <span class="text-success">$ <asp:Label ID="lblTotalFinal" runat="server" /></span>
                    </div>
                </div>

                <asp:Button ID="btnConfirmar" runat="server"
                    Text="Confirmar Compra"
                    CssClass="btn btn-finalizar w-100 py-3 fs-5"
                    OnClick="btnConfirmar_Click" />
            </div>
        </div>

        <div class="col-lg-4">
            <div class="resumen-box">
                <h4 class="fw-bold mb-3">Resumen del Pedido</h4>

                <asp:Repeater ID="repResumen" runat="server">
                    <ItemTemplate>
                        <div class="d-flex justify-content-between py-2 border-bottom border-secondary">
                            <span><%# Eval("Nombre") %> <small class="text-muted">x<%# Eval("Cantidad") %></small></span>
                            <span>$ <%# String.Format("{0:N0}", Eval("Subtotal")) %></span>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>

                <div class="d-flex justify-content-between py-3 border-bottom border-secondary">
                    <span>Envío</span>
                    <span class="text-success fw-bold">$ <asp:Label ID="lblEnvioResumen" runat="server" /></span>
                </div>

            </div>
        </div>

    </div>
</div>

<!-- MODAL Validacion comprobante -->
<div class="modal fade" id="modalComprobante" tabindex="-1" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content bg-dark text-white">
            <div class="modal-header border-secondary">
                <h5 class="modal-title">Comprobante requerido</h5>
                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
            </div>
            <div class="modal-body">
                Debes subir el comprobante para continuar con la compra.
            </div>
            <div class="modal-footer border-secondary">
                <button type="button" class="btn btn-warning" data-bs-dismiss="modal">Entendido</button>
            </div>
        </div>
    </div>
</div>

</form>
</body>
</html>
