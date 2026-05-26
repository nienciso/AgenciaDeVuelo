using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Entidades;

namespace Persistencia
{
    internal class PersistenciaEmpleado : IPEmpleado
    {
        private static PersistenciaEmpleado instancia = null;

        private PersistenciaEmpleado() { }

        public static PersistenciaEmpleado GetInstancia()
        {
            if (instancia == null)
                instancia = new PersistenciaEmpleado();

            return instancia;
        }

        public void AltaEmpleado(Empleado emp)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("EmpleadoAlta", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.AddWithValue("@Usuario", emp.Usuario);
                comando.Parameters.AddWithValue("@Contrasenia", emp.Contrasenia);
                comando.Parameters.AddWithValue("@NombreCompleto", emp.NombreCompleto);

                SqlParameter retorno = new SqlParameter("@Retorno", SqlDbType.Int);
                retorno.Direction = ParameterDirection.ReturnValue;
                comando.Parameters.Add(retorno);

                try
                {
                    conexion.Open();
                    comando.ExecuteNonQuery();

                    int r = Convert.ToInt32(retorno.Value);

                    if (r == -1)
                        throw new Exception("Ya existe un empleado con ese usuario.");

                    if (r == -2)
                        throw new Exception("La contraseña debe tener entre 5 y 10 caracteres.");

                    if (r == -3)
                        throw new Exception("La contraseña debe tener al menos una letra.");

                    if (r == -4)
                        throw new Exception("La contraseña debe tener al menos un número.");

                    if (r == -5)
                        throw new Exception("La contraseña debe tener al menos un carácter especial.");

                    if (r != 1)
                        throw new Exception("No se pudo dar de alta el empleado.");
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al dar de alta el empleado. " + ex.Message);
                }
            }
        }

        public void ModificarEmpleado(Empleado emp)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("EmpleadoModificar", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.AddWithValue("@Usuario", emp.Usuario);
                comando.Parameters.AddWithValue("@Contrasenia", emp.Contrasenia);
                comando.Parameters.AddWithValue("@NombreCompleto", emp.NombreCompleto);
                comando.Parameters.AddWithValue("@Activo", emp.Activo);

                SqlParameter retorno = new SqlParameter("@Retorno", SqlDbType.Int);
                retorno.Direction = ParameterDirection.ReturnValue;
                comando.Parameters.Add(retorno);

                try
                {
                    conexion.Open();
                    comando.ExecuteNonQuery();

                    int r = Convert.ToInt32(retorno.Value);

                    if (r == -1)
                        throw new Exception("No existe un empleado con ese usuario.");

                    if (r == -2)
                        throw new Exception("La contraseña debe tener entre 5 y 10 caracteres.");

                    if (r == -3)
                        throw new Exception("La contraseña debe tener al menos una letra.");

                    if (r == -4)
                        throw new Exception("La contraseña debe tener al menos un número.");

                    if (r == -5)
                        throw new Exception("La contraseña debe tener al menos un carácter especial.");

                    if (r != 1)
                        throw new Exception("No se pudo modificar el empleado.");
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al modificar el empleado. " + ex.Message);
                }
            }
        }

        public Empleado BuscarEmpleado(string usuario)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("EmpleadoBuscar", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@Usuario", usuario);

                try
                {
                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            return new Empleado(
                                lector["Usuario"].ToString(),
                                lector["Contrasenia"].ToString(),
                                lector["NombreCompleto"].ToString(),
                                Convert.ToBoolean(lector["Activo"])
                            );
                        }

                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al buscar el empleado. " + ex.Message);
                }
            }
        }

        public Empleado LogueoEmpleado(string usuario, string contrasenia)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("EmpleadoLogueo", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.AddWithValue("@Usuario", usuario);
                comando.Parameters.AddWithValue("@Contrasenia", contrasenia);

                try
                {
                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            return new Empleado(
                                lector["Usuario"].ToString(),
                                contrasenia,
                                lector["NombreCompleto"].ToString(),
                                Convert.ToBoolean(lector["Activo"])
                            );
                        }

                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al loguear el empleado. " + ex.Message);
                }
            }
        }

        public List<Empleado> ListarEmpleados()
        {
            List<Empleado> lista = new List<Empleado>();

            using (SqlConnection conexion = new SqlConnection(Conexion.Cnn))
            using (SqlCommand comando = new SqlCommand("EmpleadoListado", conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;

                try
                {
                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            Empleado emp = new Empleado(
                                lector["Usuario"].ToString(),
                                lector["Contrasenia"].ToString(),
                                lector["NombreCompleto"].ToString(),
                                Convert.ToBoolean(lector["Activo"])
                            );

                            lista.Add(emp);
                        }
                    }

                    return lista;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al listar empleados. " + ex.Message);
                }
            }
        }
    }
}