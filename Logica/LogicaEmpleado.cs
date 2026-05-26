using System;
using System.Collections.Generic;
using Entidades;
using Persistencia;


namespace Logica
{
    internal class LogicaEmpleado : ILEmpleado
    {
        private static LogicaEmpleado instancia = null;

        private LogicaEmpleado() { }

        public static LogicaEmpleado GetInstancia()
        {
            if (instancia == null)
                instancia = new LogicaEmpleado();

            return instancia;
        }

        public void AltaEmpleado(Empleado emp)
        {
            if (emp == null)
                throw new Exception("Debe indicar un empleado.");

            FabricaP.GetPersistenciaEmpleado().AltaEmpleado(emp);
        }

        public void ModificarEmpleado(Empleado emp)
        {
            if (emp == null)
                throw new Exception("Debe indicar un empleado.");

            FabricaP.GetPersistenciaEmpleado().ModificarEmpleado(emp);
        }

        public Empleado BuscarEmpleado(string usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                throw new Exception("Debe indicar un usuario.");

            return FabricaP.GetPersistenciaEmpleado().BuscarEmpleado(usuario);
        }

        public Empleado LogueoEmpleado(string usuario, string contrasenia)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                throw new Exception("Debe ingresar el usuario.");

            if (string.IsNullOrWhiteSpace(contrasenia))
                throw new Exception("Debe ingresar la contraseña.");

            Empleado emp = FabricaP.GetPersistenciaEmpleado().LogueoEmpleado(usuario, contrasenia);

            if (emp == null)
                throw new Exception("Usuario o contraseña incorrectos.");

            return emp;
        }

        public List<Empleado> ListarEmpleados()
        {
            return FabricaP.GetPersistenciaEmpleado().ListarEmpleados();
        }
    }
}