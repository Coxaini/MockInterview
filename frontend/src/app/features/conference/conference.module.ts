import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ConferenceRoutingModule } from './conference-routing.module';
import { ConferencePageComponent } from './conference-page/conference-page.component';
import { QuestionsModule } from '@features/questions/questions.module';
import { NgIconsModule } from '@ng-icons/core';
import {
    heroArrowLeft,
    heroArrowsRightLeft,
    heroStop,
} from '@ng-icons/heroicons/outline';

@NgModule({
    declarations: [ConferencePageComponent],
    imports: [
        CommonModule,
        ConferenceRoutingModule,
        QuestionsModule,
        NgIconsModule.withIcons({
            heroArrowsRightLeft,
            heroArrowLeft,
            heroStop,
        }),
    ],
})
export class ConferenceModule {}
