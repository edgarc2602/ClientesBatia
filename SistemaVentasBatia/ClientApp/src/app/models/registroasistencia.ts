export interface RegistroAsistencia {
    idEmpleado: number;
    nombre: string;
    horaEntrada: Date;
    horaSalida: Date;
    jornal: boolean;
    puesto: string;
    turno: string;
}