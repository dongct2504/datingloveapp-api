import { Component, OnInit } from '@angular/core';
import { AuthenticateService } from './_services/authenticate.service';
import { AuthenticationDto } from './_dtos/authenticationDtos/authenticationDto';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'DatingLoveApp';

  constructor(private authen: AuthenticateService) {
  }

  ngOnInit() {
    this.setCurrentUser();
  }

  setCurrentUser() {
    const authenUserJson = localStorage.getItem('authenUser');
    if (authenUserJson) {
      const authenUser: AuthenticationDto = JSON.parse(authenUserJson);
      this.authen.setAuthenUser(authenUser);
    }
  }
}
