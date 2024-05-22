import { Component, OnInit } from '@angular/core';
import { AuthenticateService } from '../_services/authenticate.service';
import { RegisterLocalUserDto } from '../_dtos/authenticationDtos/registerLocalUserDto';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  // @Output() cancelRegister = new EventEmitter(); // emitting an event

  registerUserDto: RegisterLocalUserDto = {} as RegisterLocalUserDto;

  validationErrors?: string[];

  constructor(private authenService: AuthenticateService, private router: Router, private toastr: ToastrService) {
  }

  ngOnInit(): void {
  }

  register() {
    this.authenService.register(this.registerUserDto).subscribe(() => {
      this.toastr.success('Đăng ký thành công!');
      this.router.navigateByUrl('/members');
    }, err => {
      this.validationErrors = err;
    });
  }
}
