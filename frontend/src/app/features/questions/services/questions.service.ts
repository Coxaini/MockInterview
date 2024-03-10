import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Question } from '@core/models/questions/question';
import { AddQuestionRequest } from './requests/add-question';
import { UpdateQuestion } from './requests/update-question';
import { Subject } from 'rxjs';

@Injectable()
export class QuestionsService {
    private readonly prefix = 'questions-lists';

    constructor(private httpClient: HttpClient) {}

    public editingQuestionStarted$ = new Subject<Question>();

    public addQuestion(questionsListId: string, question: AddQuestionRequest) {
        return this.httpClient.post<Question>(
            `${this.prefix}/${questionsListId}/questions`,
            question,
        );
    }

    public deleteQuestion(questionsListId: string, questionId: string) {
        return this.httpClient.delete(
            `${this.prefix}/${questionsListId}/questions/${questionId}`,
        );
    }

    public submitFeedback(
        questionListId: string,
        questionId: string,
        feedback: string,
    ) {
        return this.httpClient.post<Question>(
            `${this.prefix}/${questionListId}/questions/${questionId}/feedback`,
            { feedback },
        );
    }

    public updateQuestion(questionListId: string, question: UpdateQuestion) {
        return this.httpClient.put<Question>(
            `${this.prefix}/${questionListId}/questions/${question.id}`,
            question,
        );
    }

    public moveQuestion(
        questionListId: string,
        questionId: string,
        newIndex: number,
    ) {
        return this.httpClient.post(
            `${this.prefix}/${questionListId}/questions/${questionId}/move`,
            { newIndex },
        );
    }
}
