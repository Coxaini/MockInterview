import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SetSkillsRequest } from './requests/set-skills-request';

@Injectable({
    providedIn: 'root',
})
export class UserSkillsService {
    private prefix = 'user/skills';

    constructor(private httpClient: HttpClient) {}

    public setUserSkills(skills: SetSkillsRequest) {
        return this.httpClient.post(`${this.prefix}`, skills);
    }
}
