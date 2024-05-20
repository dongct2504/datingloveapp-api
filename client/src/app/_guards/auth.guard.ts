import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { Observable, map } from 'rxjs';
import { AuthenticateService } from '../_services/authenticate.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private auth: AuthenticateService) {
  }

  canActivate(): Observable<boolean> {
    // don't need to subcribe to currentUser cus it automatically subcribe for u.
    return this.auth.currentAuthenUser$.pipe(
      map(authenUser => {
        if (authenUser) return true;
        return false;
      })
    );
  }
}
