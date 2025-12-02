using System;
using System.Collections.Generic;
using Manager;
using Clases;

namespace Manager
{
    public class EstadoPreparacionManager
    {
        // Obtener todos los estados de preparacion
        public List<EstadoPreparacion> Listar()
        {
            List<EstadoPreparacion> lista = new List<EstadoPreparacion>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("SELECT ID, NOMBRE FROM ESTADO_PREPARACION ORDER BY ID");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    EstadoPreparacion aux = new EstadoPreparacion();

                    aux.Id = Convert.ToInt32(datos.Lector["ID"]);
                    aux.Nombre = datos.Lector["NOMBRE"] != DBNull.Value
                                 ? datos.Lector["NOMBRE"].ToString()
                                 : string.Empty;

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar estados de preparación: " + ex.Message, ex);
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }

        // Buscar un estado por ID
        public EstadoPreparacion ObtenerPorId(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            EstadoPreparacion aux = null;

            try
            {
                datos.SetearConsulta("SELECT ID, NOMBRE FROM ESTADO_PREPARACION WHERE ID = @ID");
                datos.SetearParametro("@ID", id);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    aux = new EstadoPreparacion();
                    aux.Id = (int)datos.Lector["ID"];
                    aux.Nombre = datos.Lector["NOMBRE"].ToString();
                }

                return aux;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }

        // Crear un nuevo estado de preparacion
        public void Crear(string nombre)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("INSERT INTO ESTADO_PREPARACION (NOMBRE) VALUES (@NOMBRE)");
                datos.SetearParametro("@NOMBRE", nombre);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }

        // Modificar un estado existente
        public void Modificar(int id, string nombre)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("UPDATE ESTADO_PREPARACION SET NOMBRE = @NOMBRE WHERE ID = @ID");
                datos.SetearParametro("@ID", id);
                datos.SetearParametro("@NOMBRE", nombre);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }


        //ObtenerPorId
        public EstadoPreparacion ObtenerPorId(byte id)
        {
            AccesoDatos datos = new AccesoDatos();
            EstadoPreparacion estado = null;
            try
            {
                datos.SetearConsulta("SELECT ID, NOMBRE FROM ESTADO_PREPARACION WHERE ID = @id");
                datos.SetearParametro("@id", id);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    estado = new EstadoPreparacion
                    {
                        Id = Convert.ToByte(datos.Lector["ID"]),
                        Nombre = datos.Lector["NOMBRE"].ToString()
                    };
                }
            }
            finally
            {
                datos.CerrarConeccion();
            }
            return estado;
        }

    }
}

