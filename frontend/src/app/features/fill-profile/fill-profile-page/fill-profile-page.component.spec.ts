import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FillProfilePageComponent } from './fill-profile-page.component';

describe('FillProfilePageComponent', () => {
    let component: FillProfilePageComponent;
    let fixture: ComponentFixture<FillProfilePageComponent>;

    beforeEach(() => {
        TestBed.configureTestingModule({
            declarations: [FillProfilePageComponent],
        });
        fixture = TestBed.createComponent(FillProfilePageComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
