import { Component, OnInit } from '@angular/core';
import { MateriaModel } from 'src/app/models/materia.model';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-materias-list',
  templateUrl: './materias-list.component.html'
})
export class MateriasListComponent implements OnInit {
  items: MateriaModel[] = [];
  private apiUrl = '/Materia';

  constructor(private apiService: ApiService) {}

  ngOnInit() {
    this.loadItems();
  }

  async loadItems() {
    let result = await this.apiService.api.get(this.apiUrl);

    console.log(result);

    this.items = [...result.data];
  }

  deleteItem(id: number) {
    //this.service.delete(id);
    this.loadItems();
  }
}