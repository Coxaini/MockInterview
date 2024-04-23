import {
    ChangeDetectionStrategy,
    Component,
    Input,
    OnInit,
} from '@angular/core';
import { UpcomingInterview } from '@core/models/interviews/upcoming-interview';
import { ProgrammingLanguagesService } from '@core/services/skills/programming-languages.service';
import { TextWithIcon } from '@core/common/text-with-icon';
import { ModalService } from '@core/services/modal/modal.service';
import { QuestionsListService } from '@core/services/questions/questions-list.service';
import { Router } from '@angular/router';
import { InterviewDashboardService } from '@core/services/interviews/interview-dashboard.service';

@Component({
    // eslint-disable-next-line @angular-eslint/component-selector
    selector: 'tr[app-upcoming-interview-row]',
    templateUrl: './upcoming-interview-row.component.html',
    styleUrl: './upcoming-interview-row.component.scss',
})
export class UpcomingInterviewRowComponent implements OnInit {
    constructor(
        private programmingLanguagesService: ProgrammingLanguagesService,
        private DashboardService: InterviewDashboardService,
        private modalService: ModalService,
        private questionsListService: QuestionsListService,
        private router: Router,
    ) {}

    ngOnInit(): void {
        this.programmingLanguageWithIcon =
            this.programmingLanguagesService.getProgrammingLanguageWithIcon(
                this.interview.programmingLanguage,
            );
    }

    @Input() public interview: UpcomingInterview;
    public programmingLanguageWithIcon?: TextWithIcon;

    cancelInterview() {
        this.modalService
            .openConfirmationDialog(
                'Cancel interview',
                'Are you sure you want to cancel this interview?',
                'Yes',
                'No',
            )
            .closed.subscribe((result) => {
                if (result) {
                    this.DashboardService.cancelUpcomingInterview(
                        this.interview,
                    ).subscribe();
                }
            });
    }

    navigateToDetails() {
        if (this.interview.mate) {
            this.router.navigate(['/interviews', this.interview.id]);
        } else {
            this.router.navigate(['/orders', this.interview.id]);
        }
    }

    navigateToConference() {
        this.router.navigate(['/conference', this.interview.id]);
    }
}
