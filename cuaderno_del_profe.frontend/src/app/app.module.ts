import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { InicioComponent } from './pages/inicio/inicio.component';
import { LogInComponent } from './pages/Auth/login-form.component';
import { ItemListComponent } from './components/item-list/item-list.component';
import { ItemFormComponent } from './components/item-form/item-form.component';
import { MateriasListComponent } from './pages/materias/materias-list.component';
import { MateriasFormComponent } from './pages/materias/materias-form.component';

@NgModule({
  declarations: [
    AppComponent, 
    InicioComponent,
    LogInComponent,
    ItemListComponent, 
    ItemFormComponent,
    MateriasListComponent,
    MateriasFormComponent
  ],
  imports: [
    BrowserModule, 
    FormsModule, 
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {}