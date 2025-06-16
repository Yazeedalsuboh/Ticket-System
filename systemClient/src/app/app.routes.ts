import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'auth',
    loadChildren: () => import('./auth/auth-routing.module'),
  },
  {
    path: 'tickets',
    loadChildren: () => import('./tickets/tickets-routing.module'),
  },
  {
    path: 'dashboard',
    loadChildren: () => import('./dashboard/dashboard.module'),
  },
  {
    path: 'employees',
    loadChildren: () => import('./employees/employees-routing.module'),
  },
  {
    path: 'clients',
    loadChildren: () => import('./clients/clients-routing.module'),
  },
  {
    path: 'landing',
    loadChildren: () => import('./landing/landing.module'),
  },
  {
    path: '',
    redirectTo: 'landing',
    pathMatch: 'full',
  },
  {
    path: '**',
    loadChildren: () => import('./dashboard/dashboard.module'),
  },
];
