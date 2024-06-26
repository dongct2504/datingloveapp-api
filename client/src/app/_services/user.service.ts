import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { LocalUserDto } from '../_dtos/localUserDtos/localUserDto';
import { LocalUserDetailDto } from '../_dtos/localUserDtos/localUserDetailDto';
import { PagedList } from '../_dtos/pagedList';
import { Observable } from 'rxjs';
import { UserParams } from '../_dtos/localUserDtos/userParams';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl: string = environment.apiUrl;

  constructor(private http: HttpClient) {
  }

  getAll(userParams: UserParams): Observable<PagedList<LocalUserDto>> {
    let params = this.getUserParams(userParams);

    return this.http.get<PagedList<LocalUserDto>>(this.baseUrl + 'users', { params });
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

  setMainPicture(pictureId: string) {
    return this.http.put(this.baseUrl + `users/set-main-picture/${pictureId}`, {});
  }

  removePicture(pictureId: string) {
    return this.http.delete(this.baseUrl + `users/remove-picture/${pictureId}`);
  }

  private getUserParams(userParams: UserParams) {
    let params = new HttpParams();

    params = params.append('page', userParams.pageNumber)
    params = params.append('gender', userParams.gender)
    params = params.append('minAge', userParams.minAge)
    params = params.append('maxAge', userParams.maxAge)

    return params;
  }
}
