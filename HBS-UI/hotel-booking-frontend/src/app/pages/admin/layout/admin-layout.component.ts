import { Component } from '@angular/core';

@Component({
  selector: 'app-admin-layout',
  templateUrl: './admin-layout.component.html',
  styleUrls: ['./admin-layout.component.scss'],
  standalone: false
})
export class AdminLayoutComponent {
  
  logout() {
    localStorage.removeItem('access_token');
    localStorage.removeItem('user_role');
    window.location.href = '/login';
  }
}
