using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;

namespace Persistencia
{
    public interface IPEmpleado
    {
        void AltaEmpleado(Empleado emp);

        void ModificarEmpleado(Empleado emp);

        Empleado BuscarEmpleado(string usuario);

        Empleado LogueoEmpleado(string usuario, string contrasenia);

        List<Empleado> ListarEmpleados();
    }
}
