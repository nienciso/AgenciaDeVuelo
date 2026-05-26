using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;

namespace Persistencia
{
    public interface IPPaquete
    {
        int AltaPaquete(Paquete paquete);

        void AgregarHospedajeAPaquete(int codigoPaquete, PaqueteHospedaje paqueteHospedaje);

        Paquete BuscarPaquete(int codigo);

        List<PaqueteHospedaje> ListarHospedajesDePaquete(int codigoPaquete);

        List<Paquete> ListarPaquetes(string codigoEstado);

        List<Paquete> ListarPaquetesPorHospedaje(Hospedaje hospedaje);
    }
}
