import { Component, inject, OnInit } from '@angular/core';
import { UserSkillsService } from '@core/services/skills/user-skills.service';
import { TextWithIcon } from '@core/common/text-with-icon';
import { ProgrammingLanguagesService } from '@core/services/skills/programming-languages.service';
import { ControlContainer } from '@angular/forms';
import { Technology } from '@core/models/skills/technology';
import { combineLatest, map, Observable, share, startWith } from 'rxjs';

@Component({
    selector: 'app-theme-selection',
    templateUrl: './theme-selection.component.html',
    styleUrl: './theme-selection.component.scss',
    viewProviders: [
        {
            provide: ControlContainer,
            useFactory: () => inject(ControlContainer, { skipSelf: true }),
        },
    ],
})
export class ThemeSelectionComponent implements OnInit {
    userLanguages$: Observable<TextWithIcon[]>;
    private userTechnologies$: Observable<Technology[]>;

    filteredTechnologyNames$: Observable<string[]>;

    languages: TextWithIcon[] =
        this.languagesService.getProgrammingLanguagesWithIcons();

    constructor(
        private userSkillsService: UserSkillsService,
        private languagesService: ProgrammingLanguagesService,
        private parentContainer: ControlContainer,
    ) {}

    get programmingLanguageControl() {
        return this.parentContainer.control!.get('programmingLanguage')!;
    }

    get technologiesControl() {
        return this.parentContainer.control!.get('technologies')!;
    }

    ngOnInit(): void {
        const userSkills$ = this.userSkillsService
            .getUserSkills()
            .pipe(share());

        this.userLanguages$ = userSkills$.pipe(
            map((skills) =>
                this.languages.filter((lang) =>
                    skills.programmingLanguages.includes(lang.name),
                ),
            ),
        );

        this.userTechnologies$ = userSkills$.pipe(
            map((skills) => skills.technologies),
        );

        const selectedLanguage$ =
            this.programmingLanguageControl.valueChanges.pipe(
                startWith(''),
                map((langs) => langs as string),
            );

        combineLatest([this.userTechnologies$, selectedLanguage$]).subscribe(
            ([technologies, selectedLanguage]) => {
                const technologiesControlValue = this.technologiesControl
                    .value as string[];

                const selectedTechnologies = technologies.filter((tech) =>
                    technologiesControlValue.includes(tech.name),
                );

                const availableTechnologies = selectedTechnologies
                    .filter(
                        (tech) =>
                            tech.programmingLanguages.includes(
                                selectedLanguage,
                            ) || tech.programmingLanguages.length === 0,
                    )
                    .map((tech) => tech.name);

                this.technologiesControl.setValue(availableTechnologies);
            },
        );

        this.filteredTechnologyNames$ = combineLatest([
            this.userTechnologies$,
            selectedLanguage$,
        ]).pipe(
            map(([technologies, selectedLanguage]) =>
                technologies
                    .filter(
                        (technology) =>
                            technology.programmingLanguages.includes(
                                selectedLanguage,
                            ) || technology.programmingLanguages.length === 0,
                    )
                    .map((technology) => technology.name),
            ),
        );
    }
}
