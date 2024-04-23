import { TestBed } from '@angular/core/testing';

import { InterviewDashboardService } from './interview-dashboard.service';

describe('InterviewDashboardService', () => {
    let service: InterviewDashboardService;

    beforeEach(() => {
        TestBed.configureTestingModule({});
        service = TestBed.inject(InterviewDashboardService);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });
});
