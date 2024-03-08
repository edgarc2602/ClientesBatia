import { Listados } from './listados';

export interface ListadoMateriales {
    listas: Listados[];
    pagina: number;
    rows: number;
    numPaginas: number;
}