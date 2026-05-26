using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Hospedaje
    {
        private string codigoInterno;
        private string nombre;
        private string direccion;
        private string tipoHospedaje;
        private decimal precioNochePersona;
        private Estado estado;
        private bool activo;

        public string CodigoInterno
        {
            get { return codigoInterno; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Ingrese un código interno, no puede estar vacío.");

                value = value.Trim().ToUpper();

                if (value.Length > 10)
                    throw new Exception("El código interno no puede superar los 10 caracteres.");

                foreach (char c in value)
                {
                    if (!char.IsLetter(c))
                        throw new Exception("El código interno solo puede contener letras.");
                }

                codigoInterno = value;
            }
        }

        public string Nombre
        {
            get { return nombre; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Ingrese un nombre, no puede estar vacío.");

                if (value.Trim().Length > 80)
                    throw new Exception("El nombre no puede superar los 80 caracteres.");

                nombre = value.Trim();
            }
        }

        public string Direccion
        {
            get { return direccion; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Ingrese una dirección, no puede estar vacía.");

                if (value.Trim().Length > 150)
                    throw new Exception("La dirección no puede superar los 150 caracteres.");

                direccion = value.Trim();
            }
        }

        public string TipoHospedaje
        {
            get { return tipoHospedaje; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Ingrese un tipo de hospedaje.");

                value = value.Trim();

                if (value != "Hotel STD" && value != "Posada" && value != "All Inclusive")
                    throw new Exception("El tipo de hospedaje debe ser Hotel STD, Posada o All Inclusive.");

                tipoHospedaje = value;
            }
        }

        public decimal PrecioNochePersona
        {
            get { return precioNochePersona; }
            set
            {
                if (value <= 0)
                    throw new Exception("El precio por noche por persona debe ser mayor a cero.");

                precioNochePersona = value;
            }
        }

        public Estado Estado
        {
            get { return estado; }
            set
            {
                if (value == null)
                    throw new Exception("Debe indicar el estado del hospedaje.");

                estado = value;
            }
        }

        public bool Activo
        {
            get { return activo; }
            set { activo = value; }
        }

        public Hospedaje(
            string codigoInterno,
            string nombre,
            string direccion,
            string tipoHospedaje,
            decimal precioNochePersona,
            Estado estado,
            bool activo)
        {
            CodigoInterno = codigoInterno;
            Nombre = nombre;
            Direccion = direccion;
            TipoHospedaje = tipoHospedaje;
            PrecioNochePersona = precioNochePersona;
            Estado = estado;
            Activo = activo;
        }

        public override string ToString()
        {
            return "\n - Código interno: " + CodigoInterno +
                   "\n - Nombre: " + Nombre +
                   "\n - Dirección: " + Direccion +
                   "\n - Tipo: " + TipoHospedaje +
                   "\n - Precio noche/persona: " + PrecioNochePersona +
                   "\n - Estado: " + Estado.Nombre +
                   "\n - Activo: " + Activo;
        }
    }
}
