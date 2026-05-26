using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Estado
    {
        private string codigo;
        private string nombre;
        private string pais;
        private bool activo;

        public string Codigo
        {
            get { return codigo; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Ingrese un código, no puede estar vacío.");

                value = value.Trim().ToUpper();

                if (value.Length < 4 || value.Length > 5)
                    throw new Exception("El código debe tener entre 4 y 5 letras.");

                foreach (char c in value)
                {
                    if (!char.IsLetter(c))
                        throw new Exception("El código solo puede contener letras.");
                }

                codigo = value;
            }
        }

        public string Nombre
        {
            get { return nombre; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Ingrese un nombre, no puede estar vacío.");

                if (value.Trim().Length > 50)
                    throw new Exception("El nombre no puede superar los 50 caracteres.");

                nombre = value.Trim();
            }
        }

        public string Pais
        {
            get { return pais; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Ingrese un país, no puede estar vacío.");

                if (value.Trim().Length > 50)
                    throw new Exception("El país no puede superar los 50 caracteres.");

                pais = value.Trim();
            }
        }

        public bool Activo
        {
            get { return activo; }
            set { activo = value; }
        }

        public Estado(string codigo, string nombre, string pais, bool activo)
        {
            Codigo = codigo;
            Nombre = nombre;
            Pais = pais;
            Activo = activo;
        }

        public override string ToString()
        {
            return "\n - Código: " + Codigo +
                   "\n - Nombre: " + Nombre +
                   "\n - País: " + Pais +
                   "\n - Activo: " + Activo;
        }
    }
}
