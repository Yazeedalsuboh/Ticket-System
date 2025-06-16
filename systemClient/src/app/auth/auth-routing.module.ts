import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoleGuard } from '../shared/guards/role.guards';

const routes: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./pages/login/login.component'),
  },
  {
    path: 'signup',
    loadComponent: () => import('./pages/signup/signup.component'),
  },
  {
    path: 'change-password',
    loadComponent: () =>
      import('./pages/change-password/change-password.component'),
    canActivate: [RoleGuard],
    data: { roles: ['Client', 'Support'] },
  },
  {
    path: 'forgot-password',
    loadComponent: () =>
      import('./pages/forgot-password/forgot-password.component'),
  },
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export default class AuthRoutingModule {}
