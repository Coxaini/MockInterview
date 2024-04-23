import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { InterviewDetails } from '@core/models/interviews/interview-details';
import { map } from 'rxjs';
import { UpcomingInterview } from '@core/models/interviews/upcoming-interview';
import { InterviewsData } from '@core/models/interviews/interviews-data';
import { Interview } from '@core/models/interviews/interview';

@Injectable({
    providedIn: 'root',
})
export class InterviewService {
    constructor(private httpClient: HttpClient) {}

    getInterviewDetails(id: string) {
        return this.httpClient
            .get<InterviewDetails>(`interviews/${id}/details`)
            .pipe(
                map(
                    (interview) =>
                        ({
                            ...interview,
                            type: interview.endDateTime ? 'ended' : 'arranged',
                        }) as InterviewDetails,
                ),
            );
    }

    getRequestedInterviewDetails(id: string) {
        return this.httpClient
            .get<InterviewDetails>(`interview-orders/${id}`)
            .pipe(
                map(
                    (interview) =>
                        ({
                            ...interview,
                            type: 'requested',
                        }) as InterviewDetails,
                ),
            );
    }

    getInterview(id: string) {
        return this.httpClient.get<Interview>(`interviews/${id}`);
    }

    getInterviewOrders() {
        return this.httpClient.get<UpcomingInterview[]>(
            `schedule/requested-interviews`,
        );
    }

    getArrangedInterviews() {
        return this.httpClient.get<InterviewsData>(`interviews`);
    }

    cancelRequestedInterview(id: string) {
        return this.httpClient.delete(`schedule/requested-interviews/${id}`);
    }

    cancelArrangedInterview(id: string) {
        return this.httpClient.delete(`interviews/${id}`);
    }

    submitInterviewFeedback(interviewId: string, feedback: string) {
        return this.httpClient.post(`interviews/${interviewId}/feedback`, {
            feedback,
        });
    }
}
