using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Clases;

namespace Manager
{
    public class EstadoEnvioManager
    {
        public List<EstadoEnvio> Listar()
        {
            List<EstadoEnvio> lista = new List<EstadoEnvio>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("SELECT ID, NOMBRE FROM ESTADO_ENVIO ORDER BY ID");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    EstadoEnvio aux = new EstadoEnvio();

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
                throw new Exception("Error al listar estados de envío: " + ex.Message, ex);
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }


        public void Agregar(EstadoEnvio estado)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("INSERT INTO ESTADO_ENVIO (NOMBRE) VALUES (@nombre)");
                datos.SetearParametro("@nombre", estado.Nombre);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConeccion();
                datos.LimpiarParametros();
            }
        }

        public void Modificar(EstadoEnvio estado)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("UPDATE ESTADO_ENVIO SET NOMBRE = @nombre WHERE ID = @id");
                datos.SetearParametro("@id", estado.Id);
                datos.SetearParametro("@nombre", estado.Nombre);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConeccion();
                datos.LimpiarParametros();
            }
        }



        public EstadoEnvio ObtenerPorId(byte id)
        {
            AccesoDatos datos = new AccesoDatos();
            EstadoEnvio estado = null;
            try
            {
                datos.SetearConsulta("SELECT ID, NOMBRE FROM ESTADO_ENVIO WHERE ID = @id");
                datos.SetearParametro("@id", id);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    estado = new EstadoEnvio
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

