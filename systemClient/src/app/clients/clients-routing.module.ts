import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoleGuard } from '../shared/guards/role.guards';

const routes: Routes = [
  {
    path: 'list',
    canActivate: [RoleGuard],
    data: { roles: ['Manager'] },
    loadComponent: () =>
      import('./pages/list-clients/list-clients.component').then(
        (m) => m.ListClientsComponent
      ),
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
export default class ClientsRoutingModule {}
