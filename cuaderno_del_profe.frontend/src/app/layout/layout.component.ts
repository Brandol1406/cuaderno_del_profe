import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
})
export class LayoutComponent {
  constructor(public router: Router) {}

  userName: string = "admin";

  ngOnInit(){
    console.log(this.router)
  }

  logout() {
    localStorage.removeItem('token'); 
  }
}