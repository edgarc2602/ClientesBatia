import { Supervision } from "./supervision";

export interface ListaSupervision {
    supervisiones: Supervision[];
    pagina: number;
    numPaginas: number;
    rows: number;
}