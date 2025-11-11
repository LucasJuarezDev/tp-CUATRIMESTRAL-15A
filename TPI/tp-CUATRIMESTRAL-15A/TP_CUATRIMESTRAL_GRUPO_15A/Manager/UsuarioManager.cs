using Clases; // Para la clase Usuario
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Manager
{
    public class UsuarioManager
    {
        public List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("SELECT us.ID, us.NICKNAME, us.CONTRASENA, us.EMAIL, r.ROL, us.ACTIVO FROM USUARIO us INNER JOIN ROL r ON us.ROLE_ID = r.ID WHERE us.ACTIVO = 1");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Usuario aux = new Usuario();
                    aux.Id = Convert.ToInt64(datos.Lector["ID"]);
                    aux.Nickname = datos.Lector["NICKNAME"].ToString();
                    aux.Contrasena = datos.Lector["CONTRASENA"].ToString();
                    aux.Email = datos.Lector["EMAIL"].ToString();
                    aux.Activo = Convert.ToBoolean(datos.Lector["ACTIVO"]);

                    Rol rol = new Rol();
                    string rolNombre = datos.Lector["ROL"].ToString();

                    switch (rolNombre)
                    {
                        case "ADMIN":
                            rol.Id = 1;
                            break;
                        case "EMPLEADO":
                            rol.Id = 2;
                            break;
                        case "CLIENTE": 
                            rol.Id = 3;
                            break;
                    }
                    rol.Nombre = datos.Lector["ROL"].ToString();
                    aux.Rol = rol; 
                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }

        public long Agregar(Usuario usuario)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("sp_AgregarUsuario");
                datos.Comando.CommandType = CommandType.StoredProcedure;

                datos.SetearParametro("@Nickname", usuario.Nickname);
                datos.SetearParametro("@Contrasena", usuario.Contrasena); 
                datos.SetearParametro("@Email", usuario.Email);
                datos.SetearParametro("@RolId", usuario.Rol.Id);         // ← Aquí usamos el ID del Rol
                datos.SetearParametro("@Activo", usuario.Activo);

                object resultado = datos.ejecutarEscalar();
                return resultado != DBNull.Value ? Convert.ToInt64(resultado) : -1;
            }
            catch (SqlException ex) when (ex.Number >= 50001 && ex.Number <= 50002)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar usuario: " + ex.Message);
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }

        public void Eliminar(long id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("sp_bajaUsuario");
                datos.Comando.CommandType = CommandType.StoredProcedure;

                datos.SetearParametro("@Id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }

        public void Modificar(Usuario usuario)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("sp_ModificarUsuario");
                datos.Comando.CommandType = CommandType.StoredProcedure;

                datos.SetearParametro("@Id", usuario.Id);
                datos.SetearParametro("@Nickname", usuario.Nickname);
                datos.SetearParametro("@Email", usuario.Email);
                datos.SetearParametro("@RolId", usuario.Rol.Id);

                datos.ejecutarAccion();
            }
            catch (SqlException ex) when (ex.Number == 50001 || ex.Number == 50002)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }

        public Usuario buscarPorId(long id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"SELECT u.ID, u.NICKNAME, u.EMAIL, u.ROLE_ID, u.ACTIVO, r.ROL AS NombreRol FROM USUARIO u INNER JOIN ROL r ON r.ID = u.ROLE_ID WHERE u.ID = @Id AND u.ACTIVO = 1");
                datos.SetearParametro("@Id", id);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    return new Usuario
                    {
                        Id = (long)datos.Lector["ID"],
                        Nickname = datos.Lector["NICKNAME"].ToString(),
                        Email = datos.Lector["EMAIL"].ToString(),
                        Rol = new Rol
                        {
                            Id = (byte)datos.Lector["ROLE_ID"],
                            Nombre = datos.Lector["NombreRol"].ToString()
                        },
                        Activo = (bool)datos.Lector["ACTIVO"]
                    };
                }

                return null; 
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar usuario por ID: " + ex.Message);
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }

        public int ContarClientes()
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("SELECT COUNT(*) FROM USUARIO WHERE ROLE_ID = 3 AND ACTIVO = 1");
                return (int)datos.ejecutarEscalar();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al contar clientes: " + ex.Message);
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }
    }
}

