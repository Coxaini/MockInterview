import { Component, Input, OnInit } from '@angular/core';
import { QuestionsList } from '@core/models/questions/questions-list';
import { QuestionsService } from '../services/questions.service';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { Question } from '@core/models/questions/question';
import { EditorQuestion } from '../models/editor-question';
import { QuestionsListStateService } from '../services/questions-list-state.service';
import { catchError, of } from 'rxjs';

@Component({
    selector: 'app-questions-list',
    templateUrl: './questions-list.component.html',
    styleUrl: './questions-list.component.scss',
    providers: [QuestionsListStateService],
})
export class QuestionsListComponent implements OnInit {
    @Input({ required: true }) questionList: QuestionsList;
    @Input() tags: string[];
    @Input() isEditable: boolean = false;
    @Input() isSelectable: boolean = false;

    currentQuestionIndex: number = 0;

    constructor(
        private questionsService: QuestionsService,
        public questionsListStateService: QuestionsListStateService,
    ) {
        questionsListStateService.editedQuestionId$.subscribe((id) => {
            if (id.id) {
                this.isQuestionFormOpened = false;
            }
        });
    }
    ngOnInit(): void {}

    public isQuestionFormOpened = false;

    drop($event: CdkDragDrop<string[]>) {
        moveItemInArray(
            this.questionList.questions,
            $event.previousIndex,
            $event.currentIndex,
        );

        this.questionsService
            .moveQuestion(
                this.questionList.id,
                this.questionList.questions[$event.currentIndex].id,
                $event.currentIndex,
            )
            .pipe(
                catchError(() => {
                    moveItemInArray(
                        this.questionList.questions,
                        $event.currentIndex,
                        $event.previousIndex,
                    );
                    return of({});
                }),
            )
            .subscribe();
    }

    nextQuestion() {
        if (
            this.currentQuestionIndex <
            this.questionList.questions.length - 1
        ) {
            this.currentQuestionIndex++;
        }
    }

    previousQuestion() {
        if (this.currentQuestionIndex > 0) {
            this.currentQuestionIndex--;
        }
    }

    public openAddQuestionForm() {
        this.questionsListStateService.exitEditMode();
        this.isQuestionFormOpened = true;
    }

    public closeAddQuestionForm() {
        this.isQuestionFormOpened = false;
    }

    public deleteQuestion(questionId: string) {
        this.questionsService
            .deleteQuestion(this.questionList.id, questionId)
            .subscribe({
                next: () => {
                    this.questionList.questions =
                        this.questionList.questions.filter(
                            (q) => q.id !== questionId,
                        );
                },
                error: () => {
                    console.log('Failed to delete question');
                },
            });
    }

    public editQuestion(question: Question) {
        this.questionsService
            .updateQuestion(this.questionList.id, question)
            .subscribe({
                next: (result) => {
                    const index = this.questionList.questions.findIndex(
                        (q) => q.id === result.id,
                    );
                    this.questionList.questions[index] = result;
                },
                error: () => {
                    console.log('Failed to update question');
                },
            });
    }

    public submitFeedback(questionId: string, feedback: string) {
        this.questionsService
            .submitFeedback(this.questionList.id, questionId, feedback)
            .subscribe({
                next: (question) => {
                    const index = this.questionList.questions.findIndex(
                        (q) => q.id === questionId,
                    );
                    this.questionList.questions[index] = question;
                },
                error: () => {
                    console.log('Failed to submit feedback');
                },
            });
    }

    public addQuestion(question: EditorQuestion) {
        this.questionsService
            .addQuestion(this.questionList.id, question)
            .subscribe({
                next: (question) => {
                    this.questionList.questions.push(question);
                },
                error: () => {
                    console.log('Failed to add question');
                },
            });
    }
}
