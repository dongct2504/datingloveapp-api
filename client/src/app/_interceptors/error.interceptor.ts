import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router, private toastr: ToastrService) { }

  // can intercept the request that go out, or the response (next) that comes back.
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError(err => {
        if (err) {
          switch (err.status) {
            case 400:
              if (err.error.errors) {
                const modelStateErrors = [];
                for (const key in err.error.errors) {
                  if (err.error.errors[key]) {
                    modelStateErrors.push(err.error.errors[key]); // push the value
                  }
                }
                throw modelStateErrors.flat();
              } else {
                this.toastr.error(err.error.detail);
              }
              break;
            case 401:
              this.toastr.error(err.statusText);
              break;
            case 403:
              this.toastr.error(err.statusText);
              break;
            case 404:
              this.router.navigateByUrl('/not-found');
              break;
            default:
              const navigationExtras: NavigationExtras = { state: { error: err.error } };
              this.router.navigateByUrl('/internal-server-error', navigationExtras);
              break;
          }
        }
        return throwError(() => new Error(err));
      })
    )
  }
}
