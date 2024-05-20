import { Component, OnInit } from '@angular/core';
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
  pagedList: PagedList<LocalUserDto> = {} as PagedList<LocalUserDto>;
  user: LocalUserDetailDto = {} as LocalUserDetailDto;

  constructor(private userService: UserService) {
  }

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers() {
    this.userService.getAll().subscribe(pagedList => {
      this.pagedList = pagedList;
    });
  }
}
