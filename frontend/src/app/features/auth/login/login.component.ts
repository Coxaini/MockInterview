import { Component } from '@angular/core';
import { AuthenticationService } from '../services/authentication.service';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginRequest } from '../services/login-request';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrl: './login.component.scss',
})
export class LoginComponent {
    constructor(
        private authService: AuthenticationService,
        private fb: FormBuilder,
        private router: Router,
    ) {}

    public loginForm = this.fb.group({
        email: ['', Validators.required],
        password: ['', Validators.required],
    });

    public login() {
        this.authService.login(this.loginForm.value as LoginRequest).subscribe({
            next: () => {
                console.log('Login success');
                this.router.navigate(['/']);
            },
            error: () => {
                console.log('Login failed');
            },
        });
    }
}
