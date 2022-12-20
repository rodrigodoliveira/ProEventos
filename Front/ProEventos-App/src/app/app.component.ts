import { Component } from '@angular/core';
import { User } from './models/identity/User';
import { AccountService } from './services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  public user = {} as User;

  constructor(public accountService: AccountService) {

  }

  ngOnInit(): void {

    this.setCuurentUser();

  }
  setCuurentUser(): void {

    const userStorageName = 'user';

    if (localStorage.getItem(userStorageName)) {
      this.user = JSON.parse(localStorage.getItem(userStorageName) ?? '{}');
    } else {
      this.user = null;
    }

    if (this.user) {
      this.accountService.setCurrentUser(this.user);
    }


  }
}
