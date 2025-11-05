<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgregarMarca.aspx.cs" Inherits="Dominio.AgregarMarca" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Agregar Marca</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</head>
<body class="bg-light">

    <form id="form1" runat="server">
        <!-- Modal centrado -->
        <div class="modal show d-block" tabindex="-1" style="background-color: rgba(0,0,0,0.5);">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content shadow">
                    <div class="modal-header">
                        <h5 class="modal-title">Nueva Marca</h5>
                        <button type="button" class="btn-close" onclick="window.location='Marcas.aspx';"></button>
                    </div>
                    <div class="modal-body">
                        <div class="mb-3">
                            <label for="txtNombre" class="form-label">Nombre</label>
                            <asp:TextBox ID="txtNombre" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label for="txtDescripcion" class="form-label">Descripción</label>
                            <asp:TextBox ID="txtDescripcion" CssClass="form-control" TextMode="MultiLine" Rows="3" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" >Cerrar</button>
                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardar_Click"  />
                    </div>
                </div>
            </div>
        </div>
    </form>

</body>
</html>


