import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RoleGuard } from '../shared/guards/role.guards';

const routes: Routes = [
  {
    path: '',
    canActivate: [RoleGuard],
    data: { roles: ['Manager'] },
    loadComponent: () => import('./dashboard/dashboard.component'),
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class DashboardRoutingModule {}
