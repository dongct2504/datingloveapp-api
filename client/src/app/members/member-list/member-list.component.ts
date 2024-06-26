import { Component, OnInit } from '@angular/core';
import { LocalUserDto } from 'src/app/_dtos/localUserDtos/localUserDto';
import { UserParams } from 'src/app/_dtos/localUserDtos/userParams';
import { PagedList } from 'src/app/_dtos/pagedList';
import { AuthenticateService } from 'src/app/_services/authenticate.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  pagination?: PagedList<LocalUserDto>;
  members: LocalUserDto[] = [];
  pageNumber: number = 1;

  currentUser?: LocalUserDto | null;

  userParams = {} as UserParams;

  constructor(private userService: UserService, private userAuthen: AuthenticateService) {
    userAuthen.currentAuthenUser$.subscribe(authenUser => this.currentUser = authenUser?.localUserDto);
  }

  ngOnInit(): void {
    this.loadAllMembers();
  }

  private loadAllMembers() {
    if (this.currentUser !== undefined && this.currentUser !== null) {
      this.userParams = new UserParams(this.currentUser);
    }

    this.userService
      .getAll(this.userParams)
      .subscribe(pagedList => {
        this.pagination = pagedList;
        this.members = pagedList.items;
      })
  }

  pageChanged(event: any) {
    this.pageNumber = event.page;
    this.loadAllMembers();
  }
}
