import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@env/environment';
import { RegisterRequest } from '@core/services/auth/register-request';
import { LoginRequest } from './login-request';

@Injectable({
    providedIn: 'root',
})
export class AuthenticationService {
    private prefix = 'auth';

    constructor(private httpClient: HttpClient) {}

    public loginByGithub() {
        window.location.href = environment.githubRedirectUri;
    }

    public login(request: LoginRequest) {
        return this.httpClient.post(`${this.prefix}/login`, request);
    }

    public register(request: RegisterRequest) {
        return this.httpClient.post(`${this.prefix}/register`, request);
    }

    public refreshToken() {
        return this.httpClient.post(`${this.prefix}/refresh`, {});
    }

    public logout() {
        return this.httpClient.post(`${this.prefix}/logout`, {});
    }
}
