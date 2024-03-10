import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MainRoutingModule } from './main-routing.module';
import { HomePageComponent } from './home-page/home-page.component';
import { LayoutModule } from '@core/components/layout/layout.module';
import { PlanInterviewModule } from '@features/plan-interview/plan-interview.module';
import { UpcomingInterviewRowComponent } from '@features/main/upcoming-interviews/upcoming-interview-row/upcoming-interview-row.component';
import { NgIconsModule } from '@ng-icons/core';
import {
    heroCalendarDays,
    heroEllipsisVertical,
    heroUserCircle,
} from '@ng-icons/heroicons/outline';
import { UpcomingInterviewsTableComponent } from '@features/main/upcoming-interviews/upcoming-interviews-table/upcoming-interviews-table.component';

@NgModule({
    declarations: [
        HomePageComponent,
        UpcomingInterviewRowComponent,
        UpcomingInterviewsTableComponent,
    ],
    imports: [
        CommonModule,
        MainRoutingModule,
        LayoutModule,
        PlanInterviewModule,
        NgIconsModule.withIcons({
            heroCalendarDays,
            heroUserCircle,
            heroEllipsisVertical,
        }),
    ],
})
export class MainModule {}
