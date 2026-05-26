using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Entidades;

namespace Persistencia
{
    internal class PersistenciaPaquete : IPPaquete
    {
        private static PersistenciaPaquete instancia = null;

        private PersistenciaPaquete() { }

        public static PersistenciaPaquete GetInstancia()
        {
            if (instancia == null)
                instancia = new PersistenciaPaquete();

            return instancia;
        }

        public int AltaPaquete(Paquete paquete)
        {
            if (paquete.Hospedajes == null || paquete.Hospedajes.Count == 0)
                throw new Exception("El paquete debe tener al menos un hospedaje.");

            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            {
                try
                {
                    conexion.Open();

                    PaqueteHospedaje primerHospedaje = paquete.Hospedajes[0];

                    SqlCommand comando = new SqlCommand("PaqueteAlta", conexion);
                    comando.CommandType = CommandType.StoredProcedure;

                    comando.Parameters.AddWithValue("@Titulo", paquete.Titulo);
                    comando.Parameters.AddWithValue("@Descripcion", paquete.Descripcion);
                    comando.Parameters.AddWithValue("@CodigoEstadoDestino", paquete.EstadoDestino.Codigo);
                    comando.Parameters.AddWithValue("@CodigoVueloIda", paquete.VueloIda.Codigo);
                    comando.Parameters.AddWithValue("@CodigoVueloVuelta", paquete.VueloVuelta.Codigo);
                    comando.Parameters.AddWithValue("@UsuarioEmpleado", paquete.Empleado.Usuario);
                    comando.Parameters.AddWithValue("@CodigoHospedaje", primerHospedaje.Hospedaje.CodigoInterno);
                    comando.Parameters.AddWithValue("@CantNoches", primerHospedaje.CantNoches);

                    SqlParameter retorno = new SqlParameter("@Retorno", SqlDbType.Int);
                    retorno.Direction = ParameterDirection.ReturnValue;
                    comando.Parameters.Add(retorno);

                    comando.ExecuteNonQuery();

                    int r = Convert.ToInt32(retorno.Value);

                    if (r == -1)
                        throw new Exception("El estado destino no existe o está inactivo.");
                    if (r == -2)
                        throw new Exception("El vuelo de ida no existe o está inactivo.");
                    if (r == -3)
                        throw new Exception("El vuelo de vuelta no existe o está inactivo.");
                    if (r == -4)
                        throw new Exception("El empleado no existe o está inactivo.");
                    if (r == -5)
                        throw new Exception("El hospedaje no existe o está inactivo.");
                    if (r == -6)
                        throw new Exception("La cantidad de noches debe ser mayor a cero.");
                    if (r == -7)
                        throw new Exception("El vuelo de ida no llega al estado destino.");
                    if (r == -8)
                        throw new Exception("El vuelo de vuelta no sale del estado destino.");
                    if (r == -9)
                        throw new Exception("El hospedaje no pertenece al estado destino.");
                    if (r == -10)
                        throw new Exception("La fecha del vuelo de vuelta debe ser posterior a la fecha del vuelo de ida.");
                    if (r <= 0)
                        throw new Exception("No se pudo dar de alta el paquete.");

                    int codigoPaquete = r;

                    for (int i = 1; i < paquete.Hospedajes.Count; i++)
                    {
                        AgregarHospedajeAPaquete(codigoPaquete, paquete.Hospedajes[i]);
                    }

                    return codigoPaquete;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al dar de alta el paquete. " + ex.Message);
                }
            }
        }

        public void AgregarHospedajeAPaquete(int codigoPaquete, PaqueteHospedaje paqueteHospedaje)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("PaqueteHospedajeAgregar", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.AddWithValue("@CodigoPaquete", codigoPaquete);
                comando.Parameters.AddWithValue("@CodigoHospedaje", paqueteHospedaje.Hospedaje.CodigoInterno);
                comando.Parameters.AddWithValue("@CantNoches", paqueteHospedaje.CantNoches);

                SqlParameter retorno = new SqlParameter("@Retorno", SqlDbType.Int);
                retorno.Direction = ParameterDirection.ReturnValue;
                comando.Parameters.Add(retorno);

                try
                {
                    conexion.Open();
                    comando.ExecuteNonQuery();

                    int r = Convert.ToInt32(retorno.Value);

                    if (r == -1)
                        throw new Exception("El paquete no existe o está inactivo.");
                    if (r == -2)
                        throw new Exception("El hospedaje no existe o está inactivo.");
                    if (r == -3)
                        throw new Exception("Ese hospedaje ya fue agregado al paquete.");
                    if (r == -4)
                        throw new Exception("La cantidad de noches debe ser mayor a cero.");
                    if (r == -5)
                        throw new Exception("El hospedaje no pertenece al estado destino del paquete.");
                    if (r != 1)
                        throw new Exception("No se pudo agregar el hospedaje al paquete.");
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al agregar hospedaje al paquete. " + ex.Message);
                }
            }
        }

        public Paquete BuscarPaquete(int codigo)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("PaqueteBuscar", conexion))
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
                            Estado estadoDestino = FabricaP.GetPersistenciaEstado().BuscarEstado(
                                lector["CodigoEstadoDestino"].ToString());

                            Vuelo vueloIda = FabricaP.GetPersistenciaVuelo().BuscarVuelo(
                                lector["CodigoVueloIda"].ToString());

                            Vuelo vueloVuelta = FabricaP.GetPersistenciaVuelo().BuscarVuelo(
                                lector["CodigoVueloVuelta"].ToString());

                            Empleado empleado = FabricaP.GetPersistenciaEmpleado().BuscarEmpleado(
                                lector["UsuarioEmpleado"].ToString());

                            List<PaqueteHospedaje> hospedajes = ListarHospedajesDePaquete(codigo);

                            return new Paquete(
                                Convert.ToInt32(lector["Codigo"]),
                                lector["Titulo"].ToString(),
                                lector["Descripcion"].ToString(),
                                estadoDestino,
                                Convert.ToInt32(lector["CantDias"]),
                                Convert.ToDecimal(lector["PrecioIndividual"]),
                                Convert.ToDecimal(lector["PrecioDoble"]),
                                Convert.ToDecimal(lector["PrecioTriple"]),
                                vueloIda,
                                vueloVuelta,
                                empleado,
                                hospedajes,
                                Convert.ToBoolean(lector["Activo"])
                            );
                        }

                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al buscar el paquete. " + ex.Message);
                }
            }
        }

        public List<PaqueteHospedaje> ListarHospedajesDePaquete(int codigoPaquete)
        {
            List<PaqueteHospedaje> lista = new List<PaqueteHospedaje>();

            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("PaqueteDetalleHospedajes", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@CodigoPaquete", codigoPaquete);

                try
                {
                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            Hospedaje hospedaje = FabricaP.GetPersistenciaHospedaje().BuscarHospedaje(
                                lector["CodigoInterno"].ToString());

                            PaqueteHospedaje ph = new PaqueteHospedaje(
                                hospedaje,
                                Convert.ToInt32(lector["CantNoches"])
                            );

                            lista.Add(ph);
                        }
                    }

                    return lista;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al listar hospedajes del paquete. " + ex.Message);
                }
            }
        }

        public List<Paquete> ListarPaquetes(string codigoEstado)
        {
            List<Paquete> lista = new List<Paquete>();

            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("PaqueteListadoGeneral", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                if (codigoEstado == null)
                    codigoEstado = "";

                comando.Parameters.AddWithValue("@CodigoEstado", codigoEstado);

                try
                {
                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            int codigo = Convert.ToInt32(lector["Codigo"]);

                            Paquete paquete = BuscarPaquete(codigo);

                            if (paquete != null)
                                lista.Add(paquete);
                        }
                    }

                    return lista;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al listar paquetes. " + ex.Message);
                }
            }
        }

        public List<Paquete> ListarPaquetesPorHospedaje(Hospedaje hospedaje)
        {
            List<Paquete> lista = new List<Paquete>();

            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("PaqueteListadoPorHospedaje", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@CodigoHospedaje", hospedaje.CodigoInterno);

                try
                {
                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            int codigo = Convert.ToInt32(lector["Codigo"]);

                            Paquete paquete = BuscarPaquete(codigo);

                            if (paquete != null)
                                lista.Add(paquete);
                        }
                    }

                    return lista;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al listar paquetes por hospedaje. " + ex.Message);
                }
            }
        }
    }
}