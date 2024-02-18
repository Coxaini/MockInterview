import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserSkills } from '@core/models/skills/user-skills';
import { UserExtendedSkills } from '@core/models/skills/user-extended-skills';

@Injectable({
    providedIn: 'root',
})
export class UserSkillsService {
    private prefix = 'user/skills';

    constructor(private httpClient: HttpClient) {}

    public setUserSkills(skills: UserSkills) {
        return this.httpClient.post(`${this.prefix}`, skills);
    }

    public getUserSkills() {
        return this.httpClient.get<UserExtendedSkills>(`${this.prefix}`);
    }

    public getUserTechnologies() {
        return this.httpClient.get<string[]>(`${this.prefix}/technologies`);
    }
}
