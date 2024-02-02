import { Component, OnChanges, Output, EventEmitter, SimpleChanges, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DetalleMaterial } from '../../models/detallematerial';
import { Catalogo } from 'src/app/models/catalogo';
import { Ticket } from 'src/app/models/ticket';
import Swal from 'sweetalert2';
import { StoreUser } from 'src/app/stores/StoreUser';


declare var bootstrap: any;

@Component({
    selector: 'generaticket-widget',
    templateUrl: './generaticket.widget.html'
})
export class GeneraTicketWidget {
    @Output('ansEvent') sendEvent = new EventEmitter<boolean>();
    model: Ticket = {
        idClienteTicket: 0, nombre: '', paterno: '', materno: '', email: '', descripcion: '', idCategoria: 0, categoria: '', idPrioridad: 0, idStatus: 0, idCliente: 0
    }
    prioridad: Catalogo[];
    categoria: Catalogo[];

    constructor(@Inject('BASE_URL') private url: string, private http: HttpClient, public user: StoreUser) {
        http.get<Catalogo[]>(`${url}api/catalogo/GetPrioridadTK`).subscribe(response => {
            this.prioridad = response;
        })
        http.get<Catalogo[]>(`${url}api/catalogo/GetCategoriaTK`).subscribe(response => {
            this.categoria = response;
        })
    }

    open() {
        this.model = {
            idClienteTicket: 0, nombre: '', paterno: '', materno: '', email: '', descripcion: '', idCategoria: 0, categoria: '', idPrioridad: 0, idStatus: 0, idCliente: 0
        }
        let docModal = document.getElementById('modalGeneraTicket');
        let myModal = bootstrap.Modal.getOrCreateInstance(docModal);
        myModal.show();
    }

    acepta() {
        this.sendEvent.emit(true);
        this.close();
    }

    guardarTicket() {
        this.model.idCliente = 0
        this.model.idCliente = this.user.idPersonal;
        this.http.post<boolean>(`${this.url}api/Ticket/GuardarTicket`, this.model).subscribe(response => {
            if (response == true) {
                Swal.fire({
                    icon: 'success',
                    timer: 1000,
                    showConfirmButton: false,
                });
                this.acepta();
            }
            else {
                Swal.fire({
                    title: 'Error',
                    text: 'Ocurrio un error al generar el reporte',
                    icon: 'error',
                    timer: 3000,
                    showConfirmButton: false,
                });
            }
        })
    }

    cancela() {
        this.close();
    }

    close() {
        let docModal = document.getElementById('modalGeneraTicket');
        let myModal = bootstrap.Modal.getOrCreateInstance(docModal);
        myModal.hide();
    }

    quitarFocoDeElementos(): void {
        const elementos = document.querySelectorAll('button, input[type="text"]');
        elementos.forEach((elemento: HTMLElement) => {
            elemento.blur();
        });
    }
}