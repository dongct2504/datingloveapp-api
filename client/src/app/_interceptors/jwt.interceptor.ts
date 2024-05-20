import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, take } from 'rxjs';
import { AuthenticateService } from '../_services/authenticate.service';
import { AuthenticationDto } from '../_dtos/authenticationDtos/authenticationDto';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private authenService: AuthenticateService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let authenUser: AuthenticationDto | null = {} as AuthenticationDto;

    this.authenService.currentAuthenUser$.pipe(take(1)).subscribe(currentAuthenUser =>
      authenUser = currentAuthenUser);

    if (authenUser) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${authenUser.token}`
        }
      })
    }

    return next.handle(request);
  }
}
