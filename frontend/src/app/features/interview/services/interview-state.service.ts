import { Injectable } from '@angular/core';
import { Subject, shareReplay } from 'rxjs';

@Injectable()
export class InterviewStateService {
    constructor() {}
    private interviewTagsSubject = new Subject<string[]>();

    public interviewTags$ = this.interviewTagsSubject
        .asObservable()
        .pipe(shareReplay(1));

    public setInterviewTags(tags: string[]) {
        this.interviewTagsSubject.next(tags);
    }
}
