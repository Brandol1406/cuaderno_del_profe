import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CalificacionModel } from 'src/app/models/calificacion.model';
import { InscripcionModel } from 'src/app/models/InscripcionModel';
import { MateriaModel } from 'src/app/models/materia.model';
import { PeriodoModel } from 'src/app/models/periodo.model';
import { ApiService } from 'src/app/services/api.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-calificaciones-form',
  templateUrl: './calificaciones-form.component.html'
})
export class CalificacionesFormComponent implements OnInit {
  model: CalificacionModel = {
    calificacion1: null,
    fevaluacion: new Date(),
    idCalificacion: 0,
    idEstudiante: null,
    idMateria: null,
    idPeriodo: null,
  };

  materias: MateriaModel[] = [];
  periodos: PeriodoModel[] = [];
  inscripciones: InscripcionModel[] = [];

  isEdit = false;
  errors: object = {};
  private apiUrl = '/Calificacion';
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
        this.model = { ...existingItem };
        this.isEdit = true;
      }
    }

    await this.loadRelationObjects();
  }

  async loadRelationObjects() {
    var response = await this.apiService.api.get(`${this.apiUrl}/GetRelationObjects`);
    let { data } = response;

    this.materias = data.materias;
    this.periodos = data.periodos;
    this.inscripciones = data.inscripciones;
  }

  filteredInscripciones(): InscripcionModel[] {
    return [...this.inscripciones].filter(x => x.idPeriodo == this.model.idPeriodo && x.idMateria == this.model.idMateria);
  }

  obtenerLiteral(calificacion) {
    if (calificacion >= 90 && calificacion <= 100) return "A";
    if (calificacion >= 80 && calificacion <= 89) return "B";
    if (calificacion >= 70 && calificacion <= 79) return "C";
    if (calificacion >= 0 && calificacion < 70) return "F";
    return "error!";
  }

  obtenerLiteralColor(calificacion) {
    let colorClass = "";

    if (calificacion >= 90 && calificacion <= 100) {
      colorClass = "bg-success"; // verde
    } else if (calificacion >= 80 && calificacion <= 89) {
      colorClass = "bg-primary"; // azul
    } else if (calificacion >= 70 && calificacion <= 79) {
      colorClass = "bg-warning text-dark"; // amarillo
    } else if (calificacion >= 0 && calificacion < 70) {
      colorClass = "bg-danger"; // rojo
    } else {
      return `<span class="text-muted">Inválida</span>`;
    }

    return colorClass;
  }

  async onSubmit(e: Event) {
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
      this.router.navigate(['/Calificaciones/List']);
    }
    catch (e) {
      this.errors = { ...e.response.data.errors };
      Swal.fire("Aviso", e.response.data.message, "warning");
      console.log(this.errors);
    }
  }
}