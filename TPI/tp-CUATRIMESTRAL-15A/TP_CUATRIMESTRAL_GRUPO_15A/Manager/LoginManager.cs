using Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class LoginManager
    {
        public UsuarioLogueado Login(string nickname, string password)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"SELECT * FROM vw_LoginUsuario WHERE NICKNAME = @Nickname AND Contrasena = @Contrasena");

                datos.SetearParametro("@Nickname", nickname);
                datos.SetearParametro("@Contrasena", password);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    var user = new UsuarioLogueado
                    {
                        Id = (long)datos.Lector["UsuarioId"],
                        Nickname = datos.Lector["NICKNAME"].ToString(),
                        Email = datos.Lector["EMAIL"]?.ToString() ?? "",
                        Rol = new Rol
                        {
                            Id = Convert.ToByte(datos.Lector["RolId"]),
                            Nombre = datos.Lector["RolNombre"].ToString()
                        }
                    };

                    // Si es cliente
                    if (datos.Lector["ClienteId"] != DBNull.Value)
                    {
                        user.Cliente = new Cliente
                        {
                            Id = (long)datos.Lector["ClienteId"],
                            Nombre = datos.Lector["ClienteNombre"].ToString(),
                            Apellido = datos.Lector["ClienteApellido"].ToString(),
                            Telefono = datos.Lector["ClienteTelefono"]?.ToString(),
                            RazonSocial = datos.Lector["ClienteRazonSocial"]?.ToString(),
                            FechaRegistro = datos.Lector["ClienteFechaRegistro"] != DBNull.Value
                                ? (DateTime)datos.Lector["ClienteFechaRegistro"]
                                : DateTime.MinValue,
                            Usuario = new Usuario { Id = user.Id, Nickname = user.Nickname, Rol = user.Rol }
                        };
                    }

                    // Si es empleado/admin
                    if (datos.Lector["EmpleadoId"] != DBNull.Value)
                    {
                        user.Empleado = new Empleado
                        {
                            Id = (long)datos.Lector["EmpleadoId"],
                            Nombre = datos.Lector["EmpleadoNombre"].ToString(),
                            Apellido = datos.Lector["EmpleadoApellido"].ToString(),
                            Telefono = datos.Lector["EmpleadoTelefono"]?.ToString(),
                            Usuario = new Usuario { Id = user.Id, Nickname = user.Nickname, Rol = user.Rol }
                        };
                    }

                    return user;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al iniciar sesión: " + ex.Message);
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }
    }
}
