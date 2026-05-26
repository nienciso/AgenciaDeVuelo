using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using Persistencia;

namespace Logica
{
    public class FabricaL
    {
        public static ILEmpleado GetLogicaEmpleado()
        {
            return LogicaEmpleado.GetInstancia();
        }

        public static ILVuelo GetLogicaVuelo()
        {
            return LogicaVuelo.GetInstancia();
        }

        public static ILPaquete GetLogicaPaquete()
        {
            return LogicaPaquete.GetInstancia();
        }

        public static ILHospedaje GetLogicaHospedaje()
        {
            return LogicaHospedaje.GetInstancia();
        }

        public static ILEstado GetLogicaEstado()
        {
            return LogicaEstado.GetInstancia();
        }

    }
}
