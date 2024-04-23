import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecentInterviewsListComponent } from './recent-interviews-list.component';

describe('RecentInterviewsListComponent', () => {
  let component: RecentInterviewsListComponent;
  let fixture: ComponentFixture<RecentInterviewsListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [RecentInterviewsListComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RecentInterviewsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
