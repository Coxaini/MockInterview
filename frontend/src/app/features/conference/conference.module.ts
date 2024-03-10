import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ConferenceRoutingModule } from './conference-routing.module';
import { ConferencePageComponent } from './conference-page/conference-page.component';
import { QuestionsModule } from '@features/questions/questions.module';

@NgModule({
    declarations: [ConferencePageComponent],
    imports: [CommonModule, ConferenceRoutingModule, QuestionsModule],
})
export class ConferenceModule {}
