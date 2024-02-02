using Dapper;
using SistemaClientesBatia.Context;
using SistemaClientesBatia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaClientesBatia.Enums;
using Microsoft.AspNetCore.Server.IIS.Core;

namespace SistemaClientesBatia.Repositories
{
    public interface ICatalogosRepository
    {
        Task<List<Catalogo>> ObtenerMeses();
        Task<List<Catalogo>> GetPrioridadTK();
        Task<List<Catalogo>> GetStatusTK();
        Task<List<Catalogo>> GetCategoriaTK();
    }

    public class CatalogosRepository : ICatalogosRepository
    {
        private readonly DapperContext ctx;

        public CatalogosRepository(DapperContext ctx)
        {
            this.ctx = ctx;
        }

        public async Task<List<Catalogo>> ObtenerMeses()
        {
            var query = @"
SELECT 
id_mes Id,
descripcion Descripcion
From tb_mes
";
            var meses = new List<Catalogo>();
            try
            {
                using var connection = ctx.CreateConnection();
                meses = (await connection.QueryAsync<Catalogo>(query)).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return meses;
        }

        public async Task<List<Catalogo>> GetPrioridadTK()
        {
            string query = @"
SELECT 
id_prioridad Id,
descripcion Descripcion
From tb_prioridadtk
";
            var prioridades = new List<Catalogo>();
            try
            {
                using var connection = ctx.CreateConnection();
                prioridades = (await connection.QueryAsync<Catalogo>(query)).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return prioridades;
        }

        public async Task<List<Catalogo>> GetStatusTK()
        {
            string query = @"
SELECT 
id_status Id,
descripcion Descripcion
From tb_statustk
";
            var status = new List<Catalogo>();
            try
            {
                using var connection = ctx.CreateConnection();
                status = (await connection.QueryAsync<Catalogo>(query)).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public async Task<List<Catalogo>> GetCategoriaTK()
        {
            string query = @"
SELECT 
id_categoria Id,
descripcion Descripcion
From tb_categoriatk
";
            var categorias = new List<Catalogo>();
            try
            {
                using var connection = ctx.CreateConnection();
                categorias = (await connection.QueryAsync<Catalogo>(query)).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return categorias;
        }
    }
}
