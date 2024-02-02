import { Component, Inject, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { fadeInOut } from 'src/app/fade-in-out';
import { StoreUser } from 'src/app/stores/StoreUser';
import { Catalogo } from 'src/app/models/catalogo'
import { ListaTicket } from 'src/app/models/listaticket'
import { GeneraTicketWidget } from '../../widgets/generaticket/generaticket.widget';

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
    model: ListaTicket = {
        tickets: [], pagina: 1, numPaginas: 0, rows: 0, idPrioridad: 0, idCategoria: 0, idStatus: 0
    }
    @ViewChild(GeneraTicketWidget, { static: false }) GenTick: GeneraTicketWidget;

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
        this.http.get<ListaTicket>(`${this.url}api/Ticket/ObtenerListaTickets/${this.prioridadsl}/${this.categoriasl}/${this.statussl}/${this.user.idPersonal}/${this.model.pagina}`).subscribe(response => {
            this.model = response;
        })
    }
    cerrarTicket(idClienteTicket: number) {
        this.http.get<boolean>(`${this.url}api/Ticket/CerrarTicket/${idClienteTicket}`).subscribe(response => {
            if (response == true) {

            }
        })
    }
    muevePagina(event) {
        this.model.pagina = event;
        this.obtenerTickets(1);
    }
}
