import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoleGuard } from '../shared/guards/role.guards';

const routes: Routes = [
  {
    path: 'add',
    canActivate: [RoleGuard],
    data: { roles: ['Client'] },
    loadComponent: () => import('./pages/add-tickets/add-tickets.component'),
  },
  {
    path: 'list',
    canActivate: [RoleGuard],
    data: { roles: ['Client', 'Manager', 'Support'] },
    loadComponent: () => import('./pages/list-tickets/list-tickets.component'),
  },
  {
    path: 'details/:id',
    canActivate: [RoleGuard],
    data: { roles: ['Client', 'Manager', 'Support'] },
    loadComponent: () =>
      import('./pages/detail-tickets/detail-tickets.component'),
  },
  {
    path: '',
    redirectTo: 'list',
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export default class TicketsRoutingModule {}
