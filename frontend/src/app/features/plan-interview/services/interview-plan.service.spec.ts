import { TestBed } from '@angular/core/testing';

import { InterviewPlanService } from './interview-plan.service';

describe('InterviewPlanService', () => {
    let service: InterviewPlanService;

    beforeEach(() => {
        TestBed.configureTestingModule({});
        service = TestBed.inject(InterviewPlanService);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });
});
