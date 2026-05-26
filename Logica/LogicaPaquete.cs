using System;
using System.Collections.Generic;
using Entidades;
using Persistencia;

namespace Logica
{
    internal class LogicaPaquete : ILPaquete
    {
        private static LogicaPaquete instancia = null;

        private LogicaPaquete() { }

        public static LogicaPaquete GetInstancia()
        {
            if (instancia == null)
                instancia = new LogicaPaquete();

            return instancia;
        }

        public int AltaPaquete(Paquete paquete)
        {
            if (paquete == null)
                throw new Exception("Debe indicar un paquete.");

            if (paquete.Hospedajes == null || paquete.Hospedajes.Count == 0)
                throw new Exception("El paquete debe tener al menos un hospedaje.");

            return FabricaP.GetPersistenciaPaquete().AltaPaquete(paquete);
        }

        public void AgregarHospedajeAPaquete(int codigoPaquete, PaqueteHospedaje paqueteHospedaje)
        {
            if (codigoPaquete <= 0)
                throw new Exception("Debe indicar un código de paquete válido.");

            if (paqueteHospedaje == null)
                throw new Exception("Debe indicar un hospedaje.");

            FabricaP.GetPersistenciaPaquete().AgregarHospedajeAPaquete(
                codigoPaquete,
                paqueteHospedaje
            );
        }

        public Paquete BuscarPaquete(int codigo)
        {
            if (codigo <= 0)
                throw new Exception("Debe indicar un código válido.");

            return FabricaP.GetPersistenciaPaquete().BuscarPaquete(codigo);
        }

        public List<Paquete> ListarPaquetes(string codigoEstado)
        {
            if (codigoEstado == null)
                codigoEstado = "";

            return FabricaP.GetPersistenciaPaquete().ListarPaquetes(codigoEstado);
        }

        public List<Paquete> ListarPaquetesPorHospedaje(Hospedaje hospedaje)
        {
            if (hospedaje == null)
                throw new Exception("Debe indicar un hospedaje.");

            return FabricaP.GetPersistenciaPaquete().ListarPaquetesPorHospedaje(hospedaje);
        }

        public List<PaqueteHospedaje> ListarHospedajesDePaquete(int codigoPaquete)
        {
            if (codigoPaquete <= 0)
                throw new Exception("Debe indicar un código válido.");

            return FabricaP.GetPersistenciaPaquete().ListarHospedajesDePaquete(codigoPaquete);
        }
    }
}