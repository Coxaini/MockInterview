import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InterviewFeedbackPageComponent } from './interview-feedback-page.component';

describe('InterviewFeedbackPageComponent', () => {
  let component: InterviewFeedbackPageComponent;
  let fixture: ComponentFixture<InterviewFeedbackPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [InterviewFeedbackPageComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(InterviewFeedbackPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
