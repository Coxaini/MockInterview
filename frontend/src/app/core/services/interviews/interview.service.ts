import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UpcomingInterview } from '@core/models/interviews/upcoming-interview';
import { BehaviorSubject, forkJoin, map } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class InterviewService {
    constructor(private httpClient: HttpClient) {}

    private scheduleInterviewsSubject = new BehaviorSubject<
        UpcomingInterview[]
    >([]);

    private scheduledInterviews$ = forkJoin([
        this.getInterviewOrders(),
        this.getArrangedInterviews(),
    ]).pipe(
        map(([interviewOrders, arrangedInterviews]) => {
            return [...interviewOrders, ...arrangedInterviews];
        }),
    );

    private sortedScheduledInterviews$ = this.scheduleInterviewsSubject.pipe(
        map((interviews) => {
            return interviews.sort(
                (a, b) =>
                    new Date(a.startDateTime).getTime() -
                    new Date(b.startDateTime).getTime(),
            );
        }),
    );

    public addInterviewOrder(interviewOrder: UpcomingInterview) {
        this.scheduleInterviewsSubject.next([
            ...this.scheduleInterviewsSubject.value,
            interviewOrder,
        ]);
    }

    public getInterviews() {
        this.scheduledInterviews$.subscribe((interviews) =>
            this.scheduleInterviewsSubject.next(interviews),
        );
        return this.sortedScheduledInterviews$;
    }

    private getInterviewOrders() {
        return this.httpClient.get<UpcomingInterview[]>(
            `schedule/requested-interviews`,
        );
    }

    private getArrangedInterviews() {
        return this.httpClient.get<UpcomingInterview[]>(`interviews`);
    }
}
