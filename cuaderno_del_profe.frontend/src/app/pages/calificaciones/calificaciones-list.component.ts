import { Component, OnInit } from '@angular/core';
import { CalificacionModel } from 'src/app/models/calificacion.model';
import { ApiService } from 'src/app/services/api.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-calificaciones-list',
  templateUrl: './calificaciones-list.component.html'
})
export class CalificacionesListComponent implements OnInit {
  items: CalificacionModel[] = [];
  private apiUrl = '/Calificacion';
  searchText: string = "";

  constructor(private apiService: ApiService) { }

  ngOnInit() {
    this.loadItems();
  }

  async loadItems() {
    let result = await this.apiService.api.get(this.apiUrl);

    this.items = [...result.data];
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