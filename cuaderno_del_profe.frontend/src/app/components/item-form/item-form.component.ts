import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Item } from 'src/app/models/item.model';
import { ItemService } from 'src/app/services/item.service';

@Component({
  selector: 'app-item-form',
  templateUrl: './item-form.component.html'
})
export class ItemFormComponent implements OnInit {
  item: Item = { id: 0, name: '', description: '' };
  isEdit = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private itemService: ItemService
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (id) {
      const existingItem = this.itemService.getById(id);
      if (existingItem) {
        this.item = { ...existingItem };
        this.isEdit = true;
      }
    }
  }

  onSubmit() {
    if (this.isEdit) {
      this.itemService.update(this.item);
    } else {
      this.itemService.add(this.item);
    }
    this.router.navigate(['/Items/List']);
  }
}