using System;
using System.Collections.Generic;
using Entidades;
using Persistencia;

namespace Logica
{
    internal class LogicaVuelo : ILVuelo
    {
        private static LogicaVuelo instancia = null;

        private LogicaVuelo() { }

        public static LogicaVuelo GetInstancia()
        {
            if (instancia == null)
                instancia = new LogicaVuelo();

            return instancia;
        }

        public void AltaVuelo(Vuelo vuelo)
        {
            if (vuelo == null)
                throw new Exception("Debe indicar un vuelo.");

            FabricaP.GetPersistenciaVuelo().AltaVuelo(vuelo);
        }

        public void ModificarVuelo(Vuelo vuelo)
        {
            if (vuelo == null)
                throw new Exception("Debe indicar un vuelo.");

            FabricaP.GetPersistenciaVuelo().ModificarVuelo(vuelo);
        }

        public void BajaVuelo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new Exception("Debe indicar un código de vuelo.");

            FabricaP.GetPersistenciaVuelo().BajaVuelo(codigo);
        }

        public Vuelo BuscarVuelo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new Exception("Debe indicar un código de vuelo.");

            return FabricaP.GetPersistenciaVuelo().BuscarVuelo(codigo);
        }

        public List<Vuelo> ListarVuelos()
        {
            return FabricaP.GetPersistenciaVuelo().ListarVuelos();
        }

        public List<Vuelo> ListarVuelosIdaPorEstado(Estado estadoDestino)
        {
            if (estadoDestino == null)
                throw new Exception("Debe indicar un estado destino.");

            return FabricaP.GetPersistenciaVuelo().ListarVuelosIdaPorEstado(estadoDestino);
        }

        public List<Vuelo> ListarVuelosVueltaPorEstado(Estado estadoDestino)
        {
            if (estadoDestino == null)
                throw new Exception("Debe indicar un estado destino.");

            return FabricaP.GetPersistenciaVuelo().ListarVuelosVueltaPorEstado(estadoDestino);
        }
    }
}