import { Component, OnInit } from '@angular/core';
import { AuthenticateService } from './_services/authenticate.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'DatingLoveApp';

  constructor(private authen: AuthenticateService, private router: Router) {
  }

  ngOnInit() {
    const isLogin: boolean = this.authen.checkInitializeAuthenUser();
    if (isLogin) {
      this.router.navigateByUrl('/members');
    }
  }
}
