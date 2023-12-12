import { Injectable } from '@angular/core';
import {
    HttpErrorResponse,
    HttpEvent,
    HttpHandler,
    HttpInterceptor,
    HttpRequest,
} from '@angular/common/http';
import { catchError, Observable, switchMap, tap } from 'rxjs';
import { AuthenticationService } from '../services/auth/authentication.service';
import { Router } from '@angular/router';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    constructor(
        private authService: AuthenticationService,
        private router: Router,
    ) {}

    intercept(
        request: HttpRequest<unknown>,
        next: HttpHandler,
    ): Observable<HttpEvent<unknown>> {
        return next.handle(request).pipe(
            catchError((error: HttpErrorResponse) => {
                if (
                    error.status === 401 &&
                    !request.url.includes('auth/refresh')
                ) {
                    return this.authService.refreshToken().pipe(
                        switchMap(() => next.handle(request)),
                        catchError((error) => {
                            this.router.navigate(['/auth/login']);
                            throw error;
                        }),
                        tap(() => console.log('refreshed')),
                    );
                }
                throw error;
            }),
        );
    }
}
