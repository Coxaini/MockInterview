import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FillProfileInfoComponent } from './fill-profile-info.component';

describe('FillProfileInfoComponent', () => {
    let component: FillProfileInfoComponent;
    let fixture: ComponentFixture<FillProfileInfoComponent>;

    beforeEach(() => {
        TestBed.configureTestingModule({
            declarations: [FillProfileInfoComponent],
        });
        fixture = TestBed.createComponent(FillProfileInfoComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
