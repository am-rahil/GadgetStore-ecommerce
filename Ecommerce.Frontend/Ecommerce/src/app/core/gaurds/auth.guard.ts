import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router } from '@angular/router';
import { Authservice } from '../services/authservice';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private auth: Authservice, private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot): boolean {
    const tokenExists = this.auth.isLoggedIn();
    if (!tokenExists) {
      console.log(' No token, redirecting to login');
      this.router.navigate(['/auth/login']);
      return false;
    }

    const allowedRoles = route.data['roles'] || [];
    if (allowedRoles.length > 0) {
      const userRole = this.auth.getUserRole();
      if (!userRole || !allowedRoles.includes(userRole)) {
         console.log(' Role access denied, redirecting to home');
        this.router.navigate(['/home']);
        return false;
      }
    }

    return true;
  }
}
