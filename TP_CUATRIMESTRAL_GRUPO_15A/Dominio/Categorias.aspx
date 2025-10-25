<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageAdmin.Master" AutoEventWireup="true" CodeBehind="Categorias.aspx.cs" Inherits="Dominio.Categorias" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
        <!-- contenedor principal -->
        <div class="card shadow-sm">
            <div class="card-header d-flex justify-content-between align-items-center">
                <h6 class="mb-0"><i class="bi bi-bookmark-fill me-2"></i>Lista de Categorias</h6>
            </div>
            <div class="card-body">

                <!-- Botón crear nuevo -->
                <div class="mb-3">
                    <a href="#" class="btn btn-success"><i class="bi bi-plus-circle me-1"></i> Crear Nuevo</a>
                </div>

                <!-- Controles superiores -->
                <div class="d-flex justify-content-between align-items-center mb-3 flex-wrap gap-2">
                    <div>
                        <label class="form-label mb-0">
                            Mostrar
                            <select class="form-select form-select-sm d-inline w-auto mx-1">
                                <option>10</option>
                                <option>25</option>
                                <option>50</option>
                                <option>100</option>
                            </select>
                            registros
                        </label>
                    </div>
                    <div>
                        <input type="text" class="form-control form-control-sm" placeholder="Buscar...">
                    </div>
                </div>

                <!-- Tabla -->
                <div class="table-responsive">
                    <table class="table table-bordered table-hover align-middle">
                        <thead class="table-light">
                            <tr>
                                <th style="width:70%">Descripción</th>
                                <th style="width:15%">Editar</th>
                                <th style="width:15%">Eliminar</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>Alimento</td>
                                <td class="text-center">
                                    <a href="#" class="btn btn-primary btn-sm"><i class="bi bi-pencil-fill"></i></a>
                                </td>
                                <td class="text-center">
                                    <a href="#" class="btn btn-danger btn-sm"><i class="bi bi-trash-fill"></i></a>
                                </td>
                            </tr>
                            <tr>
                                <td>Bebida</td>
                                <td class="text-center">
                                    <a href="#" class="btn btn-primary btn-sm"><i class="bi bi-pencil-fill"></i></a>
                                </td>
                                <td class="text-center">
                                    <a href="#" class="btn btn-danger btn-sm"><i class="bi bi-trash-fill"></i></a>
                                </td>
                            </tr>
                            <tr>
                                <td>Limpieza</td>
                                <td class="text-center">
                                    <a href="#" class="btn btn-primary btn-sm"><i class="bi bi-pencil-fill"></i></a>
                                </td>
                                <td class="text-center">
                                    <a href="#" class="btn btn-danger btn-sm"><i class="bi bi-trash-fill"></i></a>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <!-- Paginacion -->
                <div class="d-flex justify-content-between align-items-center mt-2 flex-wrap">
                    <div class="small text-muted">Mostrando 1 a 10 de 23 registros</div>
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
