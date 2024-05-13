import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AuthenticateService } from './services/authenticate.service';
import { LocalUserDto } from './dtos/localUserDtos/localUserDto';
import { PagedList } from './dtos/pagedList';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'DatingLoveApp';
  paging: PagedList<LocalUserDto> = {
    pageNumber: 0,
    pageSize: 0,
    totalRecords: 0,
    totalPages: 0,
    hasNextPage: false,
    hasPreviousPage: false,
    items: null
  };

  constructor(private http: HttpClient, private authen: AuthenticateService) {
  }

  ngOnInit(): void {
    this.getUsers();
    this.setCurrentUser();
  }

  getUsers(): void {
    this.http.get('http://localhost:5000/api/v1/users').subscribe((response: any) => {
      this.paging = response as PagedList<LocalUserDto>;
    }, error => {
      console.log(error);
    });
  }

  setCurrentUser(): void {
    const userDto: LocalUserDto = JSON.parse(localStorage.getItem('userDto') ?? '');
    this.authen.setCurrentUser(userDto)
  }
}
