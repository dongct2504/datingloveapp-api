import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthenticateService } from '../_services/authenticate.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private auth: AuthenticateService, private router: Router) {
  }

  canActivate(): Observable<boolean> | boolean {
    // don't need to subcribe to currentUser cus it automatically subcribe for u.
    if (this.auth.isLoggedGuard()) {
      return true;
    } else {
      this.router.navigateByUrl('/login');
      return false;
    }
  }
}