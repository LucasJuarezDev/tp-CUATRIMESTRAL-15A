using Clases;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class EmpleadoManager
    {
        public List<Empleado> Listar()
        {
            List<Empleado> lista = new List<Empleado>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"SELECT e.ID, e.NOMBRE, e.APELLIDO, e.TELEFONO, e.FECHAINGRESO, e.SUELDO, e.ID_USUARIO, e.ACTIVO FROM EMPLEADO e WHERE e.ACTIVO = 1 ORDER BY e.APELLIDO, e.NOMBRE");

                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Empleado emp = new Empleado
                    {
                        Id = (long)datos.Lector["ID"],
                        Nombre = datos.Lector["NOMBRE"].ToString(),
                        Apellido = datos.Lector["APELLIDO"].ToString(),
                        Telefono = datos.Lector["TELEFONO"].ToString(),
                        FechaIngreso = (DateTime)datos.Lector["FECHAINGRESO"],
                        Sueldo = (decimal)datos.Lector["SUELDO"],
                        Estado = (bool)datos.Lector["ACTIVO"]
                    };

                    lista.Add(emp);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar empleados: " + ex.Message, ex);
            }
            finally
            {
                datos.CerrarConeccion();
            }

            return lista;
        }

        public Empleado BuscarPorId(long id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                
                datos.SetearConsulta("SELECT E.ID, E.NOMBRE, E.APELLIDO, E.TELEFONO, E.FECHAINGRESO, E.SUELDO, E.ID_USUARIO, E.ACTIVO AS Estado, U.NICKNAME, U.EMAIL, U.ROLE_ID, U.ACTIVO AS UsuarioActivo FROM EMPLEADO E INNER JOIN USUARIO U ON E.ID_USUARIO = U.ID WHERE E.ID = @Id");
                datos.SetearParametro("@Id", id);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    var empleado = new Empleado
                    {
                        Id = (long)datos.Lector["ID"],
                        Nombre = datos.Lector["NOMBRE"]?.ToString(),
                        Apellido = datos.Lector["APELLIDO"]?.ToString(),
                        Telefono = datos.Lector["TELEFONO"]?.ToString(),
                        FechaIngreso = (DateTime)datos.Lector["FECHAINGRESO"],
                        Sueldo = Convert.ToDecimal(datos.Lector["SUELDO"]),
                        Estado = (bool)datos.Lector["Estado"]
                    };

                    long idUsuario = (long)datos.Lector["ID_USUARIO"];
                    UsuarioManager manager = new UsuarioManager();
                    empleado.Usuario = manager.buscarPorId(idUsuario);

                    return empleado;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar empleado: " + ex.Message);
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }

        public void Modificar(Empleado empleado)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("UPDATE EMPLEADO SET NOMBRE = @Nombre, APELLIDO = @Apellido, TELEFONO = @Telefono, SUELDO = @Sueldo WHERE ID = @Id");
                datos.SetearParametro("@Id", empleado.Id);
                datos.SetearParametro("@Nombre", empleado.Nombre ?? (object)DBNull.Value);
                datos.SetearParametro("@Apellido", empleado.Apellido ?? (object)DBNull.Value);
                datos.SetearParametro("@Telefono", string.IsNullOrWhiteSpace(empleado.Telefono) ? (object)DBNull.Value : empleado.Telefono);
                datos.SetearParametro("@Sueldo", empleado.Sueldo);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar empleado: " + ex.Message);
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }
    }
}
