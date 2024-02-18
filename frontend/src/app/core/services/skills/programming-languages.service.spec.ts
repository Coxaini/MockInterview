import { TestBed } from '@angular/core/testing';

import { ProgrammingLanguagesService } from './programming-languages.service';

describe('ProgrammingLanguagesService', () => {
    let service: ProgrammingLanguagesService;

    beforeEach(() => {
        TestBed.configureTestingModule({});
        service = TestBed.inject(ProgrammingLanguagesService);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });
});
