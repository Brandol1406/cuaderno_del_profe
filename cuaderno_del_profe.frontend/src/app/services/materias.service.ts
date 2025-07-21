import { Injectable } from '@angular/core';
import { MateriaModel } from '../models/materia.model';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MateriasService {
  private storageKey = 'materias';
  private apiUrl = '/api/Materia';

  constructor(private http: HttpClient) {}

  getAll(): MateriaModel[] {
    const data = localStorage.getItem(this.storageKey);

    var x = this.http.get<any[]>(this.apiUrl).subscribe(data => {
        console.log(data);
    });


    return data ? JSON.parse(data) : [];
  }

  getById(id: number): MateriaModel | undefined {
    return this.getAll().find(item => item.idMateria === id);
  }

  add(item: MateriaModel): void {
    const items = this.getAll();
    item.idMateria = this.generateId(items);
    items.push(item);
    this.saveAll(items);
  }

  update(item: MateriaModel): void {
    const items = this.getAll().map(i => i.idMateria === item.idMateria ? item : i);
    this.saveAll(items);
  }

  delete(id: number): void {
    const items = this.getAll().filter(i => i.idMateria !== id);
    this.saveAll(items);
  }

  private saveAll(items: MateriaModel[]): void {
    localStorage.setItem(this.storageKey, JSON.stringify(items));
  }

  private generateId(items: MateriaModel[]): number {
    return items.length > 0 ? Math.max(...items.map(i => i.idMateria)) + 1 : 1;
  }
}