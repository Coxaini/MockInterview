import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { InterviewDetails } from '@core/models/interviews/interview-details';
import { InterviewService } from '@core/services/interviews/interview.service';
import { Observable, switchMap, shareReplay } from 'rxjs';

@Component({
    selector: 'app-interview-order-page',
    templateUrl: './interview-order-page.component.html',
    styleUrl: './interview-order-page.component.scss',
})
export class InterviewOrderPageComponent implements OnInit {
    constructor(
        private interviewService: InterviewService,
        private route: ActivatedRoute,
    ) {}

    public interviewDetails$: Observable<InterviewDetails>;

    ngOnInit(): void {
        this.interviewDetails$ = this.route.paramMap.pipe(
            switchMap((params) => {
                const orderId = params.get('orderId');
                if (!orderId) throw new Error('No orderId');

                return this.interviewService.getRequestedInterview(orderId);
            }),
            shareReplay({ bufferSize: 1, refCount: true }),
        );
    }
}
