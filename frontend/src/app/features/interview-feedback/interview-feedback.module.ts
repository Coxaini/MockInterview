import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { InterviewFeedbackRoutingModule } from './interview-feedback-routing.module';
import { InterviewFeedbackPageComponent } from './interview-feedback-page/interview-feedback-page.component';
import { ProgrammingLanguageIconComponent } from '@shared/components/programming-language-icon/programming-language-icon.component';
import { AvatarCircleComponent } from '@shared/components/avatar-circle/avatar-circle.component';
import { TextareaAutosizeDirective } from '@shared/directives/textarea-autosize.directive';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
    declarations: [InterviewFeedbackPageComponent],
    imports: [
        CommonModule,
        ReactiveFormsModule,
        InterviewFeedbackRoutingModule,
        ProgrammingLanguageIconComponent,
        AvatarCircleComponent,
        TextareaAutosizeDirective,
    ],
})
export class InterviewFeedbackModule {}
