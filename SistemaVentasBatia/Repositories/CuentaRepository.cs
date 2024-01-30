using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using SistemaClientesBatia.Context;
using SistemaClientesBatia.Models;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace SistemaClientesBatia.Repositories
{
    public interface ICuentaRepository
    {
        //Task<int> ContarEstadoDeCuenta(int idProveedor);

    }

    public class CuentaRepository : ICuentaRepository
    {
        private readonly DapperContext _ctx;

        public CuentaRepository(DapperContext context)
        {
            _ctx = context;
        }

//        public async Task<List<EstadoDeCuenta>> GetEstadoDeCuenta(int idProveedor, int pagina)
//        {
//            string query = @"
//SELECT * FROM (
//SELECT
//ROW_NUMBER() over (order by b.razonsocial desc ) as rownum, 
//    b.razonsocial AS Nombre,
//    a.factura AS Factura,
//    a.total AS Total,
//    a.pago AS Pago,
//    (a.total - a.pago) AS Saldo,
//    a.ffactura AS Ffactura,
//    a.fvence AS Fvencimiento,
//    c.dias AS DiasCredito,
//    CASE
//        WHEN DATEDIFF(day, a.fvence, GETDATE()) > 0 THEN DATEDIFF(day, a.fvence, GETDATE())
//        ELSE 0
//    END AS DiasVencido,
//    CASE
//        WHEN FLOOR(DATEDIFF(day, a.fvence, GETDATE())) = 0 THEN (a.total - a.pago)
//        ELSE 0
//    END AS Corriente,
//    CASE
//        WHEN FLOOR(DATEDIFF(day, a.fvence, GETDATE())) BETWEEN 1 AND 30 THEN (a.total - a.pago)
//        ELSE 0
//    END AS Mes1,
//    CASE
//        WHEN FLOOR(DATEDIFF(day, a.fvence, GETDATE())) BETWEEN 31 AND 60 THEN (a.total - a.pago)
//        ELSE 0
//    END AS Mes2,
//    CASE
//        WHEN FLOOR(DATEDIFF(day, a.fvence, GETDATE())) BETWEEN 61 AND 90 THEN (a.total - a.pago)
//        ELSE 0
//    END AS Mes3,
//    CASE
//        WHEN FLOOR(DATEDIFF(day, a.fvence, GETDATE())) >= 91 THEN (a.total - a.pago)
//        ELSE 0
//    END AS Mes4
//FROM
//    tb_provision a
//INNER JOIN
//    tb_proveedor b ON b.id_proveedor = a.id_proveedor
//INNER JOIN
//    tb_credito c ON c.id_credito = b.credito
//WHERE
//    a.id_proveedor = @idProveedor and  (a.total - a.pago) != 0
//)
//AS EstadosDeCuenta
//WHERE   
//  RowNum >= ((@pagina - 1) * 40) + 1
//  AND RowNum <= (@pagina * 40);
//";
//            var estadodecuenta = new List<EstadoDeCuenta>();
//            try
//            {
//                using var connection = _ctx.CreateConnection();
//                estadodecuenta = (await connection.QueryAsync<EstadoDeCuenta>(query, new { idProveedor, pagina })).ToList();
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return estadodecuenta;
//        }

    }
}
