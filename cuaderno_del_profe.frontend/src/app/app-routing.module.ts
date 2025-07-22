import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';
import { LayoutComponent } from './layout/layout.component';
import { InicioComponent } from './pages/inicio/inicio.component';
import { LogInComponent } from './pages/Auth/login-form.component';
//Item
import { ItemListComponent } from './components/item-list/item-list.component';
import { ItemFormComponent } from './components/item-form/item-form.component';
//Materia
import { MateriasListComponent } from './pages/materias/materias-list.component';
import { MateriasFormComponent } from './pages/materias/materias-form.component';
//Perido
import { PeriodosListComponent } from './pages/periodos/periodos-list.component';
import { PeriodosFormComponent } from './pages/periodos/periodos-form.component';

const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    canActivate: [AuthGuard],
    children: [
      { path: 'inicio', component: InicioComponent },
      //{ path: 'cambiar-clave', component: CambiarClaveComponent },

      //Items
      { path: 'Items/List', component: ItemListComponent },
      { path: 'Items/new', component: ItemFormComponent },
      { path: 'Items/edit/:id', component: ItemFormComponent },
      //Materias
      { path: 'Materias/List', component: MateriasListComponent },
      { path: 'Materias/new', component: MateriasFormComponent },
      { path: 'Materias/edit/:id', component: MateriasFormComponent },
      //Peridos
      { path: 'Periodos/List', component: PeriodosListComponent },
      { path: 'Periodos/new', component: PeriodosFormComponent },
      { path: 'Periodos/edit/:id', component: PeriodosFormComponent },
    ]
  },
  { path: 'Auth/Login', component: LogInComponent },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }