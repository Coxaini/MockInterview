import { HttpClient } from '@angular/common/http';
import { InterviewTimeSlot } from '@features/plan-interview/models/interview-time-slot';
import { InterviewPlan } from '../models/interview-plan';
import { Subject } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable()
export class InterviewPlanService {
    private prefix = 'schedule';
    private readonly timeZoneOffset = new Date().getTimezoneOffset();

    private languageChangedSubject = new Subject<string>();
    public languageChanged$ = this.languageChangedSubject.asObservable();

    constructor(private httpClient: HttpClient) {}

    getSuggestedTimeSlots(language: string) {
        return this.httpClient.get<InterviewTimeSlot[]>(
            `${this.prefix}/suggested-times`,
            {
                params: {
                    timeZoneOffset: this.timeZoneOffset,
                    programmingLanguage: language,
                },
            },
        );
    }

    selectProgrammingLanguage(language: string) {
        this.languageChangedSubject.next(language);
    }

    planInterview(interviewPlan: InterviewPlan) {
        return this.httpClient.post(
            `${this.prefix}/plan-interview`,
            interviewPlan,
        );
    }
}
