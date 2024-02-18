import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UpcomingInterview } from '@core/models/interviews/upcoming-interview';
import { BehaviorSubject, forkJoin, map, Observable, tap } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class InterviewScheduleService {
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
    // private scheduledInterviews$ = this.getInterviewOrders();

    private sortedScheduledInterviews$ = this.scheduleInterviewsSubject.pipe(
        map((interviews) => {
            return interviews.sort(
                (a, b) =>
                    new Date(a.startDateTime).getTime() -
                    new Date(b.startDateTime).getTime(),
            );
        }),
    );

    public addUpcomingInterview(upcomingInterview: UpcomingInterview) {
        this.scheduleInterviewsSubject.next([
            ...this.scheduleInterviewsSubject.value,
            upcomingInterview,
        ]);
    }

    public cancelUpcomingInterview(upcomingInterview: UpcomingInterview) {
        const index = this.scheduleInterviewsSubject.value.findIndex(
            (interview) => interview.id === upcomingInterview.id,
        );
        if (index > -1) {
            let cancelRequest: Observable<object>;

            if (!upcomingInterview.mate) {
                cancelRequest = this.httpClient.delete(
                    `schedule/requested-interviews/${upcomingInterview.id}`,
                );
            } else {
                cancelRequest = this.httpClient.delete(
                    `interviews/${upcomingInterview.id}`,
                );
            }

            return cancelRequest.pipe(
                tap(() => {
                    const interviews = this.scheduleInterviewsSubject.value;
                    interviews.splice(index, 1);
                    this.scheduleInterviewsSubject.next(interviews);
                }),
                map(() => true),
            );
        } else {
            return new Observable<boolean>((subscriber) => {
                subscriber.next(false);
                subscriber.complete();
            });
        }
    }

    public getInterviews() {
        this.scheduledInterviews$.subscribe((interviews) => {
            return this.scheduleInterviewsSubject.next(interviews);
        });
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
