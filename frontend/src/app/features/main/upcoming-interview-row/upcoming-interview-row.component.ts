import { Component, Input, OnInit } from '@angular/core';
import { UpcomingInterview } from '@core/models/interviews/upcoming-interview';
import { ProgrammingLanguagesService } from '@core/services/skills/programming-languages.service';
import { TextWithIcon } from '@core/common/text-with-icon';

@Component({
    // eslint-disable-next-line @angular-eslint/component-selector
    selector: 'tr[app-upcoming-interview-row]',
    templateUrl: './upcoming-interview-row.component.html',
    styleUrl: './upcoming-interview-row.component.scss',
})
export class UpcomingInterviewRowComponent implements OnInit {
    constructor(
        private programmingLanguagesService: ProgrammingLanguagesService,
    ) {}

    ngOnInit(): void {
        this.programmingLanguageWithIcon =
            this.programmingLanguagesService.getProgrammingLanguageWithIcons(
                this.interview.programmingLanguage,
            );
    }

    @Input() public interview: UpcomingInterview;
    public programmingLanguageWithIcon?: TextWithIcon;
}
