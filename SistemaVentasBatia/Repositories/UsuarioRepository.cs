using Dapper;
using SistemaClientesBatia.Context;
using SistemaClientesBatia.DTOs;
using SistemaClientesBatia.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace SistemaClientesBatia.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario> Login(Acceso acceso);
        Task<bool> ConsultarUsuario(int idPersonal, string Nombres);
        Task<decimal> GetAsistenciaInd(ParamDashboardDTO param);
        Task<decimal> GetEntregasInd(ParamDashboardDTO param);
        Task<int> GetSupervisionInd(ParamDashboardDTO param);
        Task<int> GetEvaluacionesInd(ParamDashboardDTO param);
        Task<List<AsistenciaMes>> GetAsistenciaMes(ParamDashboardDTO param);
        Task<List<Incidencia>> GetIncidencia(ParamDashboardDTO param);
        Task<List<Sucursales>> GetSucursalesidCliente(int idCliente);
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

        public async Task<decimal> GetAsistenciaInd(ParamDashboardDTO param)
        {
            decimal cumplimiento = 0;
            string query;
            if (param.IdInmueble == 0)
            {
                query = @"
DECLARE  @dias as int
DECLARE @FechaInicio DATETIME = DATEFROMPARTS(@Anio, @Mes, 1);
DECLARE @UltimoDiaMes DATETIME = EOMONTH(@FechaInicio);
DECLARE @DiasEnMes INT = DAY(@UltimoDiaMes);
SET @dias = @DiasEnMes;
SELECT (
    CAST((
        SELECT COUNT(id_empleado)
        FROM tb_empleado_asistencia a
        INNER JOIN tb_cliente_inmueble b ON a.id_inmueble = b.id_inmueble
        WHERE b.id_cliente = @IdCliente
        AND movimiento IN ('A', 'N')
        AND MONTH(fecha) = @Mes
        AND YEAR(fecha) = @Anio
    ) AS decimal(18, 2)) / 
    CAST((
        (SELECT SUM(cantidad) 
        FROM tb_cliente_plantilla a 
        INNER JOIN tb_cliente_inmueble b ON a.id_inmueble = b.id_inmueble  
        WHERE a.id_status = 1   
        AND b.id_status = 1 
        AND b.id_cliente = @IdCliente)
        * @dias
    ) AS decimal(18, 2)) * 100
) AS AsistenciaMensual
                ";
                try
                {
                    using (var connection = _ctx.CreateConnection())
                    {
                        cumplimiento = await connection.QueryFirstAsync<decimal>(query, param);
                        cumplimiento = Math.Round(cumplimiento, 2);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString() + "GetAsistenciaInd");
                    throw ex;
                }
                return cumplimiento;
            }
            else
            {
                //query = @"
                //select cast(isnull(AVG(calif),0) as numeric(12,2))  as AsistenciaSuc  from( 
                //select cast((cast(count(id_empleado) as float)/
                //(Select sum(cantidad) as total from tb_cliente_plantilla a inner join tb_cliente_inmueble b on a.id_inmueble = b.id_inmueble where 
                //A.id_inmueble  = @IdInmueble 
                //and a.id_status = 1)) *100 as numeric(8,2)) as calif
                //from tb_empleado_asistencia a inner join tb_cliente_inmueble b on a.id_inmueble = b.id_inmueble where 
                //A.id_inmueble  = @IdInmueble 
                //AND movimiento = 'A'
                //and month(fecha) = @Mes
                //and YEAR (fecha) = @Anio
                //and id_cliente = @IdCliente
                //group by fecha) as tabla1

                query = @"
DECLARE  @dias as int
DECLARE @FechaInicio DATETIME = DATEFROMPARTS(@Anio, @Mes, 1);
DECLARE @UltimoDiaMes DATETIME = EOMONTH(@FechaInicio);
DECLARE @DiasEnMes INT = DAY(@UltimoDiaMes);
SET @dias = @DiasEnMes;
SELECT (
    CAST((
        SELECT COUNT(id_empleado)
        FROM tb_empleado_asistencia a
        INNER JOIN tb_cliente_inmueble b ON a.id_inmueble = b.id_inmueble
        WHERE b.id_cliente = @IdCliente
		AND b.id_inmueble = @IdInmueble
        AND movimiento IN ('A', 'N')
        AND MONTH(fecha) = @Mes
        AND YEAR(fecha) = @Anio
    ) AS decimal(18, 2)) / 
    CAST((
        (SELECT SUM(cantidad) 
        FROM tb_cliente_plantilla a 
        INNER JOIN tb_cliente_inmueble b ON a.id_inmueble = b.id_inmueble  
        WHERE a.id_status = 1
		AND b.id_inmueble = @IdInmueble
        AND b.id_status = 1 
        AND b.id_cliente = @IdCliente)
        * @dias
    ) AS decimal(18, 2)) * 100
) AS AsistenciaMensual
";
                try
                {
                    using (var connection = _ctx.CreateConnection())
                    {
                        cumplimiento = await connection.QueryFirstAsync<decimal>(query, param);
                        cumplimiento = Math.Round(cumplimiento, 2);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString() + "GetAsistenciaInd");
                    throw ex;
                }
                return cumplimiento;
            }

        }

        public async Task<decimal> GetEntregasInd(ParamDashboardDTO param)
        {
            int total;
            int entregado;
            decimal result;
            string queryTotal = @"
SELECT COUNT(id_listado) as total 
from tb_listadomaterial where 
id_cliente = @IdCliente 
and mes = @Mes 
and anio = @Anio 
and id_status != 5
and ISNULL(NULLIF(@IdInmueble,0), id_inmueble) = id_inmueble";
            string queryEntregado = @"
SELECT COUNT(id_listado) as entregado 
from tb_listadomaterial where 
id_cliente = @IdCliente 
and mes = @Mes 
and anio = @Anio 
and id_status = 4
and ISNULL(NULLIF(@IdInmueble,0), id_inmueble) = id_inmueble";
            try
            {
                using (var connection = _ctx.CreateConnection())
                {
                    total = await connection.QueryFirstAsync<int>(queryTotal, param);
                    entregado = await connection.QueryFirstAsync<int>(queryEntregado, param);
                }
                if (total != 0)
                {
                    result = ((decimal)entregado / total) * 100;
                    result = Math.Round(result, 2);
                }
                else
                {
                    result = 0;
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString() + "GetEntregasInd");
                throw ex;
            }
        }

        public async Task<int> GetSupervisionInd(ParamDashboardDTO param)
        {
            int supervision = 0;
            string query = @"
select isnull(count(id_supervision),0) as SupervisionSuc 
from tb_supervision where 
id_cliente = @IdCliente
and ISNULL(NULLIF(@IdInmueble,0), id_inmueble) = id_inmueble
and year(fechaini) = @Anio
and month(fechaini) = @Mes
";
            try
            {
                using (var connection = _ctx.CreateConnection())
                {
                    supervision = await connection.QueryFirstAsync<int>(query, param);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString() + "GetSupervisionInd");
                throw ex;
            }
            return supervision;
        }

        public async Task<int> GetEvaluacionesInd(ParamDashboardDTO param)
        {
            int evaluaciones = 0;
            string query;
            query = @"
select count(id_campania) as EvaluacionTotal from tb_encuesta_registro where 
id_cliente = @IdCliente
and ISNULL(NULLIF(@IdInmueble,0), id_inmueble) = id_inmueble
and year(fecha) = @Anio 
and month(fecha) = @Mes
";
            try
            {
                using (var connection = _ctx.CreateConnection())
                {
                    evaluaciones = await connection.QueryFirstAsync<int>(query, param);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString() + "GetEvaluacionesInd");
                throw ex;
            }
            return evaluaciones;
        }

        public async Task<List<AsistenciaMes>> GetAsistenciaMes(ParamDashboardDTO param)
        {
            var asistenciaMes = new List<AsistenciaMes>();
             string query = @"
select day(fecha) as Fecha, count(movimiento) as Asistencia
From tb_empleado_asistencia a inner Join tb_cliente_inmueble b on a.id_inmueble = b.id_inmueble where 
month(fecha) = @Mes 
and YEAR (fecha) = @Anio
and ISNULL(NULLIF(@IdCliente ,0), b.id_cliente) = b.id_cliente
and ISNULL(NULLIF(@IdInmueble,0), a.id_inmueble) = a.id_inmueble 
AND movimiento = 'A'
Group By DAY(fecha) Order By fecha
";
            try
            {
                using (var connection = _ctx.CreateConnection())
                {
                    asistenciaMes = (await connection.QueryAsync<AsistenciaMes>(query, param)).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString() + "GetAsistenciaInd");
                throw ex;
            }
            return asistenciaMes;

        }

        public async Task<List<Incidencia>> GetIncidencia(ParamDashboardDTO param)
        {
            var incidencias = new List<Incidencia>();
            string query = @"
select case 
when movimiento = 'A' then 'Asistencia'
when movimiento = 'D' then 'Doblete' 
when movimiento = 'F' then 'Falta'
when movimiento = 'FJ' then 'Falta Justificada'
when movimiento = 'IEG' then 'Incapacdad enfermedad general' 
when movimiento = 'IRT' then 'Incapacidad riesgo trabajo' 
when movimiento = 'N' then 'Descanso' 
when movimiento = 'V' then 'Vacaciones' 
else  'Otro' end as Movimiento,
count(movimiento) as Total from tb_empleado_asistencia a 
inner join tb_cliente_inmueble  b on a.id_inmueble = b.id_inmueble where 
b.id_cliente = @IdCliente  
and ISNULL(NULLIF(@IdInmueble,0), b.id_inmueble) = b.id_inmueble
and month(fecha) = @Mes 
and YEAR (fecha) = @Anio  
group by movimiento
";
            try
            {
                using (var connection = _ctx.CreateConnection())
                {
                    incidencias = (await connection.QueryAsync<Incidencia>(query, param)).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString() + "GetIncidenciaInd");
                throw ex;
            }
            return incidencias;
        }

        public async Task<List<Sucursales>> GetSucursalesidCliente(int IdCliente)
        {
            var sucursales = new List<Sucursales>();
            string query;
            query = @"
SELECT IdSucursal, Sucursal, Cantidad
FROM (
    SELECT 0 as IdSucursal, 'Total' as Sucursal, ISNULL(SUM(b.cantidad), 0) as Cantidad
    FROM tb_cliente_inmueble a
    LEFT OUTER JOIN tb_cliente_plantilla b ON a.id_inmueble = b.id_inmueble
    WHERE id_cliente = @IdCliente AND a.id_status = 1 AND b.id_status = 1
    GROUP BY a.id_cliente
    UNION ALL
    SELECT a.id_inmueble, RTRIM(nombre) as nombre, ISNULL(SUM(b.cantidad), 0) as Cantidad
    FROM tb_cliente_inmueble a
    LEFT OUTER JOIN tb_cliente_plantilla b ON a.id_inmueble = b.id_inmueble
    WHERE 
	id_cliente = @IdCliente 
	AND a.id_status = 1 
	AND b.id_status = 1
    GROUP BY a.id_inmueble, nombre
) AS tabla
";
            try
            {
                using (var connection = _ctx.CreateConnection())
                {
                    sucursales = (await connection.QueryAsync<Sucursales>(query, new { IdCliente })).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString() + "GetSucursales");
                throw ex;
            }
            return sucursales;
        }
    }
}