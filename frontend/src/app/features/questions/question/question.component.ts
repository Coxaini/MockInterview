import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Question } from '@core/models/questions/question';
import { EditorQuestion } from '../models/editor-question';
import { QuestionsListStateService } from './../services/questions-list-state.service';

@Component({
    selector: 'app-question',
    templateUrl: './question.component.html',
    styleUrl: './question.component.scss',
})
export class QuestionComponent {
    @Input() question: Question;
    @Input() tags: string[];
    @Output() deleteQuestion = new EventEmitter<Question>();
    @Output() editQuestion = new EventEmitter<Question>();
    @Input() isEdit = false;
    @Input() isEditable = false;

    isHovered = false;

    constructor(private questionsListStateService: QuestionsListStateService) {}

    delete() {
        this.deleteQuestion.emit(this.question);
    }

    edit() {
        this.isHovered = false;
        this.questionsListStateService.enterEditMode(this.question);
    }

    save(question: EditorQuestion) {
        this.editQuestion.emit({ ...this.question, ...question });
        this.questionsListStateService.exitEditMode();
    }

    cancel() {
        this.questionsListStateService.exitEditMode();
    }

    onMouseLeave() {
        this.isHovered = false;
    }
    onMouseEnter() {
        this.isHovered = true;
    }
}
