using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clases;

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
                datos.SetearConsulta(@"
            SELECT 
                e.ID, e.NOMBRE, e.APELLIDO, e.TELEFONO, 
                e.FECHAINGRESO, e.SUELDO, e.ID_USUARIO, e.ACTIVO
            FROM EMPLEADO e
            WHERE e.ACTIVO = 1
            ORDER BY e.APELLIDO, e.NOMBRE");

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

        public void Eliminar(long id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("UPDATE EMPLEADO SET ACTIVO = 0 WHERE ID = @Id");
                datos.SetearParametro("@Id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar empleado: " + ex.Message, ex);
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }
    }
}
