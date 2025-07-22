import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { LoginModel } from 'src/app/models/login.model';
import { ApiService } from '../../services/api.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-inicio',
  templateUrl: './login-form.component.html'
})
export class LogInComponent implements OnInit {
  model: LoginModel = { Usuario: '', Contrasena: '' };
  private apiUrl = '/Auth/login';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private apiService: ApiService
  ) { }

  ngOnInit() {

  }
  async onSubmit(e: Event) {
    e.preventDefault();

    try {
      const response = await this.apiService.api.post(this.apiUrl, {
        Usuario: this.model.Usuario,
        Contrasena: this.model.Contrasena,
      });

      if (response.data.success) {
        const { data, token } = response.data;
        localStorage.setItem('token', token);
        
        Swal.fire("Exito!", "Inicio de sesión exitoso")
        this.router.navigate(['/']);
      } else {
        alert(response.data.message);
      }
    } catch (error: any) {
       console.log('Error en inicio de sesion', error);
       alert(error.response.data.message);
    } finally {
      
    }
  }
}