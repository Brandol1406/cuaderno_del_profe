export interface AsistenciaEstudianteModel {
  idAsistenciaEstudiante: number;
  idAsistencia: number;
  idEstudiante: number;
  estudiante?: string;
  matricula?: string;
  presente: boolean;
}