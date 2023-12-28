import { Component, inject, OnInit } from '@angular/core';
import { ControlContainer } from '@angular/forms';
import { Technology } from 'src/app/core/models/skills/technology';
import { SkillsService } from 'src/app/core/services/skills/skills.service';
import { Tile } from 'src/app/shared/components/tile-select/tile';
import { combineLatest, map, Observable, startWith } from 'rxjs';
import { ProgrammingLanguagesService } from '@core/services/skills/programming-languages.service';

@Component({
    selector: 'app-fill-profile-skills',
    templateUrl: './fill-profile-skills.component.html',
    styleUrls: ['./fill-profile-skills.component.scss'],
    viewProviders: [
        {
            provide: ControlContainer,
            useFactory: () => inject(ControlContainer, { skipSelf: true }),
        },
    ],
})
export class FillProfileSkillsComponent implements OnInit {
    constructor(
        private skillsService: SkillsService,
        private parentContainer: ControlContainer,
        private programmingLanguagesService: ProgrammingLanguagesService,
    ) {}

    private languages: Tile[] =
        this.programmingLanguagesService.getProgrammingLanguagesWithIcons();

    ngOnInit(): void {
        this.selectedLanguages$ = this.parentContainer
            .control!.get('programmingLanguages')!
            .valueChanges.pipe(
                startWith([]),
                map((langs) => langs as string[]),
            );

        this.technologies$ = this.skillsService.getTechnologies();

        this.technologiesNames$ = combineLatest([
            this.technologies$,
            this.selectedLanguages$,
        ]).pipe(
            map(([technologies, selectedLanguages]) =>
                technologies
                    .filter(
                        (technology) =>
                            technology.programmingLanguages.some((lang) =>
                                selectedLanguages.includes(lang),
                            ) || technology.programmingLanguages.length === 0,
                    )
                    .map((technology) => technology.name),
            ),
        );
    }

    public selectedLanguages$: Observable<string[]>;

    private technologies$: Observable<Technology[]>;

    public technologiesNames$: Observable<string[]>;

    public get tiles(): Tile[] {
        return this.languages;
    }
}
