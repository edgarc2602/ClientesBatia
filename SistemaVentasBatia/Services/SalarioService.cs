﻿using AutoMapper;
using SistemaVentasBatia.DTOs;
using SistemaVentasBatia.Models;
using SistemaVentasBatia.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;

namespace SistemaVentasBatia.Services
{
    public interface ISalarioService
    {
        Task<int> Create(SalarioDTO dto);
        Task<SalarioDTO> Get(int id);
        Task<SalarioMinDTO> GetFind(int idTabulador, int idPuesto, int idTurno);
        Task<bool> Update(SalarioDTO dto);
        Task<bool> Delete(int id);
    }
    public class SalarioService : ISalarioService
    {
        private readonly ISalarioRepository _repo;
        private readonly IMapper _mapper;

        public SalarioService(ISalarioRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public Task<int> Create(SalarioDTO dto)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<SalarioDTO> Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<SalarioMinDTO> GetFind(int idTabulador, int idPuesto, int idTurno)
        {

            //IEnumerable<SalarioMinDTO> lf = _mapper.Map<IEnumerable<SalarioMinDTO>>(
            // await _repo.Busqueda(idTabulador, idPuesto, idTurno)).ToList
            // //reg = lf.FirstOrDefault();
            //if (reg == null)
            //{
            //    SalarioMinimo sm = await _repo.ObtenerMinimo(DateTime.Today.Year);
            //    reg = new SalarioMinDTO
            //    {
            //        SalarioI = sm.SalarioBase * 30.4167m
            //    };
            //}
            decimal result = 0;
            SalarioMinDTO reg = new SalarioMinDTO();
            if (idTabulador == 1)
            {
                result = await _repo.ObtenerSalarioMixto(idPuesto);
                reg.SalarioI = result;
            }
            else if (idTabulador == 2)
            {
                result = await _repo.ObtenerSalarioMixtoFrontera(idPuesto);
                reg.SalarioI = result;
            }
            else if (idTabulador == 3)
            {
                result = await _repo.ObtenerSalarioReal(idPuesto);
                reg.SalarioI = result;
            }
            else if (idTabulador == 4)
            {
                result = await _repo.ObtenerSalarioRealFrontera(idPuesto);
                reg.SalarioI = result;
            }
            return reg;
        }

        public Task<bool> Update(SalarioDTO dto)
        {
            throw new System.NotImplementedException();
        }
    }
}
