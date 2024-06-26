import { Component, OnInit } from '@angular/core';
import { AuthenticateService } from '../_services/authenticate.service';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm = {} as FormGroup;

  validationErrors?: ValidationErrors[];

  constructor(
    private authen: AuthenticateService,
    private router: Router,
    private toastr: ToastrService,
    private fb: FormBuilder) {
  }

  ngOnInit(): void {
    this.initForm();
  }

  private initForm() {
    this.loginForm = this.fb.group({
      username: ['',
        [Validators.required, Validators.maxLength(50)]
      ],
      password: ['',
        [Validators.required, Validators.minLength(3)]
      ]
    });
  }

  login(): void {
    this.authen.login(this.loginForm.value).subscribe(() => {
      this.toastr.success('Đăng nhập thành công!');
      this.router.navigateByUrl('/members');
    }, err => {
      this.validationErrors = err;
    });
  }
}