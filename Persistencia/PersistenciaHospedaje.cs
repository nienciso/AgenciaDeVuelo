using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Entidades;

namespace Persistencia
{
    internal class PersistenciaHospedaje : IPHospedaje
    {
        private static PersistenciaHospedaje instancia = null;

        private PersistenciaHospedaje() { }

        public static PersistenciaHospedaje GetInstancia()
        {
            if (instancia == null)
                instancia = new PersistenciaHospedaje();

            return instancia;
        }

        public void AltaHospedaje(Hospedaje hospedaje)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("HospedajeAlta", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.AddWithValue("@CodigoInterno", hospedaje.CodigoInterno);
                comando.Parameters.AddWithValue("@Nombre", hospedaje.Nombre);
                comando.Parameters.AddWithValue("@Direccion", hospedaje.Direccion);
                comando.Parameters.AddWithValue("@TipoHospedaje", hospedaje.TipoHospedaje);
                comando.Parameters.AddWithValue("@PrecioNochePersona", hospedaje.PrecioNochePersona);
                comando.Parameters.AddWithValue("@CodigoEstado", hospedaje.Estado.Codigo);

                SqlParameter retorno = new SqlParameter("@Retorno", SqlDbType.Int);
                retorno.Direction = ParameterDirection.ReturnValue;
                comando.Parameters.Add(retorno);

                try
                {
                    conexion.Open();
                    comando.ExecuteNonQuery();

                    int r = Convert.ToInt32(retorno.Value);

                    if (r == -1)
                        throw new Exception("Ya existe un hospedaje con ese código interno.");

                    if (r == -2)
                        throw new Exception("El estado indicado no existe o está inactivo.");

                    if (r == -3)
                        throw new Exception("El tipo de hospedaje debe ser Hotel STD, Posada o All Inclusive.");

                    if (r == -4)
                        throw new Exception("El precio por noche por persona debe ser mayor a cero.");

                    if (r != 1)
                        throw new Exception("No se pudo dar de alta el hospedaje.");
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al dar de alta el hospedaje. " + ex.Message);
                }
            }
        }

        public void ModificarHospedaje(Hospedaje hospedaje)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("HospedajeModificar", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.AddWithValue("@CodigoInterno", hospedaje.CodigoInterno);
                comando.Parameters.AddWithValue("@Nombre", hospedaje.Nombre);
                comando.Parameters.AddWithValue("@Direccion", hospedaje.Direccion);
                comando.Parameters.AddWithValue("@TipoHospedaje", hospedaje.TipoHospedaje);
                comando.Parameters.AddWithValue("@PrecioNochePersona", hospedaje.PrecioNochePersona);
                comando.Parameters.AddWithValue("@CodigoEstado", hospedaje.Estado.Codigo);
                comando.Parameters.AddWithValue("@Activo", hospedaje.Activo);

                SqlParameter retorno = new SqlParameter("@Retorno", SqlDbType.Int);
                retorno.Direction = ParameterDirection.ReturnValue;
                comando.Parameters.Add(retorno);

                try
                {
                    conexion.Open();
                    comando.ExecuteNonQuery();

                    int r = Convert.ToInt32(retorno.Value);

                    if (r == -1)
                        throw new Exception("No existe un hospedaje con ese código interno.");

                    if (r == -2)
                        throw new Exception("El estado indicado no existe o está inactivo.");

                    if (r == -3)
                        throw new Exception("El tipo de hospedaje debe ser Hotel STD, Posada o All Inclusive.");

                    if (r == -4)
                        throw new Exception("El precio por noche por persona debe ser mayor a cero.");

                    if (r != 1)
                        throw new Exception("No se pudo modificar el hospedaje.");
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al modificar el hospedaje. " + ex.Message);
                }
            }
        }

        public void BajaHospedaje(string codigoInterno)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("HospedajeBaja", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.AddWithValue("@CodigoInterno", codigoInterno);

                SqlParameter retorno = new SqlParameter("@Retorno", SqlDbType.Int);
                retorno.Direction = ParameterDirection.ReturnValue;
                comando.Parameters.Add(retorno);

                try
                {
                    conexion.Open();
                    comando.ExecuteNonQuery();

                    int r = Convert.ToInt32(retorno.Value);

                    if (r == -1)
                        throw new Exception("No existe un hospedaje con ese código interno.");

                    if (r == 2)
                        return;

                    if (r != 1)
                        throw new Exception("No se pudo eliminar el hospedaje.");
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al eliminar el hospedaje. " + ex.Message);
                }
            }
        }

        public Hospedaje BuscarHospedaje(string codigoInterno)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("HospedajeBuscar", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@CodigoInterno", codigoInterno);

                try
                {
                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            Estado estado = FabricaP.GetPersistenciaEstado().BuscarEstado(
                                lector["CodigoEstado"].ToString());

                            return new Hospedaje(
                                lector["CodigoInterno"].ToString(),
                                lector["Nombre"].ToString(),
                                lector["Direccion"].ToString(),
                                lector["TipoHospedaje"].ToString(),
                                Convert.ToDecimal(lector["PrecioNochePersona"]),
                                estado,
                                Convert.ToBoolean(lector["Activo"])
                            );
                        }

                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al buscar el hospedaje. " + ex.Message);
                }
            }
        }

        public List<Hospedaje> ListarHospedajes(string filtroNombre)
        {
            List<Hospedaje> lista = new List<Hospedaje>();

            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("HospedajeListado", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                if (filtroNombre == null)
                    filtroNombre = "";

                comando.Parameters.AddWithValue("@FiltroNombre", filtroNombre);

                try
                {
                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            Estado estado = FabricaP.GetPersistenciaEstado().BuscarEstado(
                                lector["CodigoEstado"].ToString());

                            Hospedaje hospedaje = new Hospedaje(
                                lector["CodigoInterno"].ToString(),
                                lector["Nombre"].ToString(),
                                lector["Direccion"].ToString(),
                                lector["TipoHospedaje"].ToString(),
                                Convert.ToDecimal(lector["PrecioNochePersona"]),
                                estado,
                                Convert.ToBoolean(lector["Activo"])
                            );

                            lista.Add(hospedaje);
                        }
                    }

                    return lista;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al listar hospedajes. " + ex.Message);
                }
            }
        }

        public List<Hospedaje> ListarHospedajesPorEstado(Estado estadoDestino)
        {
            List<Hospedaje> lista = new List<Hospedaje>();

            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("HospedajesPorEstado", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@CodigoEstadoDestino", estadoDestino.Codigo);

                try
                {
                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            Hospedaje hospedaje = new Hospedaje(
                                lector["CodigoInterno"].ToString(),
                                lector["Nombre"].ToString(),
                                lector["Direccion"].ToString(),
                                lector["TipoHospedaje"].ToString(),
                                Convert.ToDecimal(lector["PrecioNochePersona"]),
                                estadoDestino,
                                true
                            );

                            lista.Add(hospedaje);
                        }
                    }

                    return lista;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al listar hospedajes por estado. " + ex.Message);
                }
            }
        }
    }
}