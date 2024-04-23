import { Component, Input } from '@angular/core';
import { Interview } from '@core/models/interviews/interview';

@Component({
    selector: 'app-recent-interviews-list',
    templateUrl: './recent-interviews-list.component.html',
    styleUrl: './recent-interviews-list.component.scss',
})
export class RecentInterviewsListComponent {
    @Input({ required: true }) interviews: Interview[];
}
