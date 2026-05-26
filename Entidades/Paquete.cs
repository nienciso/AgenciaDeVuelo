using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Paquete
    {
        private int codigo;
        private string titulo;
        private string descripcion;
        private Estado estadoDestino;
        private int cantDias;
        private decimal precioIndividual;
        private decimal precioDoble;
        private decimal precioTriple;
        private Vuelo vueloIda;
        private Vuelo vueloVuelta;
        private Empleado empleado;
        private List<PaqueteHospedaje> hospedajes;
        private bool activo;

        public int Codigo
        {
            get { return codigo; }
            set
            {
                if (value < 0)
                    throw new Exception("El código del paquete no puede ser negativo.");

                codigo = value;
            }
        }

        public string Titulo
        {
            get { return titulo; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Ingrese un título, no puede estar vacío.");

                if (value.Trim().Length > 100)
                    throw new Exception("El título no puede superar los 100 caracteres.");

                titulo = value.Trim();
            }
        }

        public string Descripcion
        {
            get { return descripcion; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Ingrese una descripción, no puede estar vacía.");

                if (value.Trim().Length > 500)
                    throw new Exception("La descripción no puede superar los 500 caracteres.");

                descripcion = value.Trim();
            }
        }

        public Estado EstadoDestino
        {
            get { return estadoDestino; }
            set
            {
                if (value == null)
                    throw new Exception("Debe indicar el estado destino del paquete.");

                estadoDestino = value;
            }
        }

        public int CantDias
        {
            get { return cantDias; }
            set
            {
                if (value <= 0)
                    throw new Exception("La cantidad de días debe ser mayor a cero.");

                cantDias = value;
            }
        }

        public decimal PrecioIndividual
        {
            get { return precioIndividual; }
            set
            {
                if (value <= 0)
                    throw new Exception("El precio individual debe ser mayor a cero.");

                precioIndividual = value;
            }
        }

        public decimal PrecioDoble
        {
            get { return precioDoble; }
            set
            {
                if (value <= 0)
                    throw new Exception("El precio doble debe ser mayor a cero.");

                precioDoble = value;
            }
        }

        public decimal PrecioTriple
        {
            get { return precioTriple; }
            set
            {
                if (value <= 0)
                    throw new Exception("El precio triple debe ser mayor a cero.");

                precioTriple = value;
            }
        }

        public Vuelo VueloIda
        {
            get { return vueloIda; }
            set
            {
                if (value == null)
                    throw new Exception("Debe indicar el vuelo de ida.");

                vueloIda = value;
            }
        }

        public Vuelo VueloVuelta
        {
            get { return vueloVuelta; }
            set
            {
                if (value == null)
                    throw new Exception("Debe indicar el vuelo de vuelta.");

                if (VueloIda != null && value.Codigo == VueloIda.Codigo)
                    throw new Exception("El vuelo de vuelta debe ser diferente al vuelo de ida.");

                vueloVuelta = value;
            }
        }

        public Empleado Empleado
        {
            get { return empleado; }
            set
            {
                if (value == null)
                    throw new Exception("Debe indicar el empleado que generó el paquete.");

                empleado = value;
            }
        }

        public List<PaqueteHospedaje> Hospedajes
        {
            get { return hospedajes; }
            set
            {
                if (value == null)
                    throw new Exception("Debe indicar los hospedajes del paquete.");

                if (value.Count == 0)
                    throw new Exception("El paquete debe tener al menos un hospedaje.");

                hospedajes = value;
            }
        }

        public bool Activo
        {
            get { return activo; }
            set { activo = value; }
        }

        public Paquete(
            int codigo,
            string titulo,
            string descripcion,
            Estado estadoDestino,
            int cantDias,
            decimal precioIndividual,
            decimal precioDoble,
            decimal precioTriple,
            Vuelo vueloIda,
            Vuelo vueloVuelta,
            Empleado empleado,
            List<PaqueteHospedaje> hospedajes,
            bool activo)
        {
            Codigo = codigo;
            Titulo = titulo;
            Descripcion = descripcion;
            EstadoDestino = estadoDestino;
            CantDias = cantDias;
            PrecioIndividual = precioIndividual;
            PrecioDoble = precioDoble;
            PrecioTriple = precioTriple;
            VueloIda = vueloIda;
            VueloVuelta = vueloVuelta;
            Empleado = empleado;
            Hospedajes = hospedajes;
            Activo = activo;
        }

        public override string ToString()
        {
            return "\n - Código: " + Codigo +
                   "\n - Título: " + Titulo +
                   "\n - Estado destino: " + EstadoDestino.Nombre +
                   "\n - Cantidad de días: " + CantDias +
                   "\n - Precio individual: " + PrecioIndividual +
                   "\n - Precio doble: " + PrecioDoble +
                   "\n - Precio triple: " + PrecioTriple +
                   "\n - Vuelo ida: " + VueloIda.Codigo +
                   "\n - Vuelo vuelta: " + VueloVuelta.Codigo +
                   "\n - Empleado: " + Empleado.Usuario +
                   "\n - Activo: " + Activo;
        }
    }
}
