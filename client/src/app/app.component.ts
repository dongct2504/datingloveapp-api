import { Component, OnInit } from '@angular/core';
import { AuthenticateService } from './_services/authenticate.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'DatingLoveApp';

  constructor(private authen: AuthenticateService) {
  }

  ngOnInit() {
    this.authen.checkInitializeAuthenUser();
  }
}
