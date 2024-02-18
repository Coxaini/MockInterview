import { Component, OnInit } from '@angular/core';
import { InterviewService } from '@core/services/interviews/interview.service';
import { ActivatedRoute } from '@angular/router';
import { Observable, shareReplay, switchMap } from 'rxjs';
import { InterviewDetails } from '@core/models/interviews/interview-details';
import { InterviewStateService } from '../services/interview-state.service';

@Component({
    selector: 'app-interview-page',
    templateUrl: './interview-page.component.html',
    styleUrl: './interview-page.component.scss',
    providers: [InterviewStateService],
})
export class InterviewPageComponent implements OnInit {
    constructor(
        private interviewService: InterviewService,
        private route: ActivatedRoute,
        private interviewStateService: InterviewStateService,
    ) {}

    public interviewDetails$: Observable<InterviewDetails>;

    public canBeEdited(interview: InterviewDetails): boolean {
        const now = new Date();
        return now < new Date(interview.startDateTime);
    }

    ngOnInit(): void {
        this.interviewDetails$ = this.route.paramMap.pipe(
            switchMap((params) => {
                const interviewId = params.get('interviewId');
                if (!interviewId) throw new Error('No interviewId');

                return this.interviewService.getArrangedInterview(interviewId);
            }),
            shareReplay({ bufferSize: 1, refCount: true }),
        );

        this.interviewDetails$.subscribe((interview) => {
            this.interviewStateService.setInterviewTags(interview.tags);
        });
    }
}
