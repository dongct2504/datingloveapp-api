import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { LocalUserDetailDto } from 'src/app/_dtos/localUserDtos/localUserDetailDto';
import { LocalUserDto } from 'src/app/_dtos/localUserDtos/localUserDto';
import { PagedList } from 'src/app/_dtos/pagedList';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  // user = {} as LocalUserDetailDto;
  // user$ = {} as Observable<LocalUserDetailDto>;

  // pagedList = {} as PagedList<LocalUserDto>;
  pagedList$ = {} as Observable<PagedList<LocalUserDto>>;

  constructor(private userService: UserService) {
  }

  ngOnInit(): void {
    this.pagedList$ = this.userService.getAll();
  }
}
