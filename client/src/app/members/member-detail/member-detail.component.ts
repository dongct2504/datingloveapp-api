import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { LocalUserDetailDto } from 'src/app/_dtos/localUserDtos/localUserDetailDto';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  member: LocalUserDetailDto = {} as LocalUserDetailDto;

  galleryOptions: NgxGalleryOptions[] = [];
  galleryImages: NgxGalleryImage[] = [];

  constructor(private userService: UserService, private route: ActivatedRoute) {
  }

  ngOnInit(): void {
    this.getMember();

    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide
      },
      // max-width 800
      {
        breakpoint: 800,
        width: '100%',
        height: '600px',
        imagePercent: 80,
        thumbnailsPercent: 20,
        thumbnailsMargin: 20,
        thumbnailMargin: 20
      },
      // max-width 400
      {
        breakpoint: 400,
        preview: false
      }
    ];
  }

  getImages() {
    const imageUrls: NgxGalleryImage[] = [];

    if (this.member.pictures) {
      for (const picture of this.member.pictures) {
        imageUrls.push({
          small: picture.imageUrl,
          medium: picture.imageUrl,
          big: picture.imageUrl
        });
      }
    }

    return imageUrls;
  }

  getMember() {
    return this.userService.getById(this.route.snapshot.paramMap.get('username') || '').subscribe(user => {
      this.member = user;
      this.galleryImages = this.getImages();
    });
  }
}
