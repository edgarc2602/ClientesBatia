import { Component, Inject, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { fadeInOut } from 'src/app/fade-in-out';
import { StoreUser } from 'src/app/stores/StoreUser';
import { Catalogo } from 'src/app/models/catalogo'
import { ListaTicket } from 'src/app/models/listaticket'
import { GeneraTicketWidget } from '../../widgets/generaticket/generaticket.widget';
import { ConfirmaWidget } from '../../widgets/confirma/confirma.widget';
import Swal from 'sweetalert2';

@Component({
    selector: 'ticket-comp',
    templateUrl: './ticket.component.html',
    animations: [fadeInOut],
})
export class TicketComponent {
    prioridad: Catalogo[];
    status: Catalogo[];
    categoria: Catalogo[];
    prioridadsl: number = 0;
    categoriasl: number = 0;
    statussl: number = 0;
    idTicketIW: number = 0;
    model: ListaTicket = {
        tickets: [], pagina: 1, numPaginas: 0, rows: 0, idPrioridad: 0, idCategoria: 0, idStatus: 0
    }
    @ViewChild(GeneraTicketWidget, { static: false }) GenTick: GeneraTicketWidget;
    @ViewChild(ConfirmaWidget, { static: false }) confirma: ConfirmaWidget;

    constructor(@Inject('BASE_URL') private url: string, private http: HttpClient, public user: StoreUser) {
        http.get<Catalogo[]>(`${url}api/catalogo/GetPrioridadTK`).subscribe(response => {
            this.prioridad = response;
        })
        http.get<Catalogo[]>(`${url}api/catalogo/GetStatusTK`).subscribe(response => {
            this.status = response;
        })
        http.get<Catalogo[]>(`${url}api/catalogo/GetCategoriaTK`).subscribe(response => {
            this.categoria = response;
        })
    }

    ngOnInit() {
        this.obtenerTickets(1);
    }

    generaTicketWidget() {
        this.GenTick.open();
    }

    obtenerTickets($event) {
        this.http.get<ListaTicket>(`${this.url}api/Ticket/ObtenerListaTickets/${this.prioridadsl}/${this.categoriasl}/${this.statussl}/${this.user.idInterno}/${this.model.pagina}`).subscribe(response => {
            this.model = response;
        })
    }

    cerrarTicket($event) {
        if ($event == true) {
            this.http.get<boolean>(`${this.url}api/Ticket/CerrarTicket/${this.idTicketIW}/${this.user.idInterno}`).subscribe(response => {
                if (response == true) {
                    Swal.fire({
                        icon: 'success',
                        timer: 1000,
                        showConfirmButton: false,
                    });
                    this.obtenerTickets(1);
                }
                else {
                    Swal.fire({
                        title: 'Error',
                        text: 'Ocurrio un error al cerrar el ticket',
                        icon: 'error',
                        timer: 3000,
                        showConfirmButton: false,
                    });
                }
            })
        }
        else {
            this.idTicketIW = 0;
            this.obtenerTickets(1);
        }
        
    }

    muevePagina(event) {
        this.quitarFocoDeElementos();
        this.model.pagina = event;
        this.obtenerTickets(1);
    }

    valida(idClienteTicket: number) {
        this.idTicketIW = 0;
        this.idTicketIW = idClienteTicket;
        this.confirma.titulo = 'Cerrar ticket'
        this.confirma.mensaje = 'El estatus cambiará a "Cerrado", esta acción no se puede revertir'
        this.confirma.open();
    }

    quitarFocoDeElementos(): void {
        const elementos = document.querySelectorAll('button, input[type="text"]');
        elementos.forEach((elemento: HTMLElement) => {
            elemento.blur();
        });
    }
}
