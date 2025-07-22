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
  fregistro?: string;     // Usamos string porque en frontend las fechas suelen llegar como ISO strings
  fevaluacion: string;
}