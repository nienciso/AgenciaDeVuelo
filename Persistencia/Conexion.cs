using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia
{
    public class Conexion
    {
        private static string _cnn =
            @"Data Source=DESKTOP-75MO4J9\SQLEXPRESS;
              Initial Catalog=AgenciaDeVuelo;
              Integrated Security=true;";

        public static string Cnn
        {
            get { return _cnn; }
        }
    }
}
