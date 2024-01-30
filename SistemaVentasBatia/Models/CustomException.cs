using System;

namespace SistemaClientesBatia.Models
{
    public class CustomException : Exception
    {
        public CustomException() : base()
        {

        }

        public CustomException(string message) : base(message)
        {

        }

        /* janx: usar en caso de intentar regresar multiples errores en formato json
        public CustomException(string message, params object[] args) : base(String.Format(System.Globalization.CultureInfo.CurrentCulture, message, args))
        {

        }
        */
    }
}
