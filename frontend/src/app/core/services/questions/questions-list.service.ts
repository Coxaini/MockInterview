import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UpcomingInterview } from '@core/models/interviews/upcoming-interview';
import { QuestionsList } from '@core/models/questions/questions-list';

@Injectable({
    providedIn: 'root',
})
export class QuestionsListService {
    private readonly prefix = 'questions-lists';
    constructor(private httpClient: HttpClient) {}

    public createQuestionsList(upcomingInterview: UpcomingInterview) {
        if (!upcomingInterview.mate) {
            return this.httpClient.post<QuestionsList>(
                `${this.prefix}/from-order`,
                {},
                { params: { interviewOrderId: upcomingInterview.id } },
            );
        } else {
            return this.httpClient.post<QuestionsList>(
                `${this.prefix}/from-interview`,
                {},
                { params: { interviewId: upcomingInterview.id } },
            );
        }
    }

    public getQuestionsList(questionsListId: string) {
        return this.httpClient.get<QuestionsList>(
            `${this.prefix}/${questionsListId}`,
        );
    }
}
