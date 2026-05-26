using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia
{
    public class FabricaP
    {
        public static IPVuelo GetPersistenciaVuelo()
        {
            return PersistenciaVuelo.GetInstancia();
        
        }

        public static IPPaquete GetPersistenciaPaquete()
        {
            return PersistenciaPaquete.GetInstancia();
        }

        public static IPHospedaje GetPersistenciaHospedaje()
        {
            return PersistenciaHospedaje.GetInstancia();
        }

        public static IPEstado GetPersistenciaEstado()
        {
            return PersistenciaEstado.GetInstancia();
        }
        public static IPEmpleado GetPersistenciaEmpleado()
        {
            return PersistenciaEmpleado.GetInstancia();
        }

    }
}
