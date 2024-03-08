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
    public interface IEntregaRepository
    {
        Task <int>ContarListados(int mes, int anio, int idCliente, int idInmueble, int idStatus);
        Task<List<Listados>> ObtenerListados(int mes, int anio, int idCliente, int idInmueble, int idStatus, int pagina);
    }

    public class EntregaRepository : IEntregaRepository
    {
        private readonly DapperContext _ctx;

        public EntregaRepository(DapperContext context)
        {
            _ctx = context;
        }

        public async Task<int> ContarListados(int mes, int anio, int idCliente, int idInmueble, int idStatus)
        {
            string query = @"
                SELECT 
                COUNT(a.id_listado) as total 
                from tb_listadomaterial a 
                INNER JOIN tb_cliente_inmueble b ON a.id_inmueble = b.id_inmueble
                LEFT OUTER JOIN tb_listadomateriala c ON a.id_listado = c.id_listado
                where
                a.id_cliente = @IdCliente 
                and mes = @Mes 
                and anio = @Anio 
                and a.id_status IN (2,4)
                and ISNULL(NULLIF(@IdStatus  ,0), a.id_status)   = a.id_status
                and ISNULL(NULLIF(@IdInmueble,0), a.id_inmueble) = a.id_inmueble
                ";
            var rows = 0;
            try
            {
                using var connection = _ctx.CreateConnection();
                rows = await connection.QuerySingleAsync<int>(query, new { mes, anio, idCliente, idInmueble, idStatus });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rows;
        }

        public async Task<List<Listados>> ObtenerListados(int mes, int anio, int idCliente, int idInmueble, int idStatus, int pagina)
        {
            string query = @"
                SELECT  *   
                FROM (
                SELECT ROW_NUMBER() OVER ( ORDER BY a.id_listado desc ) AS RowNum, 
                a.id_listado IdListado,
                case
                when a.id_status = 1 Then 'Alta' 
                when a.id_status = 2 then 'Aprobado'
                when a.id_status = 3 then 'Despachado' 
                when a.id_status = 4 then 'Entregado' 
                when a.id_status = 5 then 'Cancelado' 
                else 'No existe' end AS Estatus,
                --a.id_status Status,
                COALESCE(CONVERT(varchar, a.fcalendario, 103), 'No programada') AS FechaProgramada,
                COALESCE(CONVERT(varchar, a.fentrega, 103), 'Sin entregar') AS FechaEntrega,
                b.id_inmueble IdInmueble,
                b.nombre Inmueble,
                COALESCE(CONVERT(varchar, c.archivo, 120), 'Sin acuse') AS AcuseEntrega,
                c.carpeta Carpeta
                --COUNT(id_listado) as total 
                from tb_listadomaterial a 
                INNER JOIN tb_cliente_inmueble b ON a.id_inmueble = b.id_inmueble
                LEFT OUTER JOIN tb_listadomateriala c ON a.id_listado = c.id_listado
                where
                a.id_cliente = @IdCliente 
                and mes = @Mes 
                and anio = @Anio    
                and a.id_status IN (2,4)
                and ISNULL(NULLIF(@IdStatus  ,0), a.id_status)   = a.id_status
                and ISNULL(NULLIF(@IdInmueble,0), a.id_inmueble) = a.id_inmueble
                ) AS Listados
                WHERE   RowNum >= ((@pagina - 1) * 40) + 1
                AND RowNum <= (@pagina * 40)
                ORDER BY RowNum
            ";
            var listado = new List<Listados>();

            try
            {
                using var connection = _ctx.CreateConnection();
                listado = (await connection.QueryAsync<Listados>(query, new { mes, anio, idCliente, idInmueble, idStatus, pagina})).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listado;
        }
    }
}
