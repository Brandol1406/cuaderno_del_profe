import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AsistenciaModel } from 'src/app/models/asistencia.model';
import { AsistenciaEstudianteModel } from 'src/app/models/asistenciaestudiante.model';
import { ApiService } from 'src/app/services/api.service';
import Swal from 'sweetalert2';
import { MateriaModel } from 'src/app/models/materia.model';
import { PeriodoModel } from 'src/app/models/periodo.model';
import { InscripcionModel } from 'src/app/models/InscripcionModel';

@Component({
  selector: 'app-asistencias-form',
  templateUrl: './asistencias-form.component.html'
})
export class AsistenciasFormComponent implements OnInit {
  model: AsistenciaModel = {
    asistenciasEstudiantes: [],
    fecha: null,
    idAsistencia: 0,
    idMateria: null,
    idPeriodo: null,
  };

  materias: MateriaModel[] = [];
  periodos: PeriodoModel[] = [];
  inscripciones: InscripcionModel[] = [];

  isEdit = false;
  errors: object = {};
  private apiUrl = '/Asistencia';
  private id = 0;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private apiService: ApiService
  ) { }

  async ngOnInit() {
    this.id = Number(this.route.snapshot.paramMap.get('id'));
    if (this.id) {
      let result = await this.apiService.api.get(`${this.apiUrl}/${this.id}`);

      const existingItem = result.data;
      if (existingItem) {
        console.log(existingItem)
        this.model = { ...existingItem };
        this.isEdit = true;
      }
    }

    await this.loadRelationObjects();
    this.setListaInscritos(this.model.idPeriodo, this.model.idMateria);
  }

  async loadRelationObjects() {
    var response = await this.apiService.api.get(`${this.apiUrl}/GetRelationObjects`);
    let { data } = response;
    
    this.materias = data.materias;
    this.periodos = data.periodos;
    this.inscripciones = data.inscripciones;
  }

  onSeleccionPeriodo(event: Event) {
    const valor = (event.target as HTMLSelectElement).value;
    this.setListaInscritos(this.model.idPeriodo, this.model.idMateria);
  }

  onSeleccionMateria(event: Event) {
    const valor = (event.target as HTMLSelectElement).value;
    this.setListaInscritos(this.model.idPeriodo, this.model.idMateria);
  }

  setListaInscritos(idPeriodo: number, idMateria: number) {
    this.model.asistenciasEstudiantes = [...this.inscripciones]
      .filter(x => x.idPeriodo == idPeriodo && x.idMateria == idMateria)
      .map(x  => (
        {
          idEstudiante: x.idEstudiante,
          presente: this.model.asistenciasEstudiantes.find(y => y.idEstudiante == x.idEstudiante)?.presente ?? false,
          estudiante: x.estudiante,
          matricula: x.matricula
        }
      ) as AsistenciaEstudianteModel);
  }

  async onSubmit(e: Event) {
    e.preventDefault();
    let toSend = { ...this.model };
    console.log(toSend)
    try {
      let result;
      if (this.isEdit) {
        result = await this.apiService.api.put(`${this.apiUrl}/${this.id}`, toSend);
      } else {
        result = await this.apiService.api.post(`${this.apiUrl}`, toSend);
      }
      Swal.fire("Guardado", result.data.message, "success");
      this.router.navigate(['/Asistencias/List']);
    }
    catch (e) {
      this.errors = { ...e.response.data.errors };
      Swal.fire("Aviso", e.response.data.message, "warning");
      console.log(this.errors);
    }
  }
}