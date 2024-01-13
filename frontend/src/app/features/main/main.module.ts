import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MainRoutingModule } from './main-routing.module';
import { HomePageComponent } from './home-page/home-page.component';
import { LayoutModule } from '@core/components/layout/layout.module';
import { PlanInterviewModule } from '@features/plan-interview/plan-interview.module';
import { UpcomingInterviewRowComponent } from '@features/main/upcoming-interview-row/upcoming-interview-row.component';
import { NgIconsModule } from '@ng-icons/core';
import { heroCalendarDays, heroUserCircle } from '@ng-icons/heroicons/outline';

@NgModule({
    declarations: [HomePageComponent, UpcomingInterviewRowComponent],
    imports: [
        CommonModule,
        MainRoutingModule,
        LayoutModule,
        PlanInterviewModule,
        NgIconsModule.withIcons({ heroCalendarDays, heroUserCircle }),
    ],
})
export class MainModule {}
