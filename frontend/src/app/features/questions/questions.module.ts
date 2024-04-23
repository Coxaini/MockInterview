import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { QuestionsRoutingModule } from './questions-routing.module';
import { QuestionsListComponent } from './questions-list/questions-list.component';
import { QuestionComponent } from './question/question.component';
import { QuestionsService } from './services/questions.service';
import { ReactiveFormsModule } from '@angular/forms';
import { NgIconsModule } from '@ng-icons/core';
import {
    heroBackward,
    heroChatBubbleLeft,
    heroForward,
    heroPencil,
    heroPlusSmall,
    heroTag,
    heroTrash,
} from '@ng-icons/heroicons/outline';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { QuestionEditorComponent } from './question-editor/question-editor.component';
import { TextareaAutosizeDirective } from '@shared/directives/textarea-autosize.directive';
import { QuestionCardComponent } from './question-card/question-card.component';

@NgModule({
    declarations: [
        QuestionsListComponent,
        QuestionComponent,
        QuestionEditorComponent,
        QuestionCardComponent,
    ],
    imports: [
        CommonModule,
        QuestionsRoutingModule,
        ReactiveFormsModule,
        NgIconsModule.withIcons({
            heroTrash,
            heroPencil,
            heroPlusSmall,
            heroForward,
            heroBackward,
            heroTag,
            heroChatBubbleLeft,
        }),
        DragDropModule,
        TextareaAutosizeDirective,
    ],
    exports: [QuestionsListComponent, QuestionCardComponent],
    providers: [QuestionsService],
})
export class QuestionsModule {}
