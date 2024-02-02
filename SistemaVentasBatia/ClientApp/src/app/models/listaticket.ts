import { TicketMin } from './ticketmin';

export interface ListaTicket {
    tickets: TicketMin[];
    pagina: number;
    numPaginas: number;
    rows: number;
    idPrioridad: number;
    idCategoria: number;
    idStatus: number;
}