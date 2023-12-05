import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { RegisterRequest } from 'src/app/features/auth/services/register-request';
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
}
