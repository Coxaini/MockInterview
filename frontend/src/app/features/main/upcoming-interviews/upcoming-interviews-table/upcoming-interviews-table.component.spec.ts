import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpcomingInterviewsTableComponent } from './upcoming-interviews-table.component';

describe('UpcomingInterviewsTableComponent', () => {
    let component: UpcomingInterviewsTableComponent;
    let fixture: ComponentFixture<UpcomingInterviewsTableComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [UpcomingInterviewsTableComponent],
        }).compileComponents();

        fixture = TestBed.createComponent(UpcomingInterviewsTableComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
