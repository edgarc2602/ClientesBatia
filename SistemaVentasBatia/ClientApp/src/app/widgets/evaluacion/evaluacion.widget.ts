import { Component, OnChanges, Output, EventEmitter, SimpleChanges, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Catalogo } from 'src/app/models/catalogo';
import Swal from 'sweetalert2';
import { StoreUser } from 'src/app/stores/StoreUser';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ListaEvaluacion } from 'src/app/models/listaevaluacion';
import { ParamDashboard } from '../../models/paramdashboard';
declare var bootstrap: any;

@Component({
    selector: 'evaluacion-widget',
    templateUrl: './evaluacion.widget.html'
})
export class EvaluacionWidget {
    @Output('evaEvent') sendEvent = new EventEmitter<boolean>();
    model: ListaEvaluacion = {
        evaluaciones: [], pagina: 1, numPaginas: 0, rows: 0
    }
    param: ParamDashboard = {
        dia: 0, mes: 0, anio: 0, idCliente: this.user.idCliente, idInmueble: 0, fecha: ''
    }
    mesesc: Catalogo[];
    isLoading: boolean = false;

    constructor(@Inject('BASE_URL') private url: string, private http: HttpClient, public user: StoreUser, private fb: FormBuilder) {
        http.get<Catalogo[]>(`${url}api/catalogo/obtenermeses`).subscribe(response => {
            this.mesesc = response;
        })
    }

    open(anio: number, mes: number, idSucursal: number) {
        this.limpiarParam();
        this.param.anio = anio;
        this.param.mes = mes;
        this.param.idInmueble = idSucursal;
        this.obtenerEvaluaciones(1);
        let docModal = document.getElementById('modalEvaluacion');
        let myModal = bootstrap.Modal.getOrCreateInstance(docModal);
        myModal.show();
    }

    limpiarParam() {
        this.param = {
            dia: 0, mes: 0, anio: 0, idCliente: this.user.idCliente, idInmueble: 0, fecha: ''
        }
    }

    obtenerEvaluaciones(paginaini: number) {
        this.isLoading = true;
        this.model.evaluaciones = [];
        if (paginaini == 1) {
            this.model.pagina = 1;
        }
        this.http.post<ListaEvaluacion>(`${this.url}api/evaluacion/GetListaEvaluacion/${this.model.pagina}`, this.param).subscribe(response => {
            setTimeout(() => {
                this.model = response;
                this.isLoading = false;
            }, 300);
        }, err => {
            setTimeout(() => {
                this.isLoading = false;
            }, 300);
        });
    }

    acepta() {
        this.sendEvent.emit(true);
        this.close();
    }

    cancela() {
        this.close();
    }

    close() {
        let docModal = document.getElementById('modalEvaluacion');
        let myModal = bootstrap.Modal.getOrCreateInstance(docModal);
        myModal.hide();
    }

    quitarFocoDeElementos(): void {
        const elementos = document.querySelectorAll('button, input[type="text"]');
        elementos.forEach((elemento: HTMLElement) => {
            elemento.blur();
        });
    }

    muevePagina(event) {
        this.model.pagina = event;
        this.obtenerEvaluaciones(0);
    }
}