import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoleGuard } from '../shared/guards/role.guards';

const routes: Routes = [
  {
    path: 'register',
    canActivate: [RoleGuard],
    data: { roles: ['Manager'] },
    loadComponent: () =>
      import('./pages/register-employee/register-employee.component'),
  },
  {
    path: 'list',
    canActivate: [RoleGuard],
    data: { roles: ['Manager'] },
    loadComponent: () =>
      import('./pages/list-employee/list-employee.component'),
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
export default class EmployeesRoutingModule {}
