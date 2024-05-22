import { Component, OnInit } from '@angular/core';
import { AuthenticateService } from '../_services/authenticate.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  constructor(public authen: AuthenticateService, private router: Router) {
  }

  ngOnInit(): void {
  }

  logout(): void {
    this.authen.logout();
    this.router.navigateByUrl('/');
  }
}
