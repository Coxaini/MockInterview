import { Injectable } from '@angular/core';
import { Question } from '@core/models/questions/question';
import { BehaviorSubject } from 'rxjs';

@Injectable()
export class QuestionsListStateService {
    constructor() {}

    public editedQuestionId$ = new BehaviorSubject<{ id?: string }>({});

    public enterEditMode(question: Question): void {
        this.editedQuestionId$.next({ id: question.id });
    }

    public exitEditMode(): void {
        this.editedQuestionId$.next({});
    }
}
