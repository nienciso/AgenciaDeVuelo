using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;


namespace Persistencia
{
    public interface IPVuelo
    {
        void AltaVuelo(Vuelo vuelo);

        void ModificarVuelo(Vuelo vuelo);

        void BajaVuelo(string codigo);

        Vuelo BuscarVuelo(string codigo);

        List<Vuelo> ListarVuelos();

        List<Vuelo> ListarVuelosIdaPorEstado(Estado estadoDestino);

        List<Vuelo> ListarVuelosVueltaPorEstado(Estado estadoDestino);
    }
}
