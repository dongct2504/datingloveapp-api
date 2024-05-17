import { Component, OnInit } from '@angular/core';
import { AuthenticateService } from './_services/authenticate.service';
import { LocalUserDto } from './_dtos/localUserDtos/localUserDto';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'DatingLoveApp';

  constructor(private authen: AuthenticateService) {
  }

  ngOnInit(): void {
    this.setCurrentUser();
  }

  setCurrentUser(): void {
    const userDtoFromStorage = localStorage.getItem('userDto');
    if (userDtoFromStorage) {
      const userDto: LocalUserDto = JSON.parse(userDtoFromStorage);
      this.authen.setCurrentUser(userDto)
    }
  }
}
