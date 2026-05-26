using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;

namespace Logica
{
    public interface ILPaquete
    {
        int AltaPaquete(Paquete paquete);

        void AgregarHospedajeAPaquete(int codigoPaquete, PaqueteHospedaje paqueteHospedaje);

        Paquete BuscarPaquete(int codigo);

        List<Paquete> ListarPaquetes(string codigoEstado);

        List<Paquete> ListarPaquetesPorHospedaje(Hospedaje hospedaje);

        List<PaqueteHospedaje> ListarHospedajesDePaquete(int codigoPaquete);
    }
}
