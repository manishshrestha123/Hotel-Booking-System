import { Routes } from '@angular/router';
import { adminAuthGuard } from './guards/admin-auth.guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full',
  },
  {
    path: 'home',
    loadChildren: () =>
      import('./pages/home/home.module').then((m) => m.HomeModule),
  },
  {
    path: 'rooms',
    loadChildren: () =>
      import('./pages/rooms/rooms.module').then((m) => m.RoomsModule),
  },
  {
    path: 'bookings',
    loadChildren: () =>
      import('./pages/bookings/bookings.module').then((m) => m.BookingsModule),
  },
  {
    path: 'find-booking',
    loadChildren: () =>
      import('./pages/find-booking/find-booking.module').then((m) => m.FindBookingModule),
  },
  {
    path: 'admin/login',
    loadComponent: () =>
      import('./pages/login/login.component').then((m) => m.LoginComponent),
  },
  {
    path: 'customer/login',
    loadComponent: () =>
      import('./pages/customer-login/customer-login.component').then((m) => m.CustomerLoginComponent),
  },
  {
    path: 'login',
    redirectTo: 'customer/login',
    pathMatch: 'full',
  },
  {
    path: 'register',
    redirectTo: 'admin/login',
    pathMatch: 'full',
  },
  {
    path: 'admin',
    canMatch: [adminAuthGuard],
    loadChildren: () =>
      import('./pages/admin/admin.module').then((m) => m.AdminModule),
  },
  {
    path: 'dashboard',
    canMatch: [adminAuthGuard],
    loadChildren: () =>
      import('./pages/dashboard/dashboard.module').then((m) => m.DashboardModule),
  },
  {
    path: '**',
    redirectTo: 'home'
  }
];
