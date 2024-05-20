import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { LocalUserDto } from '../_dtos/localUserDtos/localUserDto';
import { LocalUserDetailDto } from '../_dtos/localUserDtos/localUserDetailDto';
import { PagedList } from '../_dtos/pagedList';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  apiUrl: string = environment.apiUrl;

  constructor(private http: HttpClient) {
  }

  getAll(page: number = 1) {
    return this.http.get<PagedList<LocalUserDto>>(this.apiUrl + 'users', {
      params: {
        page: page.toString()
      }
    });
  }

  getById(id: string) {
    return this.http.get<LocalUserDetailDto>(this.apiUrl + `users/${id}`);
  }

  getByUsername(username: string) {
    return this.http.get<LocalUserDetailDto>(this.apiUrl + `users/${username}`);
  }
}
