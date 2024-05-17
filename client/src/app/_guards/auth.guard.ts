import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { Observable, map, tap } from 'rxjs';
import { AuthenticateService } from '../_services/authenticate.service';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private auth: AuthenticateService, private toastr: ToastrService) {
  }

  canActivate(): Observable<boolean> {
    // don't need to subcribe to currentUser cus it automatically subcribe for u.
    return this.auth.currentUser$.pipe(
      map(user => {
        if (user) return true;
        return false;
      })
    );
  }
}
