import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { InterviewDetails } from '@core/models/interviews/interview-details';
import { map } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class InterviewService {
    constructor(private httpClient: HttpClient) {}

    getArrangedInterview(id: string) {
        return this.httpClient.get<InterviewDetails>(`interviews/${id}`).pipe(
            map(
                (interview) =>
                    ({
                        ...interview,
                        type: 'arranged',
                    }) as InterviewDetails,
            ),
        );
    }

    getRequestedInterview(id: string) {
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
}
