import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'filterText'
})
export class FilterTextPipe implements PipeTransform {
  transform(items: any[], searchText: string, campos: string[] = []): any[] {
    if (!items) return [];
    if (!searchText) return items;

    searchText = searchText.toLowerCase();

    return items.filter(item => {
      return campos.some(campo => {
        const valor = item[campo];
        return valor?.toString().toLowerCase().includes(searchText);
      });
    });
  }
}
