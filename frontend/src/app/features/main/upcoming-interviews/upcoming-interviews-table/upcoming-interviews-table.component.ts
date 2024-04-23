import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { UpcomingInterview } from '@core/models/interviews/upcoming-interview';

@Component({
    selector: 'app-upcoming-interviews-table',
    templateUrl: './upcoming-interviews-table.component.html',
    styleUrl: './upcoming-interviews-table.component.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UpcomingInterviewsTableComponent {
    constructor() {}

    @Input({ required: true }) interviews: UpcomingInterview[];
}
