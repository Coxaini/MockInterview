import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FillProfileRequest } from './requests/fill-profile-request';
import { User } from '@core/models/users/user';

@Injectable({
    providedIn: 'root',
})
export class UsersService {
    private prefix = 'users/profile';

    constructor(private httpClient: HttpClient) {}

    public getUser() {
        return this.httpClient.get<User>(this.prefix);
    }

    public fillProfile(request: FillProfileRequest) {
        return this.httpClient.post<User>(this.prefix, request);
    }
}
