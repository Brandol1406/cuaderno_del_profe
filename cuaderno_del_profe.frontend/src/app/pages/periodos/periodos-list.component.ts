import { Component, OnInit } from '@angular/core';
import { PeriodoModel } from 'src/app/models/periodo.model';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-periodos-list',
  templateUrl: './periodos-list.component.html'
})
export class PeriodosListComponent implements OnInit {
  items: PeriodoModel[] = [];
  private apiUrl = '/Periodo';

  constructor(private apiService: ApiService) {}

  ngOnInit() {
    this.loadItems();
  }

  async loadItems() {
    let result = await this.apiService.api.get(this.apiUrl);

    this.items = [...result.data];
  }

  async deleteItem(id: number) {
    try {
      let result = await this.apiService.api.delete(`${this.apiUrl}/${id}`);

      this.loadItems();
    }
    catch (e) {
      alert(e.response.data.message);
      console.log(e);
    }
  }
}