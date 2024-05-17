import { Component, OnInit } from '@angular/core';
import { AuthenticateService } from '../_services/authenticate.service';
import { LoginLocalUserDto } from '../_dtos/authenticationDtos/loginLocalUserDto';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

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

  constructor(public authen: AuthenticateService, private router: Router, private toastr: ToastrService) {
  }

  ngOnInit(): void {
  }

  login(): void {
    this.authen.login(this.loginLocalUserDto).subscribe(response => {
      this.router.navigateByUrl('/members');
    }, err => {
      console.log(err);
      this.toastr.error(err.error.detail);
    });
  }

  logout(): void {
    this.authen.logout();
    this.router.navigateByUrl('/');
  }
}
