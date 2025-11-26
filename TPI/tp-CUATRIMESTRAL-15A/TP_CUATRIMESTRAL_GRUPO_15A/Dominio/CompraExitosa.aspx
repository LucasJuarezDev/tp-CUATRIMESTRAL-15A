<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompraExitosa.aspx.cs" Inherits="Dominio.CompraExitosa" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Compra Exitosa</title>

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />

    <style>
        body {
            background-color: #212529;
            color: white;
        }

        .success-box {
            background-color: #343a40;
            padding: 40px;
            border-radius: 12px;
            box-shadow: 0 0 12px rgba(0,0,0,0.4);
            text-align: center;
        }

        .success-icon {
            font-size: 80px;
            color: #28a745;
        }

        .btn-volver {
            background-color: #6c757d;
            color: white;
        }

        .btn-volver:hover {
            background-color: #5a6268;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <div class="container d-flex justify-content-center align-items-center vh-100">

            <div class="success-box col-12 col-md-6 col-lg-4">

                <div class="success-icon mb-3">
                    <i class="bi bi-check-circle-fill"></i>
                </div>

                <h2 class="fw-bold mb-3">¡Gracias por tu compra!</h2>

                <p class="text-white-50 mb-4">
                    Tu pedido ha sido procesado con éxito.
                </p>

                <a href="Catalogo.aspx" class="btn btn-volver w-100">
                    Volver al Catálogo
                </a>

            </div>

        </div>
    </form>

    <!-- Bootstrap Icons -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.1/font/bootstrap-icons.css">
</body>
</html>
