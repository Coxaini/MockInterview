import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlanInterviewModalComponent } from './plan-interview-modal.component';

describe('PlanInterviewModalComponent', () => {
    let component: PlanInterviewModalComponent;
    let fixture: ComponentFixture<PlanInterviewModalComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [PlanInterviewModalComponent],
        }).compileComponents();

        fixture = TestBed.createComponent(PlanInterviewModalComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
