import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AuthenticateService } from '../_services/authenticate.service';
import { RegisterLocalUserDto } from '../_dtos/authenticationDtos/registerLocalUserDto';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter(); // emitting an event

  registerUserDto: RegisterLocalUserDto = {
    userName: '',
    email: '',
    phoneNumber: '',
    password: '',
    role: null
  };

  validationErrors?: string[];

  constructor(private authenService: AuthenticateService, private toastr: ToastrService) {
  }

  ngOnInit(): void {
  }

  register() {
    this.authenService.register(this.registerUserDto).subscribe(response => {
      console.log(response);
      this.cancel();
    }, err => {
      console.log(err);
      this.validationErrors = err;
    });
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
