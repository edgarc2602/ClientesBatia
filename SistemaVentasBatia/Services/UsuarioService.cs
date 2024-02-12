using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS;
using SistemaClientesBatia.Controllers;
using SistemaClientesBatia.DTOs;
using SistemaClientesBatia.Models;
using SistemaClientesBatia.Repositories;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace SistemaClientesBatia.Services
{
    public interface IUsuarioService
    {
        Task<UsuarioDTO> Login(AccesoDTO dto);
        Task<bool> Existe(AccesoDTO dto);
        Task<ActionResult<DashboardDTO>> GetDashboard(ParamDashboardDTO param);
        DashboardDTO GetDashboardData(ParamDashboardDTO param);
        Task<List<SucursalesDTO>> GetSucursales(int idCliente);
    }
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repo;
        private readonly IMapper _mapper;

        public UsuarioService(IUsuarioRepository repoUsuario, IMapper mapper)
        {
            _repo = repoUsuario;
            _mapper = mapper;
        }

        public async Task<bool> Existe(AccesoDTO dto)
        {
            bool existe = false;
            try
            {
                Acceso acc = _mapper.Map<Acceso>(dto);
                UsuarioDTO usu = _mapper.Map<UsuarioDTO>(await _repo.Login(acc));
                if (usu == null)
                {
                    existe = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return existe;
        }

        public async Task<UsuarioDTO> Login(AccesoDTO dto)
        {
            UsuarioDTO usu;
            try
            {
                Acceso acc = _mapper.Map<Acceso>(dto);
                usu = _mapper.Map<UsuarioDTO>(await _repo.Login(acc));
                if (usu == null)
                {
                    throw new CustomException("Usuario no Existe");
                }
                if (usu.Estatus != 0)
                {
                    throw new CustomException("Usuario inactivo");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return usu;
        }

        public async Task<ActionResult<DashboardDTO>> GetDashboard(ParamDashboardDTO param)
        {
            var dashboard = new DashboardDTO();
            dashboard.Asistencia = await _repo.GetAsistenciaInd(param);
            dashboard.Entregas = await _repo.GetEntregasInd(param);
            dashboard.Supervision = await _repo.GetSupervisionInd(param);
            dashboard.Evaluaciones = await _repo.GetEvaluacionesInd(param);
            var sucursales = _mapper.Map<List<SucursalesDTO>>(await _repo.GetSucursales(param));
            dashboard.Sucursales = sucursales;
            var asistenciaMes = _mapper.Map <List<AsistenciaMesDTO>>(await _repo.GetAsistenciaMes(param));
            dashboard.AsistenciaMes = asistenciaMes;
            var incidencia = _mapper.Map <List<IncidenciaDTO>>(await _repo.GetIncidencia(param));
            dashboard.Incidencia = incidencia;
            return dashboard;
        }
        public DashboardDTO GetDashboardData(ParamDashboardDTO param)
        {
            return _repo.GetDashboardData(param);
        }

        public async Task<List<SucursalesDTO>> GetSucursales(int idCliente)
        {
            var sucursales = _mapper.Map<List<SucursalesDTO>>(await _repo.GetSucursalesidCliente(idCliente));
            return sucursales;
        }
    }
}
