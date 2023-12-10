import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MainRoutingModule } from './main-routing.module';
import { HomePageComponent } from './home-page/home-page.component';
import { LayoutModule } from '@core/components/layout/layout.module';
import { PlanInterviewModule } from '@features/plan-interview/plan-interview.module';

@NgModule({
    declarations: [HomePageComponent],
    imports: [
        CommonModule,
        MainRoutingModule,
        LayoutModule,
        PlanInterviewModule,
    ],
})
export class MainModule {}
