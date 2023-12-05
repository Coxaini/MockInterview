import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BorderCardComponent } from './border-card.component';

describe('AuthCardComponent', () => {
    let component: BorderCardComponent;
    let fixture: ComponentFixture<BorderCardComponent>;

    beforeEach(() => {
        TestBed.configureTestingModule({
            declarations: [BorderCardComponent],
        });
        fixture = TestBed.createComponent(BorderCardComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
