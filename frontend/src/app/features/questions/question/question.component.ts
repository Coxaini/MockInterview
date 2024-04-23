import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Question } from '@core/models/questions/question';
import { EditorQuestion } from '../models/editor-question';
import { QuestionsListStateService } from './../services/questions-list-state.service';
import { FormControl, Validators } from '@angular/forms';
import { debounceTime, filter } from 'rxjs';

@Component({
    selector: 'app-question',
    templateUrl: './question.component.html',
    styleUrl: './question.component.scss',
})
export class QuestionComponent implements OnInit {
    @Input({ required: true }) question: Question;
    @Input() tags: string[];
    @Output() deleteQuestion = new EventEmitter<Question>();
    @Output() editQuestion = new EventEmitter<Question>();
    @Output() nextQuestion = new EventEmitter<void>();
    @Output() previousQuestion = new EventEmitter<void>();
    @Output() clickOnQuestion = new EventEmitter<void>();
    @Output() feedbackSubmit = new EventEmitter<string>();
    @Input() isEdit = false;
    @Input() isEditable = false;

    @Input() isCurrent = false;
    @Input() isFeedbackAvailable = false;
    @Input() isFeedbackEditable = false;

    isHovered = false;

    constructor(private questionsListStateService: QuestionsListStateService) {}

    feedbackInput = new FormControl('', [Validators.maxLength(500)]);

    ngOnInit(): void {
        this.feedbackInput.setValue(this.question.feedback || '');

        this.feedbackInput.valueChanges
            .pipe(
                filter((value): value is string => value !== null),
                debounceTime(500),
            )
            .subscribe((value) => {
                this.feedbackSubmit.emit(value);
            });
    }

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

    click() {
        this.clickOnQuestion.emit();
    }

    next() {
        this.nextQuestion.emit();
        //scroll if needed
    }
    previous() {
        this.previousQuestion.emit();
    }

    onMouseLeave() {
        this.isHovered = false;
    }
    onMouseEnter() {
        this.isHovered = true;
    }
}
