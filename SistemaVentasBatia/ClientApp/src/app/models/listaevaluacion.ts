import { Evaluacion } from "./evaluacion";

export interface ListaEvaluacion {
    evaluaciones: Evaluacion[];
    pagina: number;
    numPaginas: number;
    rows: number;
}