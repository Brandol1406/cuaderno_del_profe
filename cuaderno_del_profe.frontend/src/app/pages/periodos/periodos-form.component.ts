import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PeriodoModel } from 'src/app/models/periodo.model';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-periodos-form',
  templateUrl: './periodos-form.component.html'
})
export class PeriodosFormComponent implements OnInit {
  model: PeriodoModel = { idPeriodo: 0, nombre: '', finicio: new Date(), ffin: new Date() };
  isEdit = false;
  errors: object = {  };
  private apiUrl = '/Periodo';
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
    try {
      if (this.isEdit) {
        let result = await this.apiService.api.put(`${this.apiUrl}/${this.id}`, toSend);
      } else {
        let result = await this.apiService.api.post(`${this.apiUrl}`, toSend);
      }

      this.router.navigate(['/Periodos/List']);
    }
    catch (e) {
      
      this.errors = { ...e.response.data.errors};
      console.log(this.errors);
    }
  }
}