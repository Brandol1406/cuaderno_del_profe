import { Component, OnInit, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
declare var bootstrap: any;

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
})
export class LayoutComponent {
  constructor(public router: Router) {}

  usrDDB: any;
  userName: string = "admin";

  ngOnInit(){
    
  }

  ngAfterViewInit() {
    // Inicializa dropdown manualmente si es necesario
    this.usrDDB = new bootstrap.Dropdown(document.getElementById("usrDropdownMenuButton"));
  }

  showUsrDDB(){
    this.usrDDB.show();
  }

  logout() {
    localStorage.removeItem('token'); 
  }
}