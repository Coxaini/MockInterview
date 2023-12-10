import { Component } from '@angular/core';
import { PlanInterviewModalComponent } from '@features/plan-interview/plan-interview-modal/plan-interview-modal.component';
import { ModalService } from '@core/services/modal/modal.service';

@Component({
    selector: 'app-home-page',
    templateUrl: './home-page.component.html',
    styleUrl: './home-page.component.scss',
})
export class HomePageComponent {
    constructor(private modalService: ModalService) {}

    openPlanInterviewModal() {
        this.modalService.openDialog(PlanInterviewModalComponent, {
            width: '600px',
            height: '700px',
        });
    }
}
