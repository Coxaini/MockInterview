import { Injectable } from '@angular/core';
import {
    HttpErrorResponse,
    HttpEvent,
    HttpHandler,
    HttpInterceptor,
    HttpRequest,
} from '@angular/common/http';
import {
    catchError,
    finalize,
    Observable,
    Subject,
    switchMap,
    takeWhile,
    tap,
} from 'rxjs';
import { AuthenticationService } from '../services/auth/authentication.service';
import { Router } from '@angular/router';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    constructor(
        private authService: AuthenticationService,
        private router: Router,
    ) {}

    private isRefreshing = false;
    private refreshTokenSubject$: Subject<boolean> = new Subject<boolean>();

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
                    if (this.isRefreshing) {
                        return this.refreshTokenSubject$.pipe(
                            takeWhile((isRefreshed) => isRefreshed),
                            switchMap(() => next.handle(request)),
                        );
                    }
                    this.isRefreshing = true;
                    return this.authService.refreshToken().pipe(
                        switchMap(() => next.handle(request)),
                        catchError((error) => {
                            this.authService.logout();
                            this.refreshTokenSubject$.next(false);
                            this.router.navigate(['/auth/login']);
                            throw error;
                        }),
                        tap(() => this.refreshTokenSubject$.next(true)),
                        finalize(() => (this.isRefreshing = false)),
                    );
                }
                throw error;
            }),
        );
    }
}
