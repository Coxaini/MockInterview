import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AvailabilitySelectionComponent } from './availability-selection.component';

describe('AvailabililtySelectionComponent', () => {
    let component: AvailabilitySelectionComponent;
    let fixture: ComponentFixture<AvailabilitySelectionComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            declarations: [AvailabilitySelectionComponent],
        }).compileComponents();

        fixture = TestBed.createComponent(AvailabilitySelectionComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
