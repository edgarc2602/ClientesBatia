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
    public interface IEvaluacionRepository
    {
        Task<int> ContarEvaluaciones(ParamDashboardDTO param);
        Task<List<Evaluacion>> ObtenerEvaluaciones(int mes, int anio, int idCliente, int pagina);
    }

    public class EvaluacionRepository : IEvaluacionRepository
    {
        private readonly DapperContext _ctx;

        public EvaluacionRepository(DapperContext context)
        {
            _ctx = context;
        }

        public async Task<int> ContarEvaluaciones(ParamDashboardDTO param)
        {
            string query = @"
SELECT
count(a.id_campania) Rows 
from tb_encuesta_registro a
where a.id_status = 1
AND MONTH(a.fecha) = @Mes
AND YEAR(a.fecha) = @Anio
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

        public async Task<List<Evaluacion>> ObtenerEvaluaciones(int mes, int anio, int idCliente, int pagina)
        {
            string query = @"
SELECT  *   
FROM (
SELECT ROW_NUMBER() OVER ( ORDER BY a.id_campania desc ) AS RowNum,
id_campania IdCampania, 
b.nombre Cliente, 
c.nombre Inmueble,
d.descripcion Encuesta, 
convert(varchar(12),fecha,103) Fecha, 
encuestado Encuestado, 
resul1 Resultado1, 
resul2 Resultado2, 
resul3 Resultado3, 
resul4 Resultado4, 
resul5 Resultado5
from tb_encuesta_registro a inner join tb_cliente b on a.id_cliente = b.id_cliente
inner join tb_cliente_inmueble c on a.id_inmueble = c.id_inmueble
inner join tb_encuesta_nombre d on a.id_encuesta = d.id_encuesta
where a.id_status = 1
AND MONTH(a.fecha) = @Mes
AND YEAR(a.fecha) = @Anio
AND a.id_cliente = @IdCliente
) AS Evaluaciones
WHERE   RowNum >= ((@pagina - 1) * 40) + 1
AND RowNum <= (@pagina * 40)
ORDER BY RowNum
";
            var Evaluaciones = new List<Evaluacion>();
            try
            {
                using var connection = _ctx.CreateConnection();
                Evaluaciones = (await connection.QueryAsync<Evaluacion>(query, new { mes,anio,idCliente,pagina})).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Evaluaciones;
        }
    }
}
