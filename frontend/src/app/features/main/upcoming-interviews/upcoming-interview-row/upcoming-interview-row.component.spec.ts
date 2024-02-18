import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpcomingInterviewRowComponent } from './upcoming-interview-row.component';

describe('InterviewCardComponent', () => {
    let component: UpcomingInterviewRowComponent;
    let fixture: ComponentFixture<UpcomingInterviewRowComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            declarations: [UpcomingInterviewRowComponent],
        }).compileComponents();

        fixture = TestBed.createComponent(UpcomingInterviewRowComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
