import {
    ChangeDetectionStrategy,
    Component,
    Input,
    OnInit,
} from '@angular/core';
import { InterviewDetails } from '@core/models/interviews/interview-details';
import { ProgrammingLanguagesService } from '@core/services/skills/programming-languages.service';
import { getDuration } from '@core/utils/get-duration';
import { Observable, of } from 'rxjs';

@Component({
    selector: 'app-planned-interview-details',
    templateUrl: './planned-interview-details.component.html',
    styleUrl: './planned-interview-details.component.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PlannedInterviewDetailsComponent implements OnInit {
    @Input() public interviewDetails: InterviewDetails;

    constructor(
        private programmingLanguagesService: ProgrammingLanguagesService,
    ) {}

    public programmingLanguageIcon$: Observable<string | undefined>;

    ngOnInit(): void {
        this.programmingLanguageIcon$ = of(
            this.programmingLanguagesService.getProgrammingLanguageWithIcon(
                this.interviewDetails.programmingLanguage,
            )?.fileName,
        );
    }

    public getInterviewDuration(
        startDateTime: string,
        endDateTime: string,
    ): string {
        return getDuration(new Date(startDateTime), new Date(endDateTime));
    }
}
