import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PlanInterviewModalComponent } from '@features/plan-interview/plan-interview-modal/plan-interview-modal.component';
import { DialogModule } from '@angular/cdk/dialog';
import { ModalLayoutComponent } from '@core/components/modal-layout/modal-layout.component';

@NgModule({
    declarations: [PlanInterviewModalComponent],
    imports: [CommonModule, DialogModule, ModalLayoutComponent],
    exports: [PlanInterviewModalComponent],
})
export class PlanInterviewModule {}
