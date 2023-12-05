import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WelcomeIntroComponent } from './welcome-intro.component';

describe('WelcomeIntroComponent', () => {
  let component: WelcomeIntroComponent;
  let fixture: ComponentFixture<WelcomeIntroComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [WelcomeIntroComponent]
    });
    fixture = TestBed.createComponent(WelcomeIntroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
