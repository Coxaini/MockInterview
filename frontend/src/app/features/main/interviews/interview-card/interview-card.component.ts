import { Component, Input } from '@angular/core';
import { Interview } from '@core/models/interviews/interview';
import { getDuration } from '@core/utils/get-duration';

@Component({
    selector: 'app-interview-card',
    templateUrl: './interview-card.component.html',
    styleUrl: './interview-card.component.scss',
})
export class InterviewCardComponent {
    @Input({ required: true }) interview: Interview;

    public getInterviewDuration(): string {
        return getDuration(
            new Date(this.interview.startDateTime),
            new Date(this.interview.endDateTime),
        );
    }
}
