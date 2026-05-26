using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class PaqueteHospedaje
    {
        private Hospedaje hospedaje;
        private int cantNoches;

        public Hospedaje Hospedaje
        {
            get { return hospedaje; }
            set
            {
                if (value == null)
                    throw new Exception("Debe indicar un hospedaje.");

                hospedaje = value;
            }
        }

        public int CantNoches
        {
            get { return cantNoches; }
            set
            {
                if (value <= 0)
                    throw new Exception("La cantidad de noches debe ser mayor a cero.");

                cantNoches = value;
            }
        }

        public PaqueteHospedaje(Hospedaje hospedaje, int cantNoches)
        {
            Hospedaje = hospedaje;
            CantNoches = cantNoches;
        }

        public override string ToString()
        {
            return "\n - Hospedaje: " + Hospedaje.Nombre +
                   "\n - Cantidad de noches: " + CantNoches;
        }
    }
}