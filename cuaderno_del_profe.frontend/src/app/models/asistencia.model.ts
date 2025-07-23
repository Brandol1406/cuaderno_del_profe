import { AsistenciaEstudianteModel } from "./asistenciaestudiante.model";

export interface AsistenciaModel {
  idAsistencia: number;
  idMateria: number;
  materia?: string;
  idPeriodo: number;
  periodo?: string;
  fecha: string; // Se recomienda usar string para fechas (ISO 8601) en TS
  asistenciasEstudiantes: AsistenciaEstudianteModel[];
}