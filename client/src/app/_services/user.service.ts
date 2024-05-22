import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { LocalUserDto } from '../_dtos/localUserDtos/localUserDto';
import { LocalUserDetailDto } from '../_dtos/localUserDtos/localUserDetailDto';
import { PagedList } from '../_dtos/pagedList';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl: string = environment.apiUrl;

  constructor(private http: HttpClient) {
  }

  getAll(page: number = 1): Observable<PagedList<LocalUserDto>> {
    return this.http.get<PagedList<LocalUserDto>>(this.baseUrl + 'users', {
      params: {
        page: page.toString()
      }
    });
  }

  getById(id: string): Observable<LocalUserDetailDto> {
    return this.http.get<LocalUserDetailDto>(this.baseUrl + `users/${id}`);
  }

  getByUsername(username: string): Observable<LocalUserDetailDto> {
    return this.http.get<LocalUserDetailDto>(this.baseUrl + `users/${username}`);
  }

  update(id: string, userDetail: LocalUserDetailDto) {
    return this.http.put(this.baseUrl + `users/${id}`, userDetail);
  }
}
