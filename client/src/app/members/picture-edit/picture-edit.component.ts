import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { take } from 'rxjs';
import { AuthenticationDto } from 'src/app/_dtos/authenticationDtos/authenticationDto';
import { LocalUserDetailDto } from 'src/app/_dtos/localUserDtos/localUserDetailDto';
import { PictureDto } from 'src/app/_dtos/pictureDtos/pictureDto';
import { AuthenticateService } from 'src/app/_services/authenticate.service';
import { UserService } from 'src/app/_services/user.service';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-picture-edit',
  templateUrl: './picture-edit.component.html',
  styleUrls: ['./picture-edit.component.css']
})
export class PictureEditComponent implements OnInit {
  @Input() member = {} as LocalUserDetailDto;

  uploader = {} as FileUploader;
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;

  authenUser = {} as AuthenticationDto | null;

  constructor(private authenService: AuthenticateService, private userService: UserService) {
    authenService.currentAuthenUser$.pipe(take(1)).subscribe(authenUser => this.authenUser = authenUser);
  }

  ngOnInit(): void {
    this.initializeUploader();
  }

  fileOverBase(e: any) {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + `users/upload-picture/${this.authenUser?.localUserDto?.localUserId}`,
      authToken: 'Bearer ' + this.authenUser?.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    }

    this.uploader.onBuildItemForm = (fileItem, form) => {
      form.append('imageFile', fileItem._file, fileItem.file.name);
    }

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const picture = JSON.parse(response);
        this.member.pictures?.push(picture);
      }
    }
  }

  setMainPicture(picture: PictureDto) {
    this.userService.setMainPicture(this.member.localUserId, picture.pictureId).subscribe(() => {
      // set main picture in navbar
      if (this.authenUser?.localUserDto?.profilePictureUrl) {
        this.authenUser.localUserDto.profilePictureUrl = picture.imageUrl;
        this.authenService.setCurrentAuthenUser(this.authenUser);
      }

      this.member.profilePictureUrl = picture.imageUrl;
      this.member.pictures?.forEach(p => {
        if (p.isMain) {
          p.isMain = false;
        }
        if (p.pictureId === picture.pictureId) {
          p.isMain = true;
        }
      });
    });
  }

  removePicture(picture: PictureDto) {
    this.userService.removePicture(this.member.localUserId, picture.pictureId).subscribe(() => {
      if (this.member.pictures) {
        this.member.pictures = this.member.pictures.filter(p => p.pictureId !== picture.pictureId);
      }
    });
  }
}
