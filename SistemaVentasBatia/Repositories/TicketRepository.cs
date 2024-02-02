using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using SistemaClientesBatia.Context;
using SistemaClientesBatia.Enums;
using SistemaClientesBatia.Models;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace SistemaClientesBatia.Repositories
{
    public interface ITicketRepository
    {
        //Task<int> ContarEstadoDeCuenta(int idProveedor);
        Task<bool> GuardarTicket(Ticket ticket);
        Task<int> ContarTickets(int idPrioridad, int idCategoria, int idStatus, int idCliente);
        Task<List<Ticket>> ObtenerTickets(int pagina, int idPrioridad, int idCategoria, int idStatus, int idCliente);
    }

    public class TicketRepository : ITicketRepository
    {
        private readonly DapperContext _ctx;

        public TicketRepository(DapperContext context)
        {
            _ctx = context;
        }

        public async Task<bool> GuardarTicket(Ticket ticket)
        {
            var query = @"
INSERT INTO tb_clienteticket (
nombre,
paterno,
materno,
email,
descripcion,
id_categoria,
id_prioridad,
id_status,
fecha_alta,
id_cliente
)VALUES(
@Nombre,
@Paterno,
@Materno,
@Email,
@Descripcion,
@IdCategoria,
@idPrioridad,
1,
GETDATE(),
@idCliente
)
";
            bool result;
            try
            {
                using var connection = _ctx.CreateConnection();
                await connection.ExecuteAsync(query, ticket);
                result = true;
                
            }
            catch (Exception ex)
            {
                result = false;
                throw ex;
            }
            return result;
        }

        public async Task<int> ContarTickets(int idPrioridad, int idCategoria, int idStatus, int idCliente)
        {
            string query = @"
SELECT 
count(id_clienteticket) Rows 
FROM tb_clienteticket
WHERE id_cliente = @idCliente AND
ISNULL(NULLIF(@idPrioridad,0), id_prioridad) = id_prioridad AND
ISNULL(NULLIF(@idCategoria,0), id_categoria) = id_categoria AND
ISNULL(NULLIF(@idStatus,0), id_status) = id_status
";
            var numrows = 0;
            try
            {
                using var connection = _ctx.CreateConnection();
                numrows = await connection.QuerySingleAsync<int>(query, new { idPrioridad, idCategoria, idStatus, idCliente });
            }
            catch (Exception ex) 
            {
                throw ex;
            }
            return numrows;
        }
        public async Task<List<Ticket>> ObtenerTickets(int pagina, int idPrioridad, int idCategoria, int idStatus, int idCliente)
        {
            string query = @"
SELECT  *   
FROM (
SELECT ROW_NUMBER() OVER ( ORDER BY a.id_clienteticket desc ) AS RowNum, 
a.id_clienteticket IdClienteTicket,
a.nombre Nombre,
a.paterno Paterno,
a.materno Materno,
a.email Email,
a.descripcion Descripcion,
a.id_categoria IdCategoria,
b.descripcion Categoria,
a.id_prioridad IdPrioridad,
a.id_status IdStatus,
a.fecha_alta FechaAlta,
a.id_cliente IdCliente
FROM tb_clienteticket a
INNER JOIN tb_categoriatk b ON b.id_categoria = a.id_categoria 
WHERE a.id_cliente = @idCliente AND
ISNULL(NULLIF(@idPrioridad,0), a.id_prioridad) = a.id_prioridad AND
ISNULL(NULLIF(@idCategoria,0), a.id_categoria) = a.id_categoria AND
ISNULL(NULLIF(@idStatus,0), a.id_status) = a.id_status
) AS Tickets
WHERE   RowNum >= ((@pagina - 1) * 10) + 1
AND RowNum <= (@pagina * 10)
ORDER BY RowNum
";
            var tickets = new List<Ticket>();
            try
            {
                using var connection = _ctx.CreateConnection();
                tickets = (await connection.QueryAsync<Ticket>(query, new {pagina, idPrioridad, idCategoria, idStatus, idCliente })).ToList();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return tickets;
        }
    }
}
