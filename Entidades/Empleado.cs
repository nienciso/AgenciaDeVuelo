using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Empleado
    {
        private string usuario;
        private string contrasenia;
        private string nombreCompleto;
        private bool activo;

        public string Usuario
        {
            get { return usuario; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Ingrese un usuario, no puede estar vacío.");

                if (value.Trim().Length > 15)
                    throw new Exception("El usuario no puede superar los 15 caracteres.");

                usuario = value.Trim();
            }
        }

        public string Contrasenia
        {
            get { return contrasenia; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Ingrese una contraseña, no puede estar vacía.");

                if (value.Trim().Length < 5 || value.Trim().Length > 10)
                    throw new Exception("La contraseña debe tener entre 5 y 10 caracteres.");

                if (!TieneLetra(value))
                    throw new Exception("La contraseña debe tener al menos una letra.");

                if (!TieneNumero(value))
                    throw new Exception("La contraseña debe tener al menos un número.");

                if (!TieneCaracterEspecial(value))
                    throw new Exception("La contraseña debe tener al menos un carácter especial.");

                contrasenia = value.Trim();
            }
        }

        public string NombreCompleto
        {
            get { return nombreCompleto; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Ingrese el nombre completo, no puede estar vacío.");

                if (value.Trim().Length > 100)
                    throw new Exception("El nombre completo no puede superar los 100 caracteres.");

                nombreCompleto = value.Trim();
            }
        }

        public bool Activo
        {
            get { return activo; }
            set { activo = value; }
        }

        public Empleado(string usuario, string contrasenia, string nombreCompleto, bool activo)
        {
            Usuario = usuario;
            Contrasenia = contrasenia;
            NombreCompleto = nombreCompleto;
            Activo = activo;
        }

        private bool TieneLetra(string texto)
        {
            foreach (char c in texto)
            {
                if (char.IsLetter(c))
                    return true;
            }

            return false;
        }

        private bool TieneNumero(string texto)
        {
            foreach (char c in texto)
            {
                if (char.IsDigit(c))
                    return true;
            }

            return false;
        }

        private bool TieneCaracterEspecial(string texto)
        {
            foreach (char c in texto)
            {
                if (!char.IsLetterOrDigit(c))
                    return true;
            }

            return false;
        }

        public override string ToString()
        {
            return "\n - Usuario: " + Usuario +
                   "\n - Nombre completo: " + NombreCompleto +
                   "\n - Activo: " + Activo;
        }
    }
}
