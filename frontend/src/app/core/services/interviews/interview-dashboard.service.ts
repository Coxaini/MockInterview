import { Injectable } from '@angular/core';
import { Interview } from '@core/models/interviews/interview';
import { UpcomingInterview } from '@core/models/interviews/upcoming-interview';
import {
    BehaviorSubject,
    forkJoin,
    map,
    Observable,
    of,
    Subject,
    switchMap,
} from 'rxjs';
import { InterviewService } from './interview.service';

@Injectable({
    providedIn: 'root',
})
export class InterviewDashboardService {
    constructor(private interviewService: InterviewService) {}

    private scheduledInterviewsSubject = new BehaviorSubject<
        UpcomingInterview[]
    >([]);

    public plannedInterviews$ = this.scheduledInterviewsSubject.asObservable();

    private recentInterviewsSubject = new BehaviorSubject<Interview[]>([]);

    public recentInterviews$ = this.recentInterviewsSubject.asObservable();

    private scheduledInterviews$ = forkJoin([
        this.interviewService.getInterviewOrders(),
        this.interviewService.getArrangedInterviews(),
    ]).pipe(
        map(([interviewOrders, arrangedInterviewsData]) => {
            return {
                upcomingInterviews: [
                    ...interviewOrders,
                    ...arrangedInterviewsData.plannedInterviews,
                ],
                recentInterviews: arrangedInterviewsData.endedInterviews,
            };
        }),
    );

    private sortedScheduledInterviews$ = this.scheduledInterviewsSubject.pipe(
        map((interviews) => {
            return interviews.sort(
                (a, b) =>
                    new Date(a.startDateTime).getTime() -
                    new Date(b.startDateTime).getTime(),
            );
        }),
    );

    public loadInterviews() {
        const loadingSubject = new Subject<boolean>();

        this.scheduledInterviews$.subscribe(
            ({ upcomingInterviews, recentInterviews }) => {
                this.scheduledInterviewsSubject.next(upcomingInterviews);
                this.recentInterviewsSubject.next(recentInterviews);
                loadingSubject.next(true);
            },
        );

        return loadingSubject.asObservable();
    }

    public addUpcomingInterview(upcomingInterview: UpcomingInterview) {
        this.scheduledInterviewsSubject.next([
            ...this.scheduledInterviewsSubject.value,
            upcomingInterview,
        ]);
    }

    public cancelUpcomingInterview(upcomingInterview: UpcomingInterview) {
        const index = this.scheduledInterviewsSubject.value.findIndex(
            (interview) => interview.id === upcomingInterview.id,
        );
        if (index > -1) {
            let cancelRequest: Observable<object>;

            if (!upcomingInterview.mate) {
                cancelRequest = this.interviewService.cancelRequestedInterview(
                    upcomingInterview.id,
                );
            } else {
                cancelRequest = this.interviewService.cancelArrangedInterview(
                    upcomingInterview.id,
                );
            }

            return cancelRequest.pipe(
                switchMap(() => {
                    const interviews = [
                        ...this.scheduledInterviewsSubject.value,
                    ];
                    interviews.splice(index, 1);
                    this.scheduledInterviewsSubject.next(interviews);
                    return of(true);
                }),
            );
        } else {
            return new Observable<boolean>((subscriber) => {
                subscriber.next(false);
                subscriber.complete();
            });
        }
    }
}
