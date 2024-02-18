import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { InterviewScheduleService } from '@core/services/interviews/interview-schedule.service';
import { UpcomingInterview } from '@core/models/interviews/upcoming-interview';
import { Observable } from 'rxjs';

@Component({
    selector: 'app-upcoming-interviews-table',
    templateUrl: './upcoming-interviews-table.component.html',
    styleUrl: './upcoming-interviews-table.component.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UpcomingInterviewsTableComponent implements OnInit {
    constructor(private interviewService: InterviewScheduleService) {}

    public interviews$: Observable<UpcomingInterview[]>;

    ngOnInit(): void {
        this.interviews$ = this.interviewService.getInterviews();
    }
}
