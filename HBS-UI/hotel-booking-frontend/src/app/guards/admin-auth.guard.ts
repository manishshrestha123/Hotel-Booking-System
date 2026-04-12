import { inject } from '@angular/core';
import { CanMatchFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const adminAuthGuard: CanMatchFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.hasRole('SuperAdmin', 'Admin', 'Staff')) {
    return true;
  }

  return router.createUrlTree(['/admin/login']);
};
