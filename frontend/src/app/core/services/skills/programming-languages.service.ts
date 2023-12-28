import { Injectable } from '@angular/core';
import { Tile } from '@shared/components/tile-select/tile';

@Injectable({
    providedIn: 'root',
})
export class ProgrammingLanguagesService {
    constructor() {}

    private languages: Tile[] = [
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

    public getProgrammingLanguagesWithIcons(): Tile[] {
        return this.languages.map((skill) => ({
            name: skill.name,
            fileName: 'assets/langs/' + skill.fileName + '.svg',
        }));
    }
}
