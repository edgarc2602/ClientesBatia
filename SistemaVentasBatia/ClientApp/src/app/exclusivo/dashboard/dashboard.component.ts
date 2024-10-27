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
import { EntregaWidget } from '../../widgets/entrega/entrega.widget';
import { RegistroAsistencia } from '../../models/registroasistencia';
import { DatePipe } from '@angular/common';
import { saveAs } from 'file-saver';

@Component({
    selector: 'dashboard-comp',
    templateUrl: './dashboard.component.html',
    animations: [fadeInOut],
    providers: [DatePipe],
})
export class DashboardComponent implements OnInit {
    mesesc: Catalogo[];
    param: ParamDashboard = {
        dia: 0, mes: 0, anio: 0, idCliente: this.user.idCliente, idInmueble: 0, fecha: ''
    }
    dashboard: Dashboard;
    idSucursal: number = 0;
    isLoading: boolean = false;
    selectedRow: any;
    sucursaln: string = '';
    sucursales: Sucursales[];
    registroA: RegistroAsistencia[] = [];
    fechaAsistencia: string = '';
    @ViewChild(EvaluacionWidget, { static: false }) EvaWid: EvaluacionWidget;
    @ViewChild(SupervisionWidget, { static: false }) SupWid: SupervisionWidget;
    @ViewChild(EntregaWidget, { static: false }) EntWid: EntregaWidget;

    constructor(@Inject('BASE_URL') private url: string, private http: HttpClient, public user: StoreUser, private dtpipe: DatePipe) {
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

    openEntregaModal() {
        this.EntWid.open(this.param.anio, this.param.mes, this.idSucursal);

    }

    ngOnInit(): void {
        this.limpiarParam();
        const anioActual = new Date().getFullYear();
        this.param.anio = anioActual;
        const mesActual = new Date().getMonth() + 1;
        this.param.mes = mesActual;
        const fechaActual = new Date();
        this.fechaAsistencia = this.dtpipe.transform(fechaActual, 'yyyy-MM-dd')
        this.sucursaln = 'Total';
        this.getDashboard('Total');
        this.getSucursales();
        accessibility(Highcharts);
        this.getRegistroAsistencia();
    }

    getSucursales() {
        this.http.get<Sucursales[]>(`api/usuario/GetSucursales/${this.user.idCliente}`).subscribe(response => {
            this.sucursales = response;
        })
    }

    limpiarParam() {
        this.param = {
            dia: 0, mes: 0, anio: 0, idCliente: this.user.idCliente, idInmueble: 0, fecha: ''
        }
    }

    selectSucursal(sucursal: any, nombre: string) {
        if (this.isLoading == false) {
            if (this.idSucursal != sucursal.idSucursal) {
                this.selectedRow = sucursal;
                this.idSucursal = sucursal.idSucursal;
                this.sucursaln = sucursal.sucursal;
                this.getDashboard(this.sucursaln);
                this.getRegistroAsistencia();
            }

        }

    }
    getRegistroAsistencia() {
        this.param.fecha = this.fechaAsistencia;
        this.http.post<RegistroAsistencia[]>(`${this.url}api/usuario/GetRegistroAsistencia`, this.param).subscribe(response => {
            this.registroA = response;
            this.isLoading = false;
        }, err => {
            this.isLoading = false;
            Swal.fire({
                title: 'Error',
                text: 'Ocurrio un error al consultar el detalle',
                icon: 'error',
                timer: 3000,
                showConfirmButton: false,
            });
        });
    }

    getDashboard(sucursaln: string) {
        sucursaln = this.sucursaln;
        this.isLoading = true;
        this.param.idInmueble = this.idSucursal;
        //this.dashboard = {
        //    asistencia: 0, entregas: 0, supervision: 0, evaluaciones: 0, asistenciaMes: [], incidencia: []
        //};
        this.http.post<Dashboard>(`${this.url}api/usuario/getDashboard`, this.param).subscribe(response => {
            this.dashboard = response;
            this.graficaAsistenciaMes(sucursaln);
            this.graficaIncidencia();
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

    //getDashboard2() {
    //    this.isLoading = true;
    //    this.param.idInmueble = this.idSucursal;
    //    this.http.post<Dashboard>(`${this.url}api/usuario/GetDashboardData`, this.param).subscribe(response => {
    //        this.dashboard = response;
    //        this.isLoading = false;
    //    }, err => {
    //        this.isLoading = false;
    //        console.log(err)
    //        Swal.fire({
    //            title: 'Error',
    //            text: 'Ocurrio un error al consultar la informacion',
    //            icon: 'error',
    //            timer: 3000,
    //            showConfirmButton: false,
    //        });
    //    });
    //}

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

                text: null/*'Asistencia mensual'*/
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
                title: null
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
                text: null /*`Incidencias`*/
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
        this.isLoading = false;

    }

    regresaEva() {

    }

    regresaSup() {

    }

    DescargarAsistencia() {
        this.isLoading = true;
        this.http.post(`${this.url}api/usuario/DescargarAsistencia/${this.user.idCliente}/${this.idSucursal}/${this.fechaAsistencia}`, null, { responseType: 'blob' }).subscribe((response: Blob) => {
            saveAs(response, 'ReporteAsistencia:' + this.fechaAsistencia +'.xlsx');
            this.isLoading = false;
        },
            error => {
                console.error('Error al descargar el archivo:', error);
                this.isLoading = false;
            }
        );
    }
    formatHoraSalida(fecha: string): string {
        if (fecha === '0001-01-01T00:00:00') {
            return 'N/A';
        } else {
            const fechaObj = new Date(fecha);
            let formattedDate = fechaObj.toLocaleString('es-ES', {
                day: '2-digit',
                month: '2-digit',
                year: 'numeric',
                hour: '2-digit',
                minute: '2-digit',
                hour12: true
            });

            // Usar una expresión regular para asegurarse de reemplazar bien "a. m." y "p. m." con "AM" y "PM"
            return formattedDate.replace(/\sa\.?\s?m\.?/i, ' AM').replace(/\sp\.?\s?m\.?/i, ' PM');
        }
    }



}