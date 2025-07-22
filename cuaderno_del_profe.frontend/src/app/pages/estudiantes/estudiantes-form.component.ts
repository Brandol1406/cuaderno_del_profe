import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EstudianteModel } from 'src/app/models/estudiante.model';
import { ApiService } from 'src/app/services/api.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-estudiantes-form',
  templateUrl: './estudiantes-form.component.html'
})
export class EstudiantesFormComponent implements OnInit {
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
    inscripciones: null 
  };
  
  isEdit = false;
  errors: object = {  };
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
        this.model = { ...existingItem };
        this.isEdit = true;
      }
    }
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
      this.router.navigate(['/Estudiantes/List']);
    }
    catch (e) {
      this.errors = { ...e.response.data.errors};
      Swal.fire("Aviso", e.response.data.message, "warning");
      console.log(this.errors);
    }
  }
}