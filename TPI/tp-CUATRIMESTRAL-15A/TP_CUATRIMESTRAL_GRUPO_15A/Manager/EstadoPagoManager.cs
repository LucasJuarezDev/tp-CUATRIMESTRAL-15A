using System;
using System.Collections.Generic;
using Clases;

namespace Manager
{
    public class EstadoPagoManager
    {
        // ======================================
        // 1. LISTAR ESTADOS DE PAGO
        // ======================================

        public List<EstadoPago> Listar()
        {
            List<EstadoPago> lista = new List<EstadoPago>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("SELECT ID, NOMBRE FROM ESTADO_PAGO ORDER BY ID");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    EstadoPago aux = new EstadoPago();

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
                throw new Exception("Error al listar estados de pago: " + ex.Message, ex);
            }
            finally
            {
                datos.CerrarConeccion();
            }
        }


        // ======================================
        // 2. CREAR ESTADO DE PAGO
        // ======================================
        public void Crear(EstadoPago estado)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("INSERT INTO ESTADO_PAGO (NOMBRE) VALUES (@nombre)");
                datos.LimpiarParametros();
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
            }
        }

        // ======================================
        // 3. MODIFICAR ESTADO DE PAGO
        // ======================================
        public void Modificar(EstadoPago estado)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("UPDATE ESTADO_PAGO SET NOMBRE = @nombre WHERE ID = @id");
                datos.LimpiarParametros();
                datos.SetearParametro("@nombre", estado.Nombre);
                datos.SetearParametro("@id", estado.Id);

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

        // ======================================
        // ObtenerPorId
        // ======================================

        public EstadoPago ObtenerPorId(byte id)
        {
            AccesoDatos datos = new AccesoDatos();
            EstadoPago estado = null;
            try
            {
                datos.SetearConsulta("SELECT ID, NOMBRE FROM ESTADO_PAGO WHERE ID = @id");
                datos.SetearParametro("@id", id);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    estado = new EstadoPago
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
