using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Entidades;

namespace Persistencia
{
    internal class PersistenciaEstado : IPEstado
    {
        private static PersistenciaEstado instancia = null;

        private PersistenciaEstado() { }

        public static PersistenciaEstado GetInstancia()
        {
            if (instancia == null)
                instancia = new PersistenciaEstado();

            return instancia;
        }

        public void AltaEstado(Estado estado)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("EstadoAlta", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.AddWithValue("@Codigo", estado.Codigo);
                comando.Parameters.AddWithValue("@Nombre", estado.Nombre);
                comando.Parameters.AddWithValue("@Pais", estado.Pais);

                SqlParameter retorno = new SqlParameter("@Retorno", SqlDbType.Int);
                retorno.Direction = ParameterDirection.ReturnValue;
                comando.Parameters.Add(retorno);

                try
                {
                    conexion.Open();
                    comando.ExecuteNonQuery();

                    int r = Convert.ToInt32(retorno.Value);

                    if (r == -1)
                        throw new Exception("Ya existe un estado con ese código.");

                    if (r != 1)
                        throw new Exception("No se pudo dar de alta el estado.");
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al dar de alta el estado. " + ex.Message);
                }
            }
        }

        public void ModificarEstado(Estado estado)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("EstadoModificar", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.AddWithValue("@Codigo", estado.Codigo);
                comando.Parameters.AddWithValue("@Nombre", estado.Nombre);
                comando.Parameters.AddWithValue("@Pais", estado.Pais);
                comando.Parameters.AddWithValue("@Activo", estado.Activo);

                SqlParameter retorno = new SqlParameter("@Retorno", SqlDbType.Int);
                retorno.Direction = ParameterDirection.ReturnValue;
                comando.Parameters.Add(retorno);

                try
                {
                    conexion.Open();
                    comando.ExecuteNonQuery();

                    int r = Convert.ToInt32(retorno.Value);

                    if (r == -1)
                        throw new Exception("No existe un estado con ese código.");

                    if (r != 1)
                        throw new Exception("No se pudo modificar el estado.");
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al modificar el estado. " + ex.Message);
                }
            }
        }

        public void BajaEstado(string codigo)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("EstadoBaja", conexion))
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
                        throw new Exception("No existe un estado con ese código.");

                    if (r == 2)
                        return;

                    if (r != 1)
                        throw new Exception("No se pudo eliminar el estado.");
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al eliminar el estado. " + ex.Message);
                }
            }
        }

        public Estado BuscarEstado(string codigo)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("EstadoBuscar", conexion))
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
                            return new Estado(
                                lector["Codigo"].ToString(),
                                lector["Nombre"].ToString(),
                                lector["Pais"].ToString(),
                                Convert.ToBoolean(lector["Activo"])
                            );
                        }

                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al buscar el estado. " + ex.Message);
                }
            }
        }

        public List<Estado> ListarEstados(string filtroNombre)
        {
            List<Estado> lista = new List<Estado>();

            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("EstadoListado", conexion))
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
                            Estado estado = new Estado(
                                lector["Codigo"].ToString(),
                                lector["Nombre"].ToString(),
                                lector["Pais"].ToString(),
                                Convert.ToBoolean(lector["Activo"])
                            );

                            lista.Add(estado);
                        }
                    }

                    return lista;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al listar estados. " + ex.Message);
                }
            }
        }
    }
}