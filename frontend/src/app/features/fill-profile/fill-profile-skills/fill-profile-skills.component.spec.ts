import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FillProfileSkillsComponent } from './fill-profile-skills.component';

describe('FillProfileSkillsComponent', () => {
    let component: FillProfileSkillsComponent;
    let fixture: ComponentFixture<FillProfileSkillsComponent>;

    beforeEach(() => {
        TestBed.configureTestingModule({
            declarations: [FillProfileSkillsComponent],
        });
        fixture = TestBed.createComponent(FillProfileSkillsComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
