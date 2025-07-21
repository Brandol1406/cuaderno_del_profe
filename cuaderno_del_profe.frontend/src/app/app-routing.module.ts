import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InicioComponent } from './pages/inicio/inicio.component';
import { LogInComponent } from './pages/Auth/login-form.component';
//Item
import { ItemListComponent } from './components/item-list/item-list.component';
import { ItemFormComponent } from './components/item-form/item-form.component';
//Materia
import { MateriasListComponent } from './pages/materias/materias-list.component';
import { MateriasFormComponent } from './pages/materias/materias-form.component';

const routes: Routes = [
  { path: '', component: InicioComponent },
  { path: 'Auth/Login', component: LogInComponent },
  //Items
  { path: 'Items/List', component: ItemListComponent },
  { path: 'Items/new', component: ItemFormComponent },
  { path: 'Items/edit/:id', component: ItemFormComponent },
  //Materia
  { path: 'Materias/List', component: MateriasListComponent },
  { path: 'Materias/new', component: MateriasFormComponent },
  { path: 'Materias/edit/:id', component: MateriasFormComponent },

  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}