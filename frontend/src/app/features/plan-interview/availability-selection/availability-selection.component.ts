import { Component, inject, OnInit } from '@angular/core';
import { InterviewPlanService } from '@features/plan-interview/services/interview-plan.service';
import {
    combineLatest,
    filter,
    map,
    Observable,
    shareReplay,
    Subject,
    switchMap,
    timer,
} from 'rxjs';
import { DateSlot } from '@shared/components/calendar/date-slot';
import { getDateWithoutTime } from '@core/utils/get-date-without-time';
import { InterviewTimeSlot } from '@features/plan-interview/models/interview-time-slot';
import { ControlContainer } from '@angular/forms';
import { RecommendationLevel } from '@features/plan-interview/models/recommendation-level';
import { isEqual } from 'date-fns';

@Component({
    selector: 'app-availability-selection',
    templateUrl: './availability-selection.component.html',
    styleUrl: './availability-selection.component.scss',
    viewProviders: [
        {
            provide: ControlContainer,
            useFactory: () => inject(ControlContainer, { skipSelf: true }),
        },
    ],
})
export class AvailabilitySelectionComponent implements OnInit {
    constructor(private interviewPlanService: InterviewPlanService) {}

    public dateSlots$: Observable<DateSlot[]>;
    public interviewTimeSlots$: Observable<InterviewTimeSlot[]>;

    private selectedDateSlot$: Subject<Date> = new Subject<Date>();
    public selectedInterviewTimeSlot$: Observable<InterviewTimeSlot>;

    public currentTime$: Observable<Date>;

    ngOnInit(): void {
        this.interviewTimeSlots$ =
            this.interviewPlanService.languageChanged$.pipe(
                switchMap((lang) =>
                    this.interviewPlanService.getSuggestedTimeSlots(lang),
                ),
                shareReplay({ bufferSize: 1, refCount: true }),
            );

        this.dateSlots$ = this.interviewTimeSlots$.pipe(
            map((interviewTimeSlots) => {
                return this.mapInterviewSlots(interviewTimeSlots);
            }),
        );

        this.selectedInterviewTimeSlot$ = combineLatest([
            this.selectedDateSlot$,
            this.interviewTimeSlots$,
        ]).pipe(
            map(([selectedDate, interviewTimeSlots]) => {
                return interviewTimeSlots.find((slot) =>
                    isEqual(selectedDate, new Date(slot.startTime)),
                );
            }),
            filter((slot): slot is InterviewTimeSlot => slot !== undefined),
        );

        this.currentTime$ = timer(0, 1000).pipe(map(() => new Date()));
    }

    get timeZone(): string {
        return Intl.DateTimeFormat().resolvedOptions().timeZone;
    }

    public timeSlotSelected(date: Date): void {
        this.selectedDateSlot$.next(date);
    }

    private mapInterviewSlots(
        interviewTimeSlots: InterviewTimeSlot[],
    ): DateSlot[] {
        const dateSlots: DateSlot[] = [];
        let dateSlot: DateSlot = new DateSlot(
            getDateWithoutTime(new Date(interviewTimeSlots[0].startTime)),
        );
        for (const slot of interviewTimeSlots) {
            const currentSlotDate = getDateWithoutTime(
                new Date(slot.startTime),
            );
            if (currentSlotDate.getUTCDate() !== dateSlot.date.getUTCDate()) {
                dateSlots.push(dateSlot);
                dateSlot = new DateSlot(currentSlotDate);
            }
            dateSlot.timeSlots.push({
                startTime: new Date(slot.startTime),
                color: this.getInterviewTimeSlotColor(slot.recommendationLevel),
            });
        }
        dateSlots.push(dateSlot);
        return dateSlots;
    }

    private getInterviewTimeSlotColor(
        recommendationLevel: RecommendationLevel,
    ): string | null {
        switch (recommendationLevel) {
            case RecommendationLevel.High:
                return '#4CAF50';
            case RecommendationLevel.Medium:
                return '#FFC107';
            case RecommendationLevel.Low:
                return null;
            default:
                return null;
        }
    }

    protected readonly RecommendationLevel = RecommendationLevel;
}
