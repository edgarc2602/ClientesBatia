using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using SistemaClientesBatia.Context;
using SistemaClientesBatia.DTOs;
using SistemaClientesBatia.Enums;
using SistemaClientesBatia.Models;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace SistemaClientesBatia.Repositories
{
    public interface ISupervisionRepository
    {
        Task<int> ContarSupervisiones(ParamDashboardDTO param);
        Task<List<Supervision>> ObtenerSupervisiones(int mes, int anio, int idCliente, int pagina, int idInmueble);
    }

    public class SupervisionRepository : ISupervisionRepository
    {
        private readonly DapperContext _ctx;

        public SupervisionRepository(DapperContext context)
        {
            _ctx = context;
        }

        public async Task<int> ContarSupervisiones(ParamDashboardDTO param)
        {
            string query = @"
SELECT 
count(a.id_supervision) Rows 
FROM tb_supervision a
WHERE MONTH(a.fechaini) = @Mes
and ISNULL(NULLIF(@IdInmueble,0), a.id_inmueble) = a.id_inmueble
AND YEAR(a.fechaini) = @Anio
AND a.id_cliente = @IdCliente
";
            var numrows = 0;
            try
            {
                using var connection = _ctx.CreateConnection();
                numrows = await connection.QuerySingleAsync<int>(query, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return numrows;
        }

        public async Task<List<Supervision>> ObtenerSupervisiones(int mes, int anio, int idCliente, int pagina, int idInmueble)
        {
            string query = @"
SELECT  *   
FROM (
SELECT ROW_NUMBER() OVER ( ORDER BY a.fechaini desc ) AS RowNum,
a.id_supervision IdSupervision,
a.fechaini Fecha,
b.nombre Cliente,
c.nombre Inmueble,
d.Per_Nombre + ' ' + d.Per_Paterno Usuario,
case when clienteentrevista = 1 then 'Si' else 'No'end Entrevista,
clientenombre ClienteNombre,
case when evalua = 3 then 'Bueno' when evalua= 2 then 'Regular' when evalua = 1 then 'Malo' else 'No califica' end Calificacion
FROM tb_supervision a
inner join tb_cliente b ON a.id_cliente=b.id_cliente 
LEFT OUTER JOIN tb_cliente_inmueble c ON a.id_inmueble=c.id_inmueble 
LEFT OUTER JOIN Personal d ON a.usuario=d.IdPersonal
WHERE MONTH(a.fechaini) = @Mes
and ISNULL(NULLIF(@IdInmueble,0), a.id_inmueble) = a.id_inmueble
AND YEAR(a.fechaini) = @Anio
AND a.id_cliente = @IdCliente
) AS Supervisiones
WHERE   RowNum >= ((@pagina - 1) * 40) + 1
AND RowNum <= (@pagina * 40)
ORDER BY RowNum
";
            var supervisiones = new List<Supervision>();
            try
            {
                using var connection = _ctx.CreateConnection();
                supervisiones = (await connection.QueryAsync<Supervision>(query, new { mes,anio,idCliente,pagina,idInmueble})).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return supervisiones;
        }
    }
}
