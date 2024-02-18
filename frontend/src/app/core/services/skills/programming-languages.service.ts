import { Injectable } from '@angular/core';
import { TextWithIcon } from '@core/common/text-with-icon';

@Injectable({
    providedIn: 'root',
})
export class ProgrammingLanguagesService {
    constructor() {}

    private languages: TextWithIcon[] = [
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
    ].map((skill) => ({
        name: skill.name,
        fileName: 'assets/langs/' + skill.fileName + '.svg',
    }));

    public getProgrammingLanguagesWithIcons(): TextWithIcon[] {
        return this.languages;
    }

    public getProgrammingLanguageWithIcon(
        name: string,
    ): TextWithIcon | undefined {
        return this.languages.find((skill) => skill.name === name);
    }
}
