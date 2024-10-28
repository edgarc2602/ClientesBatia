using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS;
using OfficeOpenXml;
using SistemaClientesBatia.Controllers;
using SistemaClientesBatia.DTOs;
using SistemaClientesBatia.Models;
using SistemaClientesBatia.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SistemaClientesBatia.Services
{
    public interface IUsuarioService
    {
        Task<UsuarioDTO> Login(AccesoDTO dto);
        Task<bool> Existe(AccesoDTO dto);
        Task<ActionResult<DashboardDTO>> GetDashboard(ParamDashboardDTO param);
        Task<List<SucursalesDTO>> GetSucursales(int idCliente);
        Task<List<RegistroAsistenciaDTO>> GetRegistroAsistencia(ParamDashboardDTO param);
        Task<byte[]> DescargarAsistencia(int idCliente, int idSucursal, string fechaAsistencia);
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
            dto.Contrasena = Encriptar(dto.Contrasena);
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
        public static string Encriptar(string pass)
        {
            // Convertir la contraseña a bytes UTF-8
            byte[] bytes = Encoding.UTF8.GetBytes(pass);

            // Crear un objeto SHA256 para calcular el hash
            using (SHA256 sha256 = SHA256.Create())
            {
                // Calcular el hash de la contraseña
                byte[] hashBytes = sha256.ComputeHash(bytes);

                // Convertir el hash a una cadena hexadecimal
                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                // Devolver el hash en formato hexadecimal
                return builder.ToString();
            }
        }
        public async Task<ActionResult<DashboardDTO>> GetDashboard(ParamDashboardDTO param)
        {
            
            try
            {
                var dashboard = new DashboardDTO
                {
                    Asistencia = await _repo.GetAsistenciaInd(param),
                    Entregas = await _repo.GetEntregasInd(param),
                    Supervision = await _repo.GetSupervisionInd(param),
                    Evaluaciones = await _repo.GetEvaluacionesInd(param),
                    AsistenciaMes = _mapper.Map<List<AsistenciaMesDTO>>(await _repo.GetAsistenciaMes(param)),
                    Incidencia = _mapper.Map<List<IncidenciaDTO>>(await _repo.GetIncidencia(param))

                };            
                return dashboard;

            }
            catch (Exception)
            {
                throw;
            }


        }

        public async Task<List<SucursalesDTO>> GetSucursales(int idCliente)
        {
            return _mapper.Map<List<SucursalesDTO>>(await _repo.GetSucursalesidCliente(idCliente));
        }

        public async Task<List<RegistroAsistenciaDTO>> GetRegistroAsistencia(ParamDashboardDTO param)
        {
            DateTime fecha = DateTime.ParseExact(param.Fecha, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            int dia = fecha.Day;
            int mes = fecha.Month;
            int anio = fecha.Year;
            //return _mapper.Map<List<RegistroAsistenciaDTO>>(await _repo.GetRegistroAsistencia(param, dia, mes, anio));
            return await ObtenerRegistroAsistenciaPorPartes(param);
        }

        public async Task <List<RegistroAsistenciaDTO>> ObtenerRegistroAsistenciaPorPartes(ParamDashboardDTO param)
        {
            DateTime fecha = DateTime.ParseExact(param.Fecha, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            param.Dia = fecha.Day;
            param.Mes = fecha.Month;
            param.Anio = fecha.Year;


            //OPERARIOS
            var listaA = _mapper.Map<List<RegistroAsistenciaDTO>>(await _repo.GetListaA(param));

            // Obtener la lista A4
            var listaA4 = _mapper.Map<List<RegistroAsistenciaDTO>>(await _repo.GetListaA4(param));

            var listaNocturnos = new List<int>();

            // Actualizar la hora de salida y a su vez llenar arreglo de registros con idTurno = 3
            foreach (var entrada in listaA)
            {
                if(entrada.IdTurno == 3)
                {
                    listaNocturnos.Add(entrada.IdEmpleado);
                }
                var salida = listaA4.FirstOrDefault(s => s.IdEmpleado == entrada.IdEmpleado);
                if (salida != null)
                {
                    if (salida.HoraSalida > entrada.HoraEntrada)
                    {
                        entrada.HoraSalida = salida.HoraSalida;
                    }
                }
            }
            //Si existen registros con turno nocturno entonces proceder a consultar su registro de salida del dia siguiente y agregarlo a la lista principal "listaA"
            if (listaNocturnos.Count != 0)
            {
                var listaA4N = _mapper.Map<List<RegistroAsistenciaDTO>>(await _repo.GetListaA4Nocturno(param, listaNocturnos));
                if (listaA4N.Count != 0)
                {
                    foreach (var entrada in listaA)
                    {
                        var salida = listaA4N.FirstOrDefault(s => s.IdEmpleado == entrada.IdEmpleado);
                        if (salida != null)
                        {
                            entrada.HoraSalida = salida.HoraSalida;
                        }
                    }
                }
            }



            //JORNALES
            //get registros de jornaleros
            var listaAJornal = _mapper.Map<List<RegistroAsistenciaDTO>>(await _repo.GetListaAJornal(param));

            // Agregar registros de listaAJornal a listaA
            if(listaAJornal.Count != 0)
            {
                listaA.AddRange(listaAJornal);
            }

            // Ordenar la lista por Inmueble y Nombre
            listaA = listaA.OrderBy(x => x.Inmueble).ThenBy(x => x.Nombre).ToList();

            return listaA;
        }

        public async Task<byte[]> DescargarAsistencia(int idCliente, int idSucursal, string fechaAsistencia)
        {
            var param = new ParamDashboardDTO();
            param.IdCliente = idCliente;
            param.IdInmueble = idSucursal;
            param.Fecha = fechaAsistencia;
            string rutaArchivo = Path.Combine("Layouts", "ReporteAsistencia.xlsx");

            try
            {
                using (var stream = new MemoryStream(File.ReadAllBytes(rutaArchivo)))
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        //Resumen

                        DateTime fecha = DateTime.ParseExact(param.Fecha, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                        int dia = fecha.Day;
                        int mes = fecha.Month;
                        int anio = fecha.Year;
                        var lista = _mapper.Map<List<RegistroAsistenciaDTO>>(await ObtenerRegistroAsistenciaPorPartes(param));

                        if (lista.Count != 0)
                        {
                            var wsD = package.Workbook.Worksheets[0];
                            int row = 2;
                            foreach (var d in lista)
                            {
                                
                                wsD.Cells[row, 1].Value = d.HoraEntrada;
                                wsD.Cells[row, 2].Value = d.Inmueble;
                                wsD.Cells[row, 3].Value = d.IdEmpleado;
                                wsD.Cells[row, 4].Value = d.Nombre;
                                wsD.Cells[row, 5].Value = d.Nss;
                                wsD.Cells[row, 6].Value = d.Puesto;
                                wsD.Cells[row, 7].Value = d.Turno;
                                wsD.Cells[row, 8].Value = "";
                                wsD.Cells[row, 9].Value = d.HoraEntrada != DateTime.MinValue ? d.HoraEntrada.ToString("dd-MM-yyyy HH:mm:ss tt") : "N/A";
                                //wsD.Cells[row, 9].Value = d.HoraSalida;
                                wsD.Cells[row, 10].Value = d.HoraSalida != DateTime.MinValue ? d.HoraSalida.ToString("dd-MM-yyyy HH:mm:ss tt") : "N/A";


                                row++;
                            }
                        }
                        return package.GetAsByteArray();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
            
        }
    }
}
