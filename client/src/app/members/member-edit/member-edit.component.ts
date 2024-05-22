import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { AuthenticationDto } from 'src/app/_dtos/authenticationDtos/authenticationDto';
import { LocalUserDetailDto } from 'src/app/_dtos/localUserDtos/localUserDetailDto';
import { AuthenticateService } from 'src/app/_services/authenticate.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm = {} as NgForm;

  authenUser: AuthenticationDto | null = {} as AuthenticationDto;
  member: LocalUserDetailDto = {} as LocalUserDetailDto;

  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(
    private authenService: AuthenticateService,
    private userService: UserService,
    private toastr: ToastrService) {
    this.authenService.currentAuthenUser$.pipe(take(1)).subscribe(authenUser => this.authenUser = authenUser)
  }

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember() {
    if (this.authenUser && this.authenUser.localUserDto) {
      this.userService.getById(this.authenUser.localUserDto.localUserId).subscribe(user => {
        this.member = user;
      });
    }
  }

  updateMember() {
    this.userService.update(this.member.localUserId, this.member).subscribe(() => {
      this.toastr.success('Hồ sơ của bạn đã cập nhật thành công!');
      this.editForm.reset(this.member);
    });
  }
}
