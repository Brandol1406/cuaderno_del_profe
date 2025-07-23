export interface CalificacionModel {
  idCalificacion: number;
  idEstudiante: number;
  estudiante?: string;
  matricula?: string;
  idMateria: number;
  materia?: string;
  idPeriodo: number;
  periodo?: string;
  calificacion1: number;
  fregistro?: Date;   
  fevaluacion: Date;
}