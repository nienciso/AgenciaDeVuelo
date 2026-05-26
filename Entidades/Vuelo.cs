using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Vuelo
    {
        private string codigo;
        private DateTime fechaHoraPartida;
        private Estado estadoPartida;
        private DateTime fechaHoraLlegada;
        private Estado estadoLlegada;
        private decimal precioPasaje;
        private bool activo;

        public string Codigo
        {
            get { return codigo; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Ingrese un código, no puede estar vacío.");

                value = value.Trim().ToUpper();

                if (value.Length != 10)
                    throw new Exception("El código del vuelo debe tener exactamente 10 caracteres.");

                codigo = value;
            }
        }

        public DateTime FechaHoraPartida
        {
            get { return fechaHoraPartida; }
            set { fechaHoraPartida = value; }
        }

        public Estado EstadoPartida
        {
            get { return estadoPartida; }
            set
            {
                if (value == null)
                    throw new Exception("Debe indicar el estado de partida.");

                estadoPartida = value;
            }
        }

        public DateTime FechaHoraLlegada
        {
            get { return fechaHoraLlegada; }
            set
            {
                if (value <= FechaHoraPartida)
                    throw new Exception("La fecha/hora de llegada debe ser posterior a la fecha/hora de partida.");

                fechaHoraLlegada = value;
            }
        }

        public Estado EstadoLlegada
        {
            get { return estadoLlegada; }
            set
            {
                if (value == null)
                    throw new Exception("Debe indicar el estado de llegada.");

                if (EstadoPartida != null && value.Codigo == EstadoPartida.Codigo)
                    throw new Exception("El estado de llegada debe ser diferente al estado de partida.");

                estadoLlegada = value;
            }
        }

        public decimal PrecioPasaje
        {
            get { return precioPasaje; }
            set
            {
                if (value <= 0)
                    throw new Exception("El precio del pasaje debe ser mayor a cero.");

                precioPasaje = value;
            }
        }

        public bool Activo
        {
            get { return activo; }
            set { activo = value; }
        }

        public Vuelo(
            string codigo,
            DateTime fechaHoraPartida,
            Estado estadoPartida,
            DateTime fechaHoraLlegada,
            Estado estadoLlegada,
            decimal precioPasaje,
            bool activo)
        {
            Codigo = codigo;
            FechaHoraPartida = fechaHoraPartida;
            EstadoPartida = estadoPartida;
            FechaHoraLlegada = fechaHoraLlegada;
            EstadoLlegada = estadoLlegada;
            PrecioPasaje = precioPasaje;
            Activo = activo;
        }

        public override string ToString()
        {
            return "\n - Código: " + Codigo +
                   "\n - Fecha/hora partida: " + FechaHoraPartida +
                   "\n - Estado partida: " + EstadoPartida.Nombre +
                   "\n - Fecha/hora llegada: " + FechaHoraLlegada +
                   "\n - Estado llegada: " + EstadoLlegada.Nombre +
                   "\n - Precio pasaje: " + PrecioPasaje +
                   "\n - Activo: " + Activo;
        }
    }
}
