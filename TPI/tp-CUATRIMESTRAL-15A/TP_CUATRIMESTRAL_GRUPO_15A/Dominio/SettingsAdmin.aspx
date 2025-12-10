<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPageAdmin.Master" CodeBehind="SettingsAdmin.aspx.cs" Inherits="Dominio.SettingsAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .card { background-color: #343a40; border: none; border-radius: 15px; }
        .form-control, .form-select { background-color: #495057; border: none; text-decoration-color: white; }
        .form-control:focus { background-color: #495057; text-decoration-color: white; box-shadow: 0 0 0 0.2rem rgba(255,193,7,0.25); }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container py-5">
        <div class="row justify-content-center">
            <div class="col-lg-8">
                <h2 class="text-center mb-5 text-dark fw-bold">
                    <i class="bi bi-gear-fill"></i> Configuración del Sistema
                </h2>

                <div class="card shadow-lg">
                    <div class="card-body p-5">
                        
                        <!-- COSTO ENVIO -->
                        <div class="mb-4">
                            <label class="form-label fw-bold text-white">Costo de Envío ($)</label>
                            <div class="input-group">
                                <span class="input-group-text bg-dark border-0 text-white">$</span>
                                <asp:TextBox ID="txtCostoEnvio" runat="server" CssClass="form-control form-control-lg text-white" TextMode="Number" min="0" step="100"></asp:TextBox>
                            </div>
                        </div>

                        <!-- WHATSAPP ADMIN -->
                        <div class="mb-4">
                            <label class="form-label fw-bold text-white">Número de WhatsApp del Admin</label>
                            <div class="input-group">
                                <span class="input-group-text bg-dark border-0 text-white">
                                    <i class="bi bi-whatsapp text-success"></i>
                                </span>
                                <asp:TextBox ID="txtWhatsApp" runat="server" CssClass="form-control form-control-lg text-white" placeholder="5491167152188"></asp:TextBox>
                            </div>
                        </div>

                        <!-- EMAIL ADMIN -->
                        <div class="mb-5">
                            <label class="form-label fw-bold text-white">Email del Administrador</label>
                            <div class="input-group">
                                <span class="input-group-text bg-dark border-0 text-white">@</span>
                                <asp:TextBox ID="txtEmailAdmin" runat="server" CssClass="form-control form-control-lg text-white" TextMode="Email" placeholder="admin@tutienda.com"></asp:TextBox>
                            </div>
                        </div>

                        <hr class="border-secondary" />

                        <div class="d-grid d-md-flex justify-content-md-between gap-3">
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar Cambios" 
                                        CssClass="btn btn-success btn-lg px-5" OnClick="btnGuardar_Click" />
                            
                            <asp:Button ID="btnProbarWhatsApp" runat="server" Text="Probar WhatsApp" 
                                        CssClass="btn btn-primary btn-lg px-5 text-white fw-bold" 
                                        OnClick="btnProbarWhatsApp_Click" CausesValidation="false" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
</asp:Content>