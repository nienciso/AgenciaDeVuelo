using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;


namespace Persistencia
{
    public interface IPEstado
    {
            void AltaEstado(Estado estado);

            void ModificarEstado(Estado estado);

            void BajaEstado(string codigo);

            Estado BuscarEstado(string codigo);

            List<Estado> ListarEstados(string filtroNombre);
    }
}
