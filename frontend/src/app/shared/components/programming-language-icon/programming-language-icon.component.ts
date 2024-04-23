import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProgrammingLanguagesService } from '@core/services/skills/programming-languages.service';
import { TextWithIcon } from '@core/common/text-with-icon';

@Component({
    selector: 'app-programming-language-icon',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './programming-language-icon.component.html',
    styleUrl: './programming-language-icon.component.scss',
})
export class ProgrammingLanguageIconComponent implements OnInit {
    constructor(
        private programmingLanguagesService: ProgrammingLanguagesService,
    ) {}

    @Input({ required: true }) programmingLanguage: string;

    @Input() size: number = 32;

    public languageIcon: TextWithIcon | undefined;

    ngOnInit(): void {
        this.languageIcon =
            this.programmingLanguagesService.getProgrammingLanguageWithIcon(
                this.programmingLanguage,
            );
    }
}
