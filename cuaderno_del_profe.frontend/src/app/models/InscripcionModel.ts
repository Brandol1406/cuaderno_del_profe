export interface InscripcionModel {
  idInscripcion?: number;
  idEstudiante?: number;
  estudiante?: string;
  matricula?: string;
  idMateria?: number;
  materia?: string;
  idPeriodo?: number;
  periodo?: string;
  fInicioPeriodo?: Date;
  fFinPeriodo?: Date;
}