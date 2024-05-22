import { Component, OnInit } from '@angular/core';
import { LoginLocalUserDto } from '../_dtos/authenticationDtos/loginLocalUserDto';
import { AuthenticateService } from '../_services/authenticate.service';
import { Router } from '@angular/router';
import { ValidationErrors } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginLocalUserDto: LoginLocalUserDto = {} as LoginLocalUserDto;

  validationErrors?: ValidationErrors[] = [];

  constructor(private authen: AuthenticateService, private router: Router, private toastr: ToastrService) {
  }

  ngOnInit(): void {
  }

  login(): void {
    this.authen.login(this.loginLocalUserDto).subscribe(() => {
      this.toastr.success('Đăng nhập thành công!');
      this.router.navigateByUrl('/members');
    }, err => {
      this.validationErrors = err;
    });
  }
}