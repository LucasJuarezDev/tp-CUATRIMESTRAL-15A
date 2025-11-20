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
    }
}
