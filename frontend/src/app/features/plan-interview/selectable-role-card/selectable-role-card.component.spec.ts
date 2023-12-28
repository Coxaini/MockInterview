import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectableRoleCardComponent } from './selectable-role-card.component';

describe('SelectableRoleCardComponent', () => {
    let component: SelectableRoleCardComponent;
    let fixture: ComponentFixture<SelectableRoleCardComponent>;

    beforeEach(() => {
        TestBed.configureTestingModule({
            declarations: [SelectableRoleCardComponent],
        });
        fixture = TestBed.createComponent(SelectableRoleCardComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
