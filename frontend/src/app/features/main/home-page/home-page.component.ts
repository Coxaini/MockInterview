import { Component, OnInit } from '@angular/core';
import { PlanInterviewModalComponent } from '@features/plan-interview/plan-interview-modal/plan-interview-modal.component';
import { ModalService } from '@core/services/modal/modal.service';
import { InterviewService } from '@core/services/interviews/interview.service';
import { Observable } from 'rxjs';
import { InterviewOrder } from '@core/models/interviews/interview-order';
import { UpcomingInterview } from '@core/models/interviews/upcoming-interview';

@Component({
    selector: 'app-home-page',
    templateUrl: './home-page.component.html',
    styleUrl: './home-page.component.scss',
})
export class HomePageComponent implements OnInit {
    constructor(
        private modalService: ModalService,
        private interviewService: InterviewService,
    ) {}

    public interviews$: Observable<UpcomingInterview[]>;

    ngOnInit(): void {
        this.interviews$ = this.interviewService.getInterviews();
    }

    isUserArrangedInterview(
        interview: InterviewOrder | UpcomingInterview,
    ): interview is UpcomingInterview {
        return 'mate' in interview;
    }

    openPlanInterviewModal() {
        this.modalService.openDialog(PlanInterviewModalComponent, {
            width: '600px',
            height: '700px',
        });
    }
}
