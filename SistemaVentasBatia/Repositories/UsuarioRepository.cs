using Dapper;
using SistemaClientesBatia.Context;
using SistemaClientesBatia.DTOs;
using SistemaClientesBatia.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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
        Task<List<RegistroAsistencia>> GetRegistroAsistencia(ParamDashboardDTO param, int dia, int mes, int anio);
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
                id_cliente as IdCliente, 
                per_status Estatus, 
                id_empleado as IdEmpleado
                FROM personal 
                where 
                per_usuario = @Usuario and 
                per_password = @Contrasena AND
                Per_Interno = 1 AND
                id_proveedor = 0";

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
            string query = ""; ;
            DateTime fechaActual = DateTime.Now;

            if (fechaActual.Year == param.Anio && fechaActual.Month == param.Mes)
            {
                param.Dia = fechaActual.Day;
            }
            else
            {
                query += @"
                        DECLARE @FechaInicio DATETIME = DATEFROMPARTS(@Anio, @Mes, 1);
                        DECLARE @UltimoDiaMes DATETIME = EOMONTH(@FechaInicio);
                        DECLARE @DiasEnMes INT = DAY(@UltimoDiaMes);
                        SET @dia = @DiasEnMes; ";
            }
            query += @"
                SELECT (
                    CAST((
                        -- Sumar asistencias de empleados y jornaleros
                        SELECT *
                            -- Asistencias únicas de empleados
                            --COUNT(*)
                        FROM (
                            SELECT 
				                SUM(ISNULL(emp.Asistencia, 0) + ISNULL(jorn.Asistencia, 0)) AS Asistencia  -- Suma las asistencias de ambas tablas
			                FROM (
				                -- Subconsulta para asistencias de empleados-----------
				                SELECT 
					                COUNT(DISTINCT a.id_empleado) AS Asistencia
				                FROM tb_empleado_asistencia a
				                INNER JOIN tb_cliente_inmueble b 
					                ON a.id_inmueble = b.id_inmueble
				                WHERE 
					                MONTH(a.fecha) = @Mes
					                AND YEAR(a.fecha) = @Anio
					                AND ISNULL(NULLIF(@IdCliente ,0), b.id_cliente) = b.id_cliente
					                AND ISNULL(NULLIF(@IdInmueble,0), a.id_inmueble) = a.id_inmueble
					                AND a.movimiento IN ('A','A4')
				                GROUP BY DAY(a.fecha)
				                --------------------------------------------------------
			                ) AS emp
			                FULL OUTER JOIN (
				                -- Subconsulta para asistencias de jornaleros------------
				                SELECT 
					                COUNT(DISTINCT a.id_jornalero) AS Asistencia
				                FROM tb_jornalero_asistencia a
				                INNER JOIN tb_cliente_inmueble b 
					                ON a.id_inmueble = b.id_inmueble
				                WHERE 
					                MONTH(a.fecha) = @Mes
					                AND YEAR(a.fecha) = @Anio
					                AND ISNULL(NULLIF(@IdCliente ,0), b.id_cliente) = b.id_cliente
					                AND ISNULL(NULLIF(@IdInmueble,0), a.id_inmueble) = a.id_inmueble
				                GROUP BY DAY(a.fecha)
				                ---------------------------------------------------------
			                ) AS jorn
			                ON emp.Asistencia = jorn.Asistencia
                        ) AS AsistenciasEmpleadosUnicas
                    ) AS decimal(18, 2)) + 
	                CAST((
                        SELECT 
				                SUM(ISNULL(descansos.Asistencia, 0)) AS Asistencia  -- Suma las asistencias de ambas tablas
			                FROM (
				                -- Subconsulta para asistencias de empleados-----------
				                SELECT 
					                COUNT(DISTINCT a.id_empleado) AS Asistencia
				                FROM tb_empleado_asistencia a
				                INNER JOIN tb_cliente_inmueble b 
					                ON a.id_inmueble = b.id_inmueble
				                WHERE 
					                MONTH(a.fecha) = @Mes
					                AND YEAR(a.fecha) = @Anio
					                AND ISNULL(NULLIF(@IdCliente ,0), b.id_cliente) = b.id_cliente
					                AND ISNULL(NULLIF(@IdInmueble,0), a.id_inmueble) = a.id_inmueble
					                AND a.movimiento IN ('N')
				                GROUP BY DAY(a.fecha)
				                --------------------------------------------------------
			                ) AS descansos
                    ) AS decimal(18, 2))
                ) / 
                -- Cálculo del total de días multiplicado por la cantidad en plantilla
                CAST((
                    SELECT SUM(cantidad) 
                    FROM tb_cliente_plantilla a 
                    INNER JOIN tb_cliente_inmueble b ON a.id_inmueble = b.id_inmueble  
                    WHERE 
                        b.id_cliente = @IdCliente
                        AND a.id_status = 1
                        AND ISNULL(NULLIF(@IdInmueble, 0), a.id_inmueble) = a.id_inmueble
                        AND b.id_status = 1 
                ) * @dia AS decimal(18, 2)) * 100
                 AS AsistenciaMensual";

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
                SELECT 
                    COALESCE(emp.Fecha, jorn.Fecha) AS Fecha,  -- Elige la fecha de cualquiera de las dos tablas si una es NULL
                    ISNULL(emp.Asistencia, 0) + ISNULL(jorn.Asistencia, 0) AS Asistencia  -- Suma las asistencias de ambas tablas
                FROM (
                    -- Subconsulta para asistencias de empleados-----------
                    SELECT 
                        DAY(a.fecha) AS Fecha, 
                        COUNT(DISTINCT a.id_empleado) AS Asistencia
                    FROM tb_empleado_asistencia a
                    INNER JOIN tb_cliente_inmueble b 
                        ON a.id_inmueble = b.id_inmueble
                    WHERE 
                        MONTH(a.fecha) = @Mes
                        AND YEAR(a.fecha) = @Anio
                        AND ISNULL(NULLIF(@IdCliente ,0), b.id_cliente) = b.id_cliente
                        AND ISNULL(NULLIF(@IdInmueble,0), a.id_inmueble) = a.id_inmueble
                        AND a.movimiento IN ('A','A4')
                    GROUP BY DAY(a.fecha)
	                --------------------------------------------------------
                ) AS emp
                FULL OUTER JOIN (
                    -- Subconsulta para asistencias de jornaleros------------
                    SELECT 
                        DAY(a.fecha) AS Fecha, 
                        COUNT(DISTINCT a.id_jornalero) AS Asistencia
                    FROM tb_jornalero_asistencia a
                    INNER JOIN tb_cliente_inmueble b 
                        ON a.id_inmueble = b.id_inmueble
                    WHERE 
                        MONTH(a.fecha) = @Mes
                        AND YEAR(a.fecha) = @Anio
                        AND ISNULL(NULLIF(@IdCliente ,0), b.id_cliente) = b.id_cliente
                        AND ISNULL(NULLIF(@IdInmueble,0), a.id_inmueble) = a.id_inmueble
                    GROUP BY DAY(a.fecha)
	                ---------------------------------------------------------
                ) AS jorn
                ON emp.Fecha = jorn.Fecha  -- Unimos ambas consultas por la fecha
                ORDER BY Fecha";
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
                -- Primera consulta: movimientos
WITH PrimerConsulta AS (
    SELECT 
        CASE 
            WHEN movimiento = 'A' THEN 'Inicio de labores'
            WHEN movimiento = 'A4' THEN 'Fin de labores'
            WHEN movimiento = 'D' THEN 'Doblete' 
            WHEN movimiento = 'F' THEN 'Falta'
            WHEN movimiento = 'FJ' THEN 'Falta Justificada'
            WHEN movimiento = 'IEG' THEN 'Incapacidad enfermedad general' 
            WHEN movimiento = 'IRT' THEN 'Incapacidad riesgo trabajo' 
            WHEN movimiento = 'N' THEN 'Descanso' 
            WHEN movimiento = 'V' THEN 'Vacaciones' 
            ELSE 'Otro' 
        END AS Movimiento,
        COUNT(*) AS Total
    FROM (
        SELECT DISTINCT a.id_empleado, DAY(a.fecha) AS dia, a.movimiento
        FROM tb_empleado_asistencia a
        INNER JOIN tb_cliente_inmueble b ON a.id_inmueble = b.id_inmueble
        WHERE 
            b.id_cliente = @IdCliente  
            AND ISNULL(NULLIF(@IdInmueble,0), b.id_inmueble) = b.id_inmueble
            AND MONTH(fecha) = @Mes 
            AND YEAR(fecha) = @Anio 
            --AND a.movimiento != 'A4'
    ) AS registros_unicos
    GROUP BY movimiento
),

-- Segunda consulta: ""Cubierto""
SegundaConsulta AS (
    SELECT 
        'Cubierto' AS Movimiento, 
        COUNT (*) AS Total 
    FROM tb_jornalero_asistencia a
    WHERE
        a.id_cliente = @IdCliente
        AND ISNULL(NULLIF(@IdInmueble,0), a.id_inmueble) = a.id_inmueble
        AND MONTH(a.fecha) = @Mes 
        AND YEAR(a.fecha) = @Anio
)

-- Unir ambas consultas con UNION ALL
SELECT * FROM PrimerConsulta
UNION ALL
SELECT * FROM SegundaConsulta
ORDER BY Movimiento;

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

        public async Task<List<RegistroAsistencia>> GetRegistroAsistencia(ParamDashboardDTO param, int dia, int mes, int anio)
        {
            string query = @"
            SELECT 
                a.id_empleado AS IdEmpleado,
                c.nombre + ' ' + c.paterno + ' ' + c.materno AS Nombre,
                MIN(CASE WHEN a.movimiento = 'A' THEN a.fecha END) AS HoraEntrada, -- La primera hora de asistencia (entrada)
                MAX(CASE WHEN a.movimiento = 'A4' THEN a.fecha END) AS HoraSalida,  -- La última hora de asistencia (salida),
	            0 AS Jornal 
            FROM tb_empleado_asistencia a 
            INNER JOIN tb_cliente_inmueble b ON a.id_inmueble = b.id_inmueble
            INNER JOIN tb_empleado c ON c.id_empleado = a.id_empleado
            WHERE 
                MONTH(a.fecha) = @mes AND 
                YEAR(a.fecha) = @anio AND 
                DAY(a.fecha) = @dia AND
                ISNULL(NULLIF(@IdCliente, 0), b.id_cliente) = b.id_cliente AND 
                ISNULL(NULLIF(@IdInmueble, 0), a.id_inmueble) = a.id_inmueble AND 
                a.movimiento IN ('A','A4')
            GROUP BY 
                a.id_empleado, 
                c.nombre, c.paterno, c.materno

            UNION ALL

            SELECT 
            a.id_jornalero AS IdEmpleado,
            b.nombre + ' ' + b.paterno + ' ' + b.materno AS Nombre,
            fecha AS HoraEntrada,
            fecha AS HoraSalida,
            1 AS Jornal 
            FROM tb_jornalero_asistencia a
            INNER JOIN tb_jornalero b ON a.id_jornalero = b.id_jornalero
            WHERE 
                MONTH(a.fecha) = @mes AND 
                YEAR(a.fecha) = @anio AND 
                DAY(a.fecha) = @dia AND
                ISNULL(NULLIF(@IdCliente, 0), a.id_cliente) = a.id_cliente AND 
                ISNULL(NULLIF(@IdInmueble, 0), a.id_inmueble) = a.id_inmueble
            ORDER BY Nombre";
            try
            {
                using (var connection = _ctx.CreateConnection())
                {
                    var list = (await connection.QueryAsync<RegistroAsistencia>(query, new { param.IdCliente, param.IdInmueble, dia, mes, anio })).ToList();
                    return list;
                }
            }
            catch (Exception)
            {
                throw new CustomException("Error al obtener detalle de asistencia");
            }
        }
    }
}