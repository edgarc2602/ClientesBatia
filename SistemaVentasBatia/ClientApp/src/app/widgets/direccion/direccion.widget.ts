import { Component, OnChanges, Input, SimpleChanges, Inject, Output, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Direccion } from '../../models/direccion';
import { Catalogo } from '../../models/catalogo';
declare var bootstrap: any;
import { Subject } from 'rxjs';
import { ToastWidget } from '../toast/toast.widget';

@Component({
    selector: 'direc-widget',
    templateUrl: './direccion.widget.html'
})
export class DireccionWidget {
    idD: number = 0;
    idP: number = 0;
    @Output('smEvent') sendEvent = new EventEmitter<any>();
    model: Direccion = {} as Direccion;
    tips: Catalogo[] = [];
    edos: Catalogo[] = [];
    tabs: Catalogo[] = [];
    lerr: any = {};
    muns: Catalogo[] = [];

    evenSub: Subject<void> = new Subject<void>();
    isErr: boolean = false;
    validaMess: string = '';

    constructor(@Inject('BASE_URL') private url: string, private http: HttpClient) {
        http.get<Catalogo[]>(`${url}api/catalogo/getestado`).subscribe(response => {
            this.edos = response;
        }, err => console.log(err));
        http.get<Catalogo[]>(`${url}api/catalogo/getinmuebletipo`).subscribe(response => {
            this.tips = response;
        }, err => console.log(err));
    }

    nuevo() {
        this.model = {
            idDireccion: 0, idProspecto: this.idP, idCotizacion: 0, nombreSucursal: '',
            idTipoInmueble: 0, idEstado: 0, municipio: '', ciudad: '', colonia: '',
            domicilio: '', referencia: '', codigoPostal: '', idDireccionCotizacion: 0
        };
    }

    refresh() {
        this.http.get<Direccion>(`${this.url}api/direccion/${this.idD}/0`).subscribe(response => {
            this.model = response;
            this.chgEdo();
        }, err => console.log(err));
    }

    guarda() {
        this.lerr = {};
        if (this.valida()) {
            if (this.model.idDireccion == 0) {
                this.http.post<Direccion>(`${this.url}api/direccion`, this.model).subscribe(response => {
                    this.sendEvent.emit(response.idDireccion);
                    this.close();
                    this.isErr = false;
                    this.validaMess = 'Direccion agregada';
                    this.evenSub.next();
                }, err => {
                    console.log(err);
                    this.isErr = true;
                    this.validaMess = 'Ocurrio un error';
                    this.evenSub.next();
                    if (err.error) {
                        if (err.error.errors) {
                            this.lerr = err.error.errors;
                        }
                    }
                });
            } else {
                this.http.put<Direccion>(`${this.url}api/direccion`, this.model).subscribe(response => {
                    this.sendEvent.emit(response.idDireccion);
                    this.close();
                    this.isErr = false;
                    this.validaMess = 'Direcci�n actualizada';
                    this.evenSub.next();
                }, err => {
                    console.log(err);
                    this.isErr = true;
                    this.validaMess = 'Ocurrio un error';
                    this.evenSub.next();
                    if (err.error) {
                        if (err.error.errors) {
                            this.lerr = err.error.errors;
                        }
                    }
                });
            }
        }
    }

    chgEdo() {
        this.http.get<Catalogo[]>(`${this.url}api/tabulador/getbyedo/${1}`).subscribe(response => {
            this.tabs = response;
        }, err => console.log(err));
    }
    loadMun() {
        this.http.get<Catalogo[]>(`${this.url}api/catalogo/getmunicipio/${this.model.idEstado}`).subscribe(response => {
            this.muns = response;
        }, err => console.log(err));

        
    }

    valida() {
        return true;
    }

    ferr(nm: string) {
        let fld = this.lerr[nm];
        if (fld)
            return true;
        else
            return false;
    }

    terr(nm: string) {
        let fld = this.lerr[nm];
        let msg: string = fld.map((x: string) => "-" + x);
        return msg;
    }
    existe() {  
        this.http.get<Direccion>(`${this.url}api/direccion/obtenerdireccion/${this.idD}/${this.idP}`).subscribe(response => {
            this.model = response;
            this.loadMun();
        }, err => {
            console.log(err);
            if (err.error) {
                if (err.error.errors) {
                    this.lerr = err.error.errors;
                }
            }
        });
        
        let docModal = document.getElementById('modalAgregarDireccion');
        let myModal = bootstrap.Modal.getOrCreateInstance(docModal);
        myModal.show();
    }

    open(pro: number, dir: number) {
        this.lerr = {};
        this.idP = pro;
        this.idD = dir;
        if (dir != 0) {
            this.existe();
        }
        else if (dir == 0) {
            this.nuevo();
        }
        else {
            this.refresh();
        }
        let docModal = document.getElementById('modalAgregarDireccion');
        let myModal = bootstrap.Modal.getOrCreateInstance(docModal);
        myModal.show();
        this.chgEdo();
    }

    close() {
        let docModal = document.getElementById('modalAgregarDireccion');
        let myModal = bootstrap.Modal.getOrCreateInstance(docModal);
        myModal.hide();
    }
}