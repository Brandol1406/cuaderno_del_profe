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
//Estudiantes
import { EstudiantesListComponent } from './pages/estudiantes/estudiantes-list.component';
import { EstudiantesFormComponent } from './pages/estudiantes/estudiantes-form.component';
import { CalificacionesListComponent } from './pages/calificaciones/calificaciones-list.component';
import { CalificacionesFormComponent } from './pages/calificaciones/calificaciones-form.component';

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
      //Estudiantes
      { path: 'Estudiantes/List', component: EstudiantesListComponent },
      { path: 'Estudiantes/new', component: EstudiantesFormComponent },
      { path: 'Estudiantes/edit/:id', component: EstudiantesFormComponent },
      //Calificaciones
      { path: 'Calificaciones/List', component: CalificacionesListComponent },
      { path: 'Calificaciones/new', component: CalificacionesFormComponent },
      { path: 'Calificaciones/edit/:id', component: CalificacionesFormComponent },
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