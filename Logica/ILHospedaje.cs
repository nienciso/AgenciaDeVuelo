using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;

namespace Logica
{
    public interface ILHospedaje
    {
        void AltaHospedaje(Hospedaje hospedaje);

        void ModificarHospedaje(Hospedaje hospedaje);

        void BajaHospedaje(string codigoInterno);

        Hospedaje BuscarHospedaje(string codigoInterno);

        List<Hospedaje> ListarHospedajes(string filtroNombre);

        List<Hospedaje> ListarHospedajesPorEstado(Estado estadoDestino);
    }
}
