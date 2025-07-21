import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { LoginModel } from '../models/login.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = '/api/Auth/login';

  constructor(private http: HttpClient) {}

  login(credentials: LoginModel): Observable<any> {
  return this.http.post(this.apiUrl, credentials).pipe(
    tap((response: any) => {
      localStorage.setItem('token', response.token);
    })
  );
}
}