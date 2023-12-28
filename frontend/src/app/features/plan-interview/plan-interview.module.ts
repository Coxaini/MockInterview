import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PlanInterviewModalComponent } from '@features/plan-interview/plan-interview-modal/plan-interview-modal.component';
import { DialogModule } from '@angular/cdk/dialog';
import { ModalLayoutComponent } from '@core/components/modal-layout/modal-layout.component';
import { StepperModule } from '@shared/components/stepper/stepper.module';
import { ThemeSelectionComponent } from './theme-selection/theme-selection.component';
import { TileSelectComponent } from '@shared/components/tile-select/tile-select.component';
import { MultiSelectComponent } from '@shared/components/multi-select/multi-select.component';
import { ReactiveFormsModule } from '@angular/forms';
import { RolesSelectComponent } from '@features/plan-interview/roles-select/roles-select.component';
import { SelectableRoleCardComponent } from '@features/plan-interview/selectable-role-card/selectable-role-card.component';
import { AvailabilitySelectionComponent } from '@features/plan-interview/availability-selection/availability-selection.component';
import { CalendarComponent } from '@shared/components/calendar/calendar.component';
import { InfoCardsModule } from '@shared/components/info-cards/info-cards.module';

@NgModule({
    declarations: [
        PlanInterviewModalComponent,
        ThemeSelectionComponent,
        RolesSelectComponent,
        SelectableRoleCardComponent,
        AvailabilitySelectionComponent,
    ],
    imports: [
        CommonModule,
        ReactiveFormsModule,
        DialogModule,
        ModalLayoutComponent,
        StepperModule,
        TileSelectComponent,
        MultiSelectComponent,
        CalendarComponent,
        InfoCardsModule,
    ],
    exports: [PlanInterviewModalComponent],
})
export class PlanInterviewModule {}
