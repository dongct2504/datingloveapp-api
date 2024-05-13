import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, ReplaySubject, map } from 'rxjs';
import { AuthenticationDto } from '../dtos/authenticationDtos/authenticationDto';
import { LoginLocalUserDto } from '../dtos/authenticationDtos/loginLocalUserDto';
import { LocalUserDto } from '../dtos/localUserDtos/localUserDto';

@Injectable({
  providedIn: 'root'
})
export class AuthenticateService {
  baseUrl: string = 'http://localhost:5000/api/';
  version: string = 'v1/';

  private currentUserSource = new ReplaySubject<LocalUserDto | null>(1); // special Observable object
  currentUser$ = this.currentUserSource;

  constructor(private http: HttpClient) {

  }

  login(model: LoginLocalUserDto) {
    return this.http.post(this.baseUrl + this.version + 'authen/login', model).pipe(
      map((response: any) => {
        const authenticationDto = response as AuthenticationDto;
        if (authenticationDto) {
          const userDto: LocalUserDto | null = authenticationDto.localUserDto;
          localStorage.setItem('userDto', JSON.stringify(userDto));
          this.currentUserSource.next(userDto);
        }
      })
    );
  }

  setCurrentUser(userDto: LocalUserDto) {
    this.currentUserSource.next(userDto);
  }

  logout() {
    localStorage.removeItem('userDto');
    this.currentUserSource.next(null);
  }
}
