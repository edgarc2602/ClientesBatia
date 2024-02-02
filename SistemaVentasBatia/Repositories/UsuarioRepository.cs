using Dapper;
using SistemaClientesBatia.Context;
using SistemaClientesBatia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaClientesBatia.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario> Login(Acceso acceso);
        Task<bool> ConsultarUsuario(int idPersonal, string Nombres);

    }

    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly DapperContext _ctx;

        public UsuarioRepository(DapperContext context)
        {
            _ctx = context;
        }

        public async Task<Usuario> Login(Acceso acceso)
        {
            Usuario usu;
            string query = @"
SELECT 
per_usuario Identificador, 
per_nombre Nombre, 
idpersonal as IdPersonal,
per_interno as IdInterno, 
per_status Estatus, 
id_empleado as IdEmpleado
FROM personal where per_usuario = @Usuario and per_password = @Contrasena
"; // and per_status=0

            using (var connection = _ctx.CreateConnection())
            {
                usu = (await connection.QueryFirstOrDefaultAsync<Usuario>(query, acceso));
            }
            return usu;
        }

        public async Task<bool> ConsultarUsuario(int idPersonal, string Nombres)
        {
            var query = @"
           SELECT CASE
           WHEN COUNT(*) > 0 THEN 'true'
           ELSE 'false'
           END AS Resultado
           FROM Personal
           WHERE IdPersonal = @idPersonal AND Per_Nombre LIKE @Nombres;
";
            bool result = false;
            try
            {
                using var connection = _ctx.CreateConnection();
                result = await connection.QueryFirstOrDefaultAsync<bool>(query, new { idPersonal, Nombres = "%" + Nombres + "%" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}