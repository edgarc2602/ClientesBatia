import { AsistenciaMes } from "./asistenciames";
import { Incidencia } from "./incidencia";
import { Sucursales } from "./sucursales";

export interface Dashboard {
    asistencia: number;
    entregas: number;
    supervision: number;
    evaluaciones: number;
    asistenciaMes: AsistenciaMes[];
    incidencia: Incidencia[];
}