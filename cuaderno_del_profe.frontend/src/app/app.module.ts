import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { LayoutComponent } from './layout/layout.component';
import { InicioComponent } from './pages/inicio/inicio.component';
import { LogInComponent } from './pages/Auth/login-form.component';
import { ItemListComponent } from './components/item-list/item-list.component';
import { ItemFormComponent } from './components/item-form/item-form.component';
import { MateriasListComponent } from './pages/materias/materias-list.component';
import { MateriasFormComponent } from './pages/materias/materias-form.component';
import { PeriodosFormComponent } from './pages/periodos/periodos-form.component';
import { PeriodosListComponent } from './pages/periodos/periodos-list.component';
import { EstudiantesListComponent } from './pages/estudiantes/estudiantes-list.component';
import { EstudiantesFormComponent } from './pages/estudiantes/estudiantes-form.component';
import { CalificacionesListComponent } from './pages/calificaciones/calificaciones-list.component';
import { CalificacionesFormComponent } from './pages/calificaciones/calificaciones-form.component';

@NgModule({
  declarations: [
    AppComponent, 
    LayoutComponent,
    InicioComponent,
    LogInComponent,
    ItemListComponent, 
    ItemFormComponent,
    MateriasListComponent,
    MateriasFormComponent,
    PeriodosListComponent,
    PeriodosFormComponent,
    EstudiantesListComponent,
    EstudiantesFormComponent,
    CalificacionesListComponent,
    CalificacionesFormComponent
  ],
  imports: [
    BrowserModule, 
    FormsModule, 
    AppRoutingModule,
    HttpClientModule,
    SweetAlert2Module.forRoot(),
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {}