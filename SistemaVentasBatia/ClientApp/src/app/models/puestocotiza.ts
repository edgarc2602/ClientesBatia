﻿export interface PuestoCotiza {
    idPuestoDireccionCotizacion: number;
    idPuesto: number;
    idDireccionCotizacion: number;
    idCotizacion: number;
    cantidad?: number;
    idTurno: number;
    idSalario: number;
    hrInicio: string;
    hrFin: string;
    diaInicio: number;
    diaFin: number;
    fechaAlta: string;
    idPersonal: number;
    sueldo: number;
    vacaciones: number;
    primaVacacional: number;
    imss: number;
    isn: number;
    aguinaldo: number;
    total: number;

    jornada: number;
    idTabulador: number;
    idClase: number;
}