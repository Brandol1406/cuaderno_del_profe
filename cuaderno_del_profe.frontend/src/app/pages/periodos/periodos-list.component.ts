import { Component, OnInit } from '@angular/core';
import { PeriodoModel } from 'src/app/models/periodo.model';
import { ApiService } from 'src/app/services/api.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-periodos-list',
  templateUrl: './periodos-list.component.html'
})
export class PeriodosListComponent implements OnInit {
  items: PeriodoModel[] = [];
  private apiUrl = '/Periodo';
  searchText: string;

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