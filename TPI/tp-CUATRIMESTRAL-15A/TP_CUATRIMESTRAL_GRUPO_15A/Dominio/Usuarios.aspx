<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageAdmin.Master" AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs" Inherits="Dominio.Usuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid mt-4">
 

        <!-- Card principal -->
            <div class="card">
             <div class="card-header d-flex justify-content-between align-items-center">
                  <h5 class="mb-0"><i class="bi bi-people-fill"></i> Lista de Usuarios</h5>
                   <a href="AgregarUsuario.aspx" class="btn btn-success">Crear Nuevo</a>
             </div>
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

                <!-- Tabla de usuarios -->
                <div class="table-responsive">
                    <table class="table table-bordered table-hover table-striped align-middle">
                        <thead class="table-light">
                            <tr>
                                <th>Nombres</th>
                                <th>Apellidos</th>
                                <th>Correo</th>
                                <th>Activo</th>
                                <th>Acciones</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>test</td>
                                <td>user</td>
                                <td>admin@example.com</td>
                                <td><span class="badge bg-success">Sí</span></td>
                                <td>
                                    <a href="#" class="btn btn-primary btn-sm"><i class="bi bi-pencil-fill"></i></a>
                                    <a href="#" class="btn btn-danger btn-sm"><i class="bi bi-trash-fill"></i></a>
                                </td>
                            </tr>
                            <tr>
                                <td>user001</td>
                                <td>ape001</td>
                                <td>user001@gmail.com</td>
                                <td><span class="badge bg-success">Sí</span></td>
                                <td>
                                    <a href="#" class="btn btn-primary btn-sm"><i class="bi bi-pencil-fill"></i></a>
                                    <a href="#" class="btn btn-danger btn-sm"><i class="bi bi-trash-fill"></i></a>
                                </td>
                            </tr>
                            <tr>
                                <td>user002</td>
                                <td>ape002</td>
                                <td>xapodn63@xixx.site</td>
                                <td><span class="badge bg-success">Sí</span></td>
                                <td>
                                    <a href="#" class="btn btn-primary btn-sm"><i class="bi bi-pencil-fill"></i></a>
                                    <a href="#" class="btn btn-danger btn-sm"><i class="bi bi-trash-fill"></i></a>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <!-- Paginación (ver si dejamos esto o lo sacamos) -->
                <div class="d-flex justify-content-between">
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

