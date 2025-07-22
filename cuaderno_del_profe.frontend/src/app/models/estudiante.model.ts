export interface EstudianteModel {
    idEstudiante: number;
    nombres: string;
    apellidos: string;
    sexo: string;
    fechaNacimiento: Date;
    direccion: string;
    telefono1: string;
    telefono2: string;
    email: string;
    fregistro: Date;
    inscripciones: Array<any>;
}