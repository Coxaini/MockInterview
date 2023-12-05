import { Component, OnInit, inject } from '@angular/core';
import { ControlContainer } from '@angular/forms';
import { Technology } from 'src/app/core/models/skills/technology';
import { SkillsService } from 'src/app/core/services/skills/skills.service';
import { Tile } from 'src/app/shared/components/tile-select/tile';
import { Observable, combineLatest, map, shareReplay, startWith } from 'rxjs';

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
    ) {}

    private languages: { fileName: string; name: string }[] = [
        { name: 'C#', fileName: 'csharp' },
        { name: 'C++', fileName: 'cplusplus' },
        { name: 'C', fileName: 'c' },
        { name: 'Go', fileName: 'go' },
        { name: 'Java', fileName: 'java' },
        { name: 'JavaScript', fileName: 'javascript' },
        { name: 'PHP', fileName: 'php' },
        { name: 'Python', fileName: 'python' },
        { name: 'Ruby', fileName: 'ruby' },
        { name: 'Swift', fileName: 'swift' },
        { name: 'TypeScript', fileName: 'typescript' },
        { name: 'Rust', fileName: 'rust' },
    ];

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
        return this.languages.map((skill) => ({
            name: skill.name,
            fileName: 'assets/langs/' + skill.fileName + '.svg',
        }));
    }
}
