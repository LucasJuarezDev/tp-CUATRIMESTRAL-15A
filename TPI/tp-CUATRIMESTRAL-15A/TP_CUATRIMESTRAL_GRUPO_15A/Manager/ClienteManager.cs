using Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class ClienteManager
    {
        public void RegistrarCliente(Cliente cliente, string contrasena)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("sp_RegistrarCliente");
                datos.Comando.CommandType = CommandType.StoredProcedure;

                // Parámetros del SP
                datos.SetearParametro("@Nickname", cliente.Usuario.Nickname);
                datos.SetearParametro("@Contrasena", contrasena);
                datos.SetearParametro("@Email", string.IsNullOrWhiteSpace(cliente.Usuario.Email) ? (object)DBNull.Value : cliente.Usuario.Email.Trim());
                datos.SetearParametro("@Nombre", cliente.Nombre);
                datos.SetearParametro("@Apellido", cliente.Apellido);
                datos.SetearParametro("@Telefono", string.IsNullOrWhiteSpace(cliente.Telefono) ? (object)DBNull.Value : cliente.Telefono.Trim());
                datos.SetearParametro("@EsEmpresa", cliente.RazonSocial != null && cliente.RazonSocial.Trim() != "");
                datos.SetearParametro("@RazonSocial",
                    !string.IsNullOrWhiteSpace(cliente.RazonSocial) ? cliente.RazonSocial.Trim() : (object)DBNull.Value);

                datos.ejecutarAccion();
            }
            catch (SqlException ex) when (ex.Number == 50001 || ex.Number == 50002)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el cliente: " + ex.Message);
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }

        public List<Cliente> Listar(string filtro = "")
        {
            List<Cliente> lista = new List<Cliente>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta(@"
            SELECT 
                c.ID, c.NOMBRE, c.APELLIDO, c.TELEFONO, c.FECHA_REGISTRO,
                c.RAZON_SOCIAL, c.ACTIVO,
                u.ID AS IdUsuario, u.NICKNAME, u.EMAIL, u.ACTIVO AS UsuarioActivo
            FROM CLIENTE c
            LEFT JOIN USUARIO u ON c.ID_USUARIO = u.ID
            WHERE 1=1
        ");

                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Cliente cliente = new Cliente
                    {
                        Id = (long)datos.Lector["ID"],
                        Nombre = datos.Lector["NOMBRE"].ToString(),
                        Apellido = datos.Lector["APELLIDO"].ToString(),
                        Telefono = datos.Lector["TELEFONO"] as string,
                        FechaRegistro = (DateTime)datos.Lector["FECHA_REGISTRO"],
                        RazonSocial = datos.Lector["RAZON_SOCIAL"] as string,
                        Activo = (bool)datos.Lector["ACTIVO"],

                        // Cargar Usuario si existe
                        Usuario = datos.Lector["IdUsuario"] != DBNull.Value ? new Usuario
                        {
                            Id = (long)datos.Lector["IdUsuario"],
                            Nickname = datos.Lector["NICKNAME"]?.ToString(),
                            Email = datos.Lector["EMAIL"]?.ToString(),
                            Activo = (bool)datos.Lector["UsuarioActivo"]
                        } : null
                    };

                    lista.Add(cliente);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cargar clientes: " + ex.Message);
            }
            finally
            {
                datos.CerrarConeccion();
            }

            return lista;
        }

        // Para activar/desactivar cliente
        public void CambiarEstado(long idCliente)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("UPDATE CLIENTE SET ACTIVO = ~ACTIVO WHERE ID = @id");
                datos.SetearParametro("@id", idCliente);
                datos.ejecutarAccion();
            }
            finally { datos.CerrarConeccion(); }
        }
    }
}
