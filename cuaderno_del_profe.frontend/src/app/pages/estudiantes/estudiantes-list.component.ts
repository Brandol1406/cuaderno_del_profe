import { Component, OnInit } from '@angular/core';
import { EstudianteModel } from 'src/app/models/estudiante.model';
import { ApiService } from 'src/app/services/api.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-estudiantes-list',
  templateUrl: './estudiantes-list.component.html'
})
export class EstudiantesListComponent implements OnInit {
  items: EstudianteModel[] = [];
  private apiUrl = '/Estudiantes';

  constructor(private apiService: ApiService) {}

  ngOnInit() {
    this.loadItems();
  }

  async loadItems() {
    let result = await this.apiService.api.get(this.apiUrl);

    this.items = [...result.data];
  }

  async deleteItem(id: number) {
      Swal.fire({
        title: "¿Seguro desea eliminar este registro?",
        showCancelButton: true,
        cancelButtonText: 'Cancelar',
        confirmButtonText: `
        <i class="bi bi-trash"></i> Eliminar`,
        confirmButtonColor: 'red',
      }).then(async (result) => {
        /* Read more about isConfirmed, isDenied below */
        if (result.isConfirmed) {
          try {
            let result = await this.apiService.api.delete(`${this.apiUrl}/${id}`);
  
            this.loadItems();
            Swal.fire("Eliminado!", "Se ha eliminado con éxito", "success");
          }
          catch (e) {
            console.log(e);
            Swal.fire("Aviso", e.response.data.message, "warning");
          }
        }
      });
    }
}