import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MateriaModel } from 'src/app/models/materia.model';
import { ApiService } from 'src/app/services/api.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-materias-form',
  templateUrl: './materias-form.component.html'
})
export class MateriasFormComponent implements OnInit {
  model: MateriaModel = { idMateria: 0, nombre: '', descripcion: '' };
  isEdit = false;
  errors: object = {  };
  private apiUrl = '/Materia';
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
        this.router.navigate(['/Periodos/List']);
      }
      catch (e) {
        this.errors = { ...e.response.data.errors};
        Swal.fire("Aviso", e.response.data.message, "warning");
        console.log(this.errors);
      }
    }
}