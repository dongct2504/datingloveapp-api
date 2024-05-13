import { Component, OnInit } from '@angular/core';
import { AuthenticateService } from '../services/authenticate.service';
import { LoginLocalUserDto } from '../dtos/authenticationDtos/loginLocalUserDto';
import { Observable } from 'rxjs';
import { LocalUserDto } from '../dtos/localUserDtos/localUserDto';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  loginLocalUserDto: LoginLocalUserDto = {
    userName: '',
    password: ''
  };

  constructor(public authen: AuthenticateService) {
  }

  ngOnInit(): void {
  }

  login(): void {
    this.authen.login(this.loginLocalUserDto).subscribe(response => {
      console.log(response);
    }, err => {
      console.log(err);
    });
  }

  logout(): void {
    this.authen.logout();
  }
}
