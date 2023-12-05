import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { RegisterRequest } from '../services/register-request';
import { AuthenticationService } from '../services/authentication.service';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrl: './register.component.scss',
})
export class RegisterComponent {
    constructor(
        private authService: AuthenticationService,
        private fb: FormBuilder,
        private router: Router,
    ) {}

    public registerForm = this.fb.group({
        email: ['', [Validators.required, Validators.email]],
        username: ['', Validators.required],
        password: ['', Validators.required],
    });

    public register() {
        this.authService
            .register(this.registerForm.value as RegisterRequest)
            .subscribe({
                next: () => {
                    console.log('register success');
                    this.router.navigate(['/fill-profile']);
                },
                error: () => {
                    console.log('register failed');
                },
            });
    }
}
