import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Technology } from '../../models/skills/technology';

@Injectable({
    providedIn: 'root',
})
export class SkillsService {
    private prefix = 'skills';

    constructor(private httpClient: HttpClient) {}

    public getLanguages() {
        return this.httpClient.get<string[]>(`${this.prefix}/languages`);
    }

    public getTechnologies() {
        return this.httpClient.get<Technology[]>(`${this.prefix}/technologies`);
    }
}
