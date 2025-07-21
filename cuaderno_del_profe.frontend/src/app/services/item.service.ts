import { Injectable } from '@angular/core';
import { Item } from '../models/item.model';

@Injectable({
  providedIn: 'root'
})
export class ItemService {
  private storageKey = 'items';

  getAll(): Item[] {
    const data = localStorage.getItem(this.storageKey);
    return data ? JSON.parse(data) : [];
  }

  getById(id: number): Item | undefined {
    return this.getAll().find(item => item.id === id);
  }

  add(item: Item): void {
    const items = this.getAll();
    item.id = this.generateId(items);
    items.push(item);
    this.saveAll(items);
  }

  update(item: Item): void {
    const items = this.getAll().map(i => i.id === item.id ? item : i);
    this.saveAll(items);
  }

  delete(id: number): void {
    const items = this.getAll().filter(i => i.id !== id);
    this.saveAll(items);
  }

  private saveAll(items: Item[]): void {
    localStorage.setItem(this.storageKey, JSON.stringify(items));
  }

  private generateId(items: Item[]): number {
    return items.length > 0 ? Math.max(...items.map(i => i.id)) + 1 : 1;
  }
}