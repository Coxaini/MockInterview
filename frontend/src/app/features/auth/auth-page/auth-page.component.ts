import { Component } from '@angular/core';
import { AuthenticationService } from '@core/services/auth/authentication.service';

@Component({
    selector: 'app-auth-page',
    templateUrl: './auth-page.component.html',
    styleUrls: ['./auth-page.component.scss'],
})
export class AuthPageComponent {
    constructor(private authenticationService: AuthenticationService) {}

    loginByGithub() {
        this.authenticationService.loginByGithub();
    }
}
