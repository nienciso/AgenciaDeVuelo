using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Entidades;

namespace Persistencia
{
    internal class PersistenciaVuelo : IPVuelo
    {
        private static PersistenciaVuelo instancia = null;

        private PersistenciaVuelo() { }

        public static PersistenciaVuelo GetInstancia()
        {
            if (instancia == null)
                instancia = new PersistenciaVuelo();

            return instancia;
        }

        public void AltaVuelo(Vuelo vuelo)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("VueloAlta", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.AddWithValue("@Codigo", vuelo.Codigo);
                comando.Parameters.AddWithValue("@FechaHoraPartida", vuelo.FechaHoraPartida);
                comando.Parameters.AddWithValue("@CodigoEstadoPartida", vuelo.EstadoPartida.Codigo);
                comando.Parameters.AddWithValue("@FechaHoraLlegada", vuelo.FechaHoraLlegada);
                comando.Parameters.AddWithValue("@CodigoEstadoLlegada", vuelo.EstadoLlegada.Codigo);
                comando.Parameters.AddWithValue("@PrecioPasaje", vuelo.PrecioPasaje);

                SqlParameter retorno = new SqlParameter("@Retorno", SqlDbType.Int);
                retorno.Direction = ParameterDirection.ReturnValue;
                comando.Parameters.Add(retorno);

                try
                {
                    conexion.Open();
                    comando.ExecuteNonQuery();

                    int r = Convert.ToInt32(retorno.Value);

                    if (r == -1)
                        throw new Exception("Ya existe un vuelo con ese código.");

                    if (r == -2)
                        throw new Exception("El estado de partida no existe o está inactivo.");

                    if (r == -3)
                        throw new Exception("El estado de llegada no existe o está inactivo.");

                    if (r == -4)
                        throw new Exception("La fecha/hora de llegada debe ser posterior a la fecha/hora de partida.");

                    if (r == -5)
                        throw new Exception("El estado de partida y el estado de llegada deben ser distintos.");

                    if (r == -6)
                        throw new Exception("El precio del pasaje debe ser mayor a cero.");

                    if (r != 1)
                        throw new Exception("No se pudo dar de alta el vuelo.");
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al dar de alta el vuelo. " + ex.Message);
                }
            }
        }

        public void ModificarVuelo(Vuelo vuelo)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("VueloModificar", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.AddWithValue("@Codigo", vuelo.Codigo);
                comando.Parameters.AddWithValue("@FechaHoraPartida", vuelo.FechaHoraPartida);
                comando.Parameters.AddWithValue("@CodigoEstadoPartida", vuelo.EstadoPartida.Codigo);
                comando.Parameters.AddWithValue("@FechaHoraLlegada", vuelo.FechaHoraLlegada);
                comando.Parameters.AddWithValue("@CodigoEstadoLlegada", vuelo.EstadoLlegada.Codigo);
                comando.Parameters.AddWithValue("@PrecioPasaje", vuelo.PrecioPasaje);
                comando.Parameters.AddWithValue("@Activo", vuelo.Activo);

                SqlParameter retorno = new SqlParameter("@Retorno", SqlDbType.Int);
                retorno.Direction = ParameterDirection.ReturnValue;
                comando.Parameters.Add(retorno);

                try
                {
                    conexion.Open();
                    comando.ExecuteNonQuery();

                    int r = Convert.ToInt32(retorno.Value);

                    if (r == -1)
                        throw new Exception("No existe un vuelo con ese código.");

                    if (r == -2)
                        throw new Exception("El estado de partida no existe o está inactivo.");

                    if (r == -3)
                        throw new Exception("El estado de llegada no existe o está inactivo.");

                    if (r == -4)
                        throw new Exception("La fecha/hora de llegada debe ser posterior a la fecha/hora de partida.");

                    if (r == -5)
                        throw new Exception("El estado de partida y el estado de llegada deben ser distintos.");

                    if (r == -6)
                        throw new Exception("El precio del pasaje debe ser mayor a cero.");

                    if (r != 1)
                        throw new Exception("No se pudo modificar el vuelo.");
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al modificar el vuelo. " + ex.Message);
                }
            }
        }

        public void BajaVuelo(string codigo)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("VueloBaja", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@Codigo", codigo);

                SqlParameter retorno = new SqlParameter("@Retorno", SqlDbType.Int);
                retorno.Direction = ParameterDirection.ReturnValue;
                comando.Parameters.Add(retorno);

                try
                {
                    conexion.Open();
                    comando.ExecuteNonQuery();

                    int r = Convert.ToInt32(retorno.Value);

                    if (r == -1)
                        throw new Exception("No existe un vuelo con ese código.");

                    if (r == 2)
                        return;

                    if (r != 1)
                        throw new Exception("No se pudo eliminar el vuelo.");
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al eliminar el vuelo. " + ex.Message);
                }
            }
        }

        public Vuelo BuscarVuelo(string codigo)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("VueloBuscar", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@Codigo", codigo);

                try
                {
                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            Estado estadoPartida = FabricaP.GetPersistenciaEstado().BuscarEstado(
                                lector["CodigoEstadoPartida"].ToString());

                            Estado estadoLlegada = FabricaP.GetPersistenciaEstado().BuscarEstado(
                                lector["CodigoEstadoLlegada"].ToString());

                            return new Vuelo(
                                lector["Codigo"].ToString(),
                                Convert.ToDateTime(lector["FechaHoraPartida"]),
                                estadoPartida,
                                Convert.ToDateTime(lector["FechaHoraLlegada"]),
                                estadoLlegada,
                                Convert.ToDecimal(lector["PrecioPasaje"]),
                                Convert.ToBoolean(lector["Activo"])
                            );
                        }

                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al buscar el vuelo. " + ex.Message);
                }
            }
        }

        public List<Vuelo> ListarVuelos()
        {
            List<Vuelo> lista = new List<Vuelo>();

            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("VueloListado", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                try
                {
                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            Estado estadoPartida = FabricaP.GetPersistenciaEstado().BuscarEstado(
                                lector["CodigoEstadoPartida"].ToString());

                            Estado estadoLlegada = FabricaP.GetPersistenciaEstado().BuscarEstado(
                                lector["CodigoEstadoLlegada"].ToString());

                            Vuelo vuelo = new Vuelo(
                                lector["Codigo"].ToString(),
                                Convert.ToDateTime(lector["FechaHoraPartida"]),
                                estadoPartida,
                                Convert.ToDateTime(lector["FechaHoraLlegada"]),
                                estadoLlegada,
                                Convert.ToDecimal(lector["PrecioPasaje"]),
                                Convert.ToBoolean(lector["Activo"])
                            );

                            lista.Add(vuelo);
                        }
                    }

                    return lista;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al listar vuelos. " + ex.Message);
                }
            }
        }

        public List<Vuelo> ListarVuelosIdaPorEstado(Estado estadoDestino)
        {
            List<Vuelo> lista = new List<Vuelo>();

            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("VuelosIdaPorEstado", conexion))
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
                            Vuelo vuelo = BuscarVuelo(lector["Codigo"].ToString());
                            lista.Add(vuelo);
                        }
                    }

                    return lista;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al listar vuelos de ida por estado. " + ex.Message);
                }
            }
        }

        public List<Vuelo> ListarVuelosVueltaPorEstado(Estado estadoDestino)
        {
            List<Vuelo> lista = new List<Vuelo>();

            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("VuelosVueltaPorEstado", conexion))
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
                            Vuelo vuelo = BuscarVuelo(lector["Codigo"].ToString());
                            lista.Add(vuelo);
                        }
                    }

                    return lista;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al listar vuelos de vuelta por estado. " + ex.Message);
                }
            }
        }
    }
}