import { Routes } from '@angular/router';

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
    path: 'admin',
    loadChildren: () =>
      import('./pages/admin/admin.module').then((m) => m.AdminModule),
  },
  {
    path: 'dashboard',
    loadChildren: () =>
      import('./pages/dashboard/dashboard.module').then((m) => m.DashboardModule),
  },
  {
    path: 'login',
    loadChildren: () =>
      import('./pages/login/login.module').then((m) => m.LoginModule),
  },
  {
    path: 'register',
    loadChildren: () =>
      import('./pages/register/register.module').then((m) => m.RegisterModule),
  },
  {
    path: '**',
    redirectTo: 'home'
  }
];
