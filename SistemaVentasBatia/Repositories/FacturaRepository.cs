using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Xml.Schema;
using Dapper;
using Microsoft.AspNetCore.Http;
using SistemaClientesBatia.Context;
using SistemaClientesBatia.Models;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace SistemaClientesBatia.Repositories
{
    public interface IFacturaRepository
    {
        //Task<List<OrdenCompra>> ObtenerOrdenesCompra(int idProveedor, string fechaInicio, string fechaFin, int pagina, int idStatus);
    }

    public class FacturaRepository : IFacturaRepository
    {
        private readonly DapperContext _ctx;

        public FacturaRepository(DapperContext context)
        {
            _ctx = context;
        }

//        public async Task<List<OrdenCompra>> ObtenerOrdenesCompra(int idProveedor, string fechaInicio, string fechaFin, int pagina, int idStatus)
//        {
//            var query = @"
//SELECT * FROM (
//SELECT 
//ROW_NUMBER() over (order by a.id_orden desc ) as rownum, 
//a.id_orden IdOrden,
//case when a.tipo = 1 then 'Materiales' else 'Servicios' end as Tipo,
//e.descripcion as Estatus, 
//convert(varchar(12), a.falta,103) as FechaAlta ,  
//b.nombre as Empresa	,
//isnull(c.nombre,'') as Proveedor, 
//isnull(d.nombre,'') as Cliente,
//f.Per_Nombre + ' ' + f.Per_Paterno as Elabora, 
//a.Total Total, 
//a.observacion Observacion, 
//a.inventario Inventario,
//CAST(SUM(ISNULL(r.total, 0)) AS decimal(18, 2)) Facturado
//from tb_ordencompra a inner join tb_empresa b on a.id_empresa = b.id_empresa
//left outer join tb_proveedor c on a.id_proveedor = c.id_proveedor
//left outer join tb_cliente d on a.id_cliente = d.id_cliente
//left outer join tb_statusc e on a.id_status = e.id_status inner join personal f on a.ualta = f.IdPersonal
//left outer join tb_recepcion r on r.id_orden = a.id_orden
//WHERE a.id_proveedor = @idProveedor
//        AND (@fechaInicio IS NULL OR @fechaFin IS NULL OR falta BETWEEN @fechaInicio AND @fechaFin)
//        AND (@idStatus = 0 OR a.id_status = @idStatus)
//    GROUP BY 
//        a.id_orden, 
//        a.tipo,
//        e.descripcion,
//        a.falta,
//        b.nombre,
//        c.nombre,
//        d.nombre,
//        f.Per_Nombre,
//        f.Per_Paterno,
//        a.Total,
//        a.observacion,
//        a.inventario
//) AS Ordenes
//WHERE   
//  RowNum >= ((@pagina - 1) * 40) + 1
//  AND RowNum <= (@pagina * 40);
//";
//            var ordenes = new List<OrdenCompra>();
//            try
//            {
//                using var connection = _ctx.CreateConnection();
//                ordenes = (await connection.QueryAsync<OrdenCompra>(query, new { idProveedor, fechaInicio, fechaFin, pagina, idStatus })).ToList();
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            return ordenes;
//        }

    }
}
