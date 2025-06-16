import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class RoleGuard implements CanActivate {
  constructor(private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot): boolean {
    const allowedRoles = route.data['roles'] as string[];
    const userRole = localStorage.getItem('role');

    if (!userRole) {
      this.router.navigate(['/auth/login']);
      return false;
    }

    if (allowedRoles.includes(userRole)) {
      return true;
    }

    switch (userRole) {
      case 'Client':
        this.router.navigate(['/tickets/list']);
        break;
      case 'Support':
        this.router.navigate(['/tickets/list']);
        break;
      case 'Manager':
        this.router.navigate(['/dashboard']);
        break;
      default:
        this.router.navigate(['/auth/login']);
        break;
    }

    return false;
  }
}
