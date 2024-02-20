import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import * as Highcharts from 'highcharts';
import { fadeInOut } from 'src/app/fade-in-out';
import { HttpClient } from '@angular/common/http';
import { StoreUser } from 'src/app/stores/StoreUser';
import { Catalogo } from 'src/app/models/catalogo';
import { Dashboard } from '../../models/dashboard';
import { ParamDashboard } from '../../models/paramdashboard'
import Swal from 'sweetalert2';
import { Sucursales } from '../../models/sucursales';
import { EvaluacionWidget } from '../../widgets/evaluacion/evaluacion.widget'
import { SupervisionWidget } from '../../widgets/supervision/supervision.widget'
import accessibility from 'highcharts/modules/accessibility';


@Component({
    selector: 'dashboard-comp',
    templateUrl: './dashboard.component.html',
    animations: [fadeInOut],
})
export class DashboardComponent implements OnInit {
    mesesc: Catalogo[];
    param: ParamDashboard = {
        mes: 0, anio: 0, idCliente: 0, idInmueble: 0
    }
    dashboard: Dashboard;
    idSucursal: number = 0;
    isLoading: boolean = false;
    selectedRow: any;
    sucursaln: string = '';
    sucursales: Sucursales[];
    @ViewChild(EvaluacionWidget, { static: false }) EvaWid: EvaluacionWidget;
    @ViewChild(SupervisionWidget, { static: false }) SupWid: SupervisionWidget;
    

    constructor(@Inject('BASE_URL') private url: string, private http: HttpClient, public user: StoreUser) {
        http.get<Catalogo[]>(`${url}api/catalogo/obtenermeses`).subscribe(response => {
            this.mesesc = response;
        })
        
    }
    openSupervisionModal() {
        this.SupWid.open(this.param.anio, this.param.mes, this.idSucursal);
    }

    openEvaluacionModal() {
        this.EvaWid.open(this.param.anio, this.param.mes, this.idSucursal);
    }

    ngOnInit(): void {
        this.limpiarParam();
        const anioActual = new Date().getFullYear();
        this.param.anio = anioActual;
        const mesActual = new Date().getMonth() + 1;
        this.param.mes = mesActual;
        this.sucursaln = 'Total';
        this.getDashboard('Total');
        this.getSucursales();
        accessibility(Highcharts);
    }

    getSucursales() {
        this.param.idCliente = this.user.idInterno;
        this.http.get<Sucursales[]>(`api/usuario/GetSucursales/${this.user.idInterno}`).subscribe(response => {
            this.sucursales = response;
        })
    }

    limpiarParam() {
        this.param = {
            mes: 0, anio: 0, idCliente: 0, idInmueble: 0
        }
    }

    selectSucursal(sucursal: any, nombre: string) {
        this.selectedRow = sucursal;
        this.idSucursal = sucursal.idSucursal;
        this.sucursaln = sucursal.sucursal;
        this.getDashboard(this.sucursaln);
    }

    getDashboard(sucursaln: string) {
        sucursaln = this.sucursaln;
        this.isLoading = true;
        this.param.idInmueble = this.idSucursal;
        this.param.idCliente = this.user.idInterno;
        this.dashboard = {
            asistencia: 0, entregas: 0, supervision: 0, evaluaciones: 0, asistenciaMes: [], incidencia: []
        };
        this.http.post<Dashboard>(`${this.url}api/usuario/getDashboard`, this.param).subscribe(response => {
            this.dashboard = response;
            this.graficaAsistenciaMes(sucursaln);
            this.graficaIncidencia();
            this.isLoading = false;
        }, err => {
            this.isLoading = false;
            console.log(err)
            Swal.fire({
                title: 'Error',
                text: 'Ocurrio un error al consultar la informacion',
                icon: 'error',
                timer: 3000,
                showConfirmButton: false,
            });
        });
    }

    getDashboard2() {
        this.isLoading = true;
        this.param.idInmueble = this.idSucursal;
        this.param.idCliente = this.user.idInterno;
        this.http.post<Dashboard>(`${this.url}api/usuario/GetDashboardData`, this.param).subscribe(response => {
            this.dashboard = response;
            this.isLoading = false;
        }, err => {
            this.isLoading = false;
            console.log(err)
            Swal.fire({
                title: 'Error',
                text: 'Ocurrio un error al consultar la informacion',
                icon: 'error',
                timer: 3000,
                showConfirmButton: false,
            });
        });
    }

    graficaAsistenciaMes(sucursaln: string) {
        let container: HTMLElement = document.getElementById('GAM');
        let totalAsistencia = 0;
        this.dashboard.asistenciaMes.forEach(dia => {
            totalAsistencia += dia.asistencia;
        });
        const seriesData = this.dashboard.asistenciaMes.map(dia => {
            return {
                name: dia.fecha,
                y: dia.asistencia
            };
        });
        const totalSubtitle = `Total: ${totalAsistencia}`;
        Highcharts.chart(container, {
            chart: {
                type: 'column'
            },
            title: {

                text: 'Asistencia mensual ' + sucursaln
            },
            subtitle: {
                text: totalSubtitle,
                align: 'center',
                style: {
                    fontSize: '16px'
                }
            },
            plotOptions: {
                column: {
                    color: '#5094fc'
                }
            },
            xAxis: {
                categories: this.dashboard.asistenciaMes.map(dia => dia.fecha.toString()),
                crosshair: true
            },
            yAxis: {
                allowDecimals: false,
                min: 0,
                title: {
                    text: 'Asistencia'
                }
            },
            tooltip: {
                headerFormat: '<span style="font-size:10px">Dia:{point.key}</span><table>',
                pointFormat: '<tr><td style="color:{series.color};padding:0">Total: </td>' +
                    '<td style="padding:0"><b>{point.y:.0f}</b></td></tr>',
                footerFormat: '</table>',
                shared: true,
                useHTML: true
            },
            series: [{
                type: 'column',
                name: 'Dias',
                data: seriesData
            }],
            credits: {
                enabled: false
            }
        });
    }

    graficaIncidencia() {
        let container: HTMLElement = document.getElementById('GIM');
        let totalIncidencias = 0;
        this.dashboard.incidencia.forEach(incidencia => {
            totalIncidencias += incidencia.total;
        });
        const seriesData = this.dashboard.incidencia.map(incidencia => {
            return {
                name: incidencia.movimiento,
                y: incidencia.total
            };
        });
        //const totalSubtitle = `Total: ${totalIncidencias}`;
        Highcharts.chart(container, {
            chart: {
                type: 'pie'
            },
            title: {
                text: `Incidencias`
            },
            //subtitle: {
            //    text: totalSubtitle,
            //    align: 'center',
            //    style: {
            //        fontSize: '16px'
            //    }
            //},
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        format: '<b>{point.name}</b>: {point.percentage:.1f}%'
                    }
                }
            },
            tooltip: {
                headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                pointFormat: '<tr><td style="color:{series.color};padding:0">Total: </td>' +
                    '<td style="padding:0"><b>{point.y:.0f}</b></td></tr>',
                footerFormat: '</table>',
                shared: true,
                useHTML: true
            },
            series: [{
                type: 'pie',
                name: 'Incidencias',
                data: seriesData
            }],
            credits: {
                enabled: false
            }
        });
    }

    regresaEva() {

    }
    regresaSup() {

    }
}