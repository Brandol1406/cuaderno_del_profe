import { Component, OnInit, AfterViewInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EstudianteModel } from 'src/app/models/estudiante.model';
import { ApiService } from 'src/app/services/api.service';
import Swal from 'sweetalert2';
import { Modal } from 'bootstrap'; // Import Bootstrap's Modal
import { MateriaModel } from 'src/app/models/materia.model';
import { PeriodoModel } from 'src/app/models/periodo.model';
import { InscripcionModel } from 'src/app/models/InscripcionModel';
import arraysUtils from 'arrays-utils';

@Component({
  selector: 'app-estudiantes-form',
  templateUrl: './estudiantes-form.component.html'
})
export class EstudiantesFormComponent implements OnInit, AfterViewInit {
  model: EstudianteModel = {
    idEstudiante: 0,
    nombres: null,
    apellidos: null,
    sexo: null,
    direccion: null,
    email: null,
    fechaNacimiento: null,
    fregistro: null,
    telefono1: null,
    telefono2: null,
    inscripciones: []
  };

  inscripcionModel: InscripcionModel = { idInscripcion: 0, idEstudiante: 0, idMateria: null, idPeriodo: null };;
  inscripcionError: string = null;

  myModal: Modal | undefined;
  materias: Array<MateriaModel>;
  periodos: Array<PeriodoModel>;

  isEdit = false;
  errors: object = {};
  private apiUrl = '/Estudiantes';
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
  }

  async loadRelationObjects() {
    var materiasResponse = await this.apiService.api.get(`/Materia`);
    this.materias = materiasResponse.data;
    var periodosResponse = await this.apiService.api.get(`/Periodo`);
    this.periodos = periodosResponse.data;
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
      this.router.navigate(['/Estudiantes/List']);
    }
    catch (e) {
      this.errors = { ...e.response.data.errors };
      Swal.fire("Aviso", e.response.data.message, "warning");
      console.log(this.errors);
    }
  }

  initInscripcionModel(){
    this.inscripcionModel = { idInscripcion: 0, idEstudiante: 0, idMateria: null, idPeriodo: null };
  }

  addInscripcion(e: Event){
    e.preventDefault();

    if(this.existInscripcion()) {
      this.inscripcionError = "Ya existe esta inscripción";
      return;
    }

    let periodoFound = this.periodos.find(x => x.idPeriodo == this.inscripcionModel.idPeriodo);
    let materiaFound = this.materias.find(x => x.idMateria == this.inscripcionModel.idMateria);

    this.inscripcionModel.periodo = periodoFound.nombre;
    this.inscripcionModel.materia = materiaFound.nombre;
    this.inscripcionModel.fInicioPeriodo = periodoFound.finicio;
    this.inscripcionModel.fFinPeriodo = periodoFound.ffin;

    this.model.inscripciones.push({...this.inscripcionModel});
    this.initInscripcionModel();
    this.cerrarModal();
    this.inscripcionError = null;
  }

  deleteItem(item: InscripcionModel): void{
    Swal.fire({
            title: "¿Seguro desea eliminar esta inscripción?",
            showCancelButton: true,
            cancelButtonText: 'Cancelar',
            confirmButtonText: `
            <i class="bi bi-trash"></i> Eliminar`,
            confirmButtonColor: 'red',
          }).then(async (result) => {
            /* Read more about isConfirmed, isDenied below */
            if (result.isConfirmed) {
              this.model.inscripciones = arraysUtils.remove(this.model.inscripciones, item);
            }
    });
  }

  existInscripcion(){
    let found = this.model
      .inscripciones
      .find(x => x.idPeriodo == this.inscripcionModel.idPeriodo && x.idMateria == this.inscripcionModel.idMateria);

    return found != null;
  }

  dateSortedInscripciones() : InscripcionModel[] {
    return arraysUtils.orderBy(this.model.inscripciones, 'fInicioPeriodo');
  }

  ngAfterViewInit() {
    const modalElement = document.getElementById('myModal');
    if (modalElement) {
      this.myModal = new Modal(modalElement);
    }
  }

  abrirModal() {
    this.myModal.show();
  }

  cerrarModal() {
    this.myModal.hide();
  }
}