using System;
using System.Collections.Generic;
using Entidades;
using Persistencia;

namespace Logica
{
    internal class LogicaEstado : ILEstado
    {
        private static LogicaEstado instancia = null;

        private LogicaEstado() { }

        public static LogicaEstado GetInstancia()
        {
            if (instancia == null)
                instancia = new LogicaEstado();

            return instancia;
        }

        public void AltaEstado(Estado estado)
        {
            if (estado == null)
                throw new Exception("Debe indicar un estado.");

            FabricaP.GetPersistenciaEstado().AltaEstado(estado);
        }

        public void ModificarEstado(Estado estado)
        {
            if (estado == null)
                throw new Exception("Debe indicar un estado.");

            FabricaP.GetPersistenciaEstado().ModificarEstado(estado);
        }

        public void BajaEstado(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new Exception("Debe indicar un código.");

            FabricaP.GetPersistenciaEstado().BajaEstado(codigo);
        }

        public Estado BuscarEstado(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new Exception("Debe indicar un código.");

            return FabricaP.GetPersistenciaEstado().BuscarEstado(codigo);
        }

        public List<Estado> ListarEstados(string filtroNombre)
        {
            if (filtroNombre == null)
                filtroNombre = "";

            return FabricaP.GetPersistenciaEstado().ListarEstados(filtroNombre);
        }
    }
}