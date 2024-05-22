import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject, map } from 'rxjs';
import { AuthenticationDto } from '../_dtos/authenticationDtos/authenticationDto';
import { LoginLocalUserDto } from '../_dtos/authenticationDtos/loginLocalUserDto';
import { RegisterLocalUserDto } from '../_dtos/authenticationDtos/registerLocalUserDto';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class AuthenticateService {
  apiUrl: string = environment.apiUrl;

  private currentAuthenUserSource = new ReplaySubject<AuthenticationDto | null>(1); // special Observable object
  currentAuthenUser$ = this.currentAuthenUserSource.asObservable();

  constructor(private http: HttpClient) {
  }

  login(model: LoginLocalUserDto) {
    return this.http.post(this.apiUrl + 'authen/login', model).pipe(
      map(response => {
        const authenticationDto = response as AuthenticationDto;
        if (authenticationDto) {
          localStorage.setItem('authenUser', JSON.stringify(authenticationDto));
          this.currentAuthenUserSource.next(authenticationDto);
        }
      })
    );
  }

  register(model: RegisterLocalUserDto) {
    return this.http.post(this.apiUrl + 'authen/register', model).pipe(
      map(response => {
        const authenticationDto = response as AuthenticationDto;
        if (authenticationDto) {
          localStorage.setItem('authenUser', JSON.stringify(authenticationDto));
          this.currentAuthenUserSource.next(authenticationDto);
        }
      })
    )
  }

  logout() {
    localStorage.removeItem('authenUser');
    this.currentAuthenUserSource.next(null);
  }

  checkInitializeAuthenUser() {
    const authenUserJson = localStorage.getItem('authenUser');
    if (authenUserJson) {
      const authenUser: AuthenticationDto = JSON.parse(authenUserJson);
      this.currentAuthenUserSource.next(authenUser);
    }
  }

  isLoggedGuard(): boolean {
    return localStorage.getItem('authenUser') !== null;
  }
}
