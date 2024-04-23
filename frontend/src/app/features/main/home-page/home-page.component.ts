import { Component, OnInit } from '@angular/core';
import { PlanInterviewModalComponent } from '@features/plan-interview/plan-interview-modal/plan-interview-modal.component';
import { ModalService } from '@core/services/modal/modal.service';
import { InterviewDashboardService } from '@core/services/interviews/interview-dashboard.service';
import { Observable } from 'rxjs';

@Component({
    selector: 'app-home-page',
    templateUrl: './home-page.component.html',
    styleUrl: './home-page.component.scss',
})
export class HomePageComponent implements OnInit {
    constructor(
        private modalService: ModalService,
        private dashboardService: InterviewDashboardService,
    ) {}

    public plannedInterviews$ = this.dashboardService.plannedInterviews$;
    public recentInterviews$ = this.dashboardService.recentInterviews$;
    public isLoaded$: Observable<boolean>;

    openPlanInterviewModal() {
        this.modalService.openCustomModal(PlanInterviewModalComponent, {
            width: '600px',
            height: '700px',
        });
    }

    ngOnInit(): void {
        this.isLoaded$ = this.dashboardService.loadInterviews();
    }
}
