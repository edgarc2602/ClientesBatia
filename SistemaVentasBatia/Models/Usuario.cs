namespace SistemaClientesBatia.Models
{
    public class Usuario
    {
        public string Identificador { get; set; }
        public string Nombre { get; set; }
        public int IdPersonal { get; set; }
        public int IdInterno { get; set; }
        public int IdEmpleado { get; set; }
        public int Estatus { get; set; }
        public int IdProveedor { get; set;}
        public int IdCliente { get; set; }
    }
}
