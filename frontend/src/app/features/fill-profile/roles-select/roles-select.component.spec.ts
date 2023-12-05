import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RolesSelectComponent } from './roles-select.component';

describe('RoleSelectComponent', () => {
    let component: RolesSelectComponent;
    let fixture: ComponentFixture<RolesSelectComponent>;

    beforeEach(() => {
        TestBed.configureTestingModule({
            declarations: [RolesSelectComponent],
        });
        fixture = TestBed.createComponent(RolesSelectComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
