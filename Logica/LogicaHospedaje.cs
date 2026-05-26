using System;
using System.Collections.Generic;
using Entidades;
using Persistencia;

namespace Logica
{
    internal class LogicaHospedaje : ILHospedaje
    {
        private static LogicaHospedaje instancia = null;

        private LogicaHospedaje() { }

        public static LogicaHospedaje GetInstancia()
        {
            if (instancia == null)
                instancia = new LogicaHospedaje();

            return instancia;
        }

        public void AltaHospedaje(Hospedaje hospedaje)
        {
            if (hospedaje == null)
                throw new Exception("Debe indicar un hospedaje.");

            FabricaP.GetPersistenciaHospedaje().AltaHospedaje(hospedaje);
        }

        public void ModificarHospedaje(Hospedaje hospedaje)
        {
            if (hospedaje == null)
                throw new Exception("Debe indicar un hospedaje.");

            FabricaP.GetPersistenciaHospedaje().ModificarHospedaje(hospedaje);
        }

        public void BajaHospedaje(string codigoInterno)
        {
            if (string.IsNullOrWhiteSpace(codigoInterno))
                throw new Exception("Debe indicar un código interno.");

            FabricaP.GetPersistenciaHospedaje().BajaHospedaje(codigoInterno);
        }

        public Hospedaje BuscarHospedaje(string codigoInterno)
        {
            if (string.IsNullOrWhiteSpace(codigoInterno))
                throw new Exception("Debe indicar un código interno.");

            return FabricaP.GetPersistenciaHospedaje().BuscarHospedaje(codigoInterno);
        }

        public List<Hospedaje> ListarHospedajes(string filtroNombre)
        {
            if (filtroNombre == null)
                filtroNombre = "";

            return FabricaP.GetPersistenciaHospedaje().ListarHospedajes(filtroNombre);
        }

        public List<Hospedaje> ListarHospedajesPorEstado(Estado estadoDestino)
        {
            if (estadoDestino == null)
                throw new Exception("Debe indicar un estado.");

            return FabricaP.GetPersistenciaHospedaje().ListarHospedajesPorEstado(estadoDestino);
        }
    }
}