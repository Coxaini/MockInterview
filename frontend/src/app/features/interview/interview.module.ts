import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { InterviewRoutingModule } from './interview-routing.module';
import { InterviewPageComponent } from '@features/interview/interview-page/interview-page.component';
import { NgIconsModule, provideNgIconsConfig } from '@ng-icons/core';
import {
    heroCalendarDays,
    heroChatBubbleBottomCenter,
    heroChatBubbleLeft,
    heroClock,
    heroInformationCircle,
    heroTag,
    heroUserCircle,
} from '@ng-icons/heroicons/outline';
import { QuestionsModule } from '@features/questions/questions.module';
import { PlannedInterviewDetailsComponent } from './planned-interview-details/planned-interview-details.component';
import { InterviewOrderPageComponent } from './interview-order-page/interview-order-page.component';

@NgModule({
    declarations: [
        InterviewPageComponent,
        PlannedInterviewDetailsComponent,
        InterviewOrderPageComponent,
    ],
    imports: [
        CommonModule,
        InterviewRoutingModule,
        QuestionsModule,
        NgIconsModule.withIcons({
            heroCalendarDays,
            heroUserCircle,
            heroInformationCircle,
            heroTag,
            heroClock,
            heroChatBubbleLeft,
            heroChatBubbleBottomCenter,
        }),
    ],
    providers: [
        provideNgIconsConfig({
            size: '2rem',
        }),
    ],
})
export class InterviewModule {}
