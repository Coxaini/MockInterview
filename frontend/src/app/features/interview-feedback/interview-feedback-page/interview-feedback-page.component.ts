import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Interview } from '@core/models/interviews/interview';
import { InterviewService } from '@core/services/interviews/interview.service';
import { getDuration } from '@core/utils/get-duration';
import confetti from 'canvas-confetti';
import { interval, Observable, switchMap, takeUntil, timer } from 'rxjs';

@Component({
    selector: 'app-interview-feedback-page',
    templateUrl: './interview-feedback-page.component.html',
    styleUrl: './interview-feedback-page.component.scss',
})
export class InterviewFeedbackPageComponent implements OnInit {
    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private interviewService: InterviewService,
        private fb: FormBuilder,
    ) {}

    interview$: Observable<Interview>;

    feedbackForm = this.fb.group({
        feedback: ['', Validators.required],
    });

    ngOnInit(): void {
        this.interview$ = this.route.paramMap.pipe(
            switchMap((params) => {
                const interviewId = params.get('interviewId');
                if (!interviewId) throw new Error('No interviewId');

                return this.interviewService.getInterview(interviewId);
            }),
        );

        this.initConfetti();
    }

    submitFeedback(interviewId: string) {
        if (this.feedbackForm.invalid) return;

        this.interviewService
            .submitInterviewFeedback(
                interviewId,
                this.feedbackForm.value.feedback as string,
            )
            .subscribe({
                next: () => {
                    this.router.navigate(['/home']);
                },
            });
    }

    initConfetti() {
        const launchConfetti = interval(500);

        const stopConfetti = timer(3500);

        launchConfetti.pipe(takeUntil(stopConfetti)).subscribe((x) => {
            this.celebrate(x % 2 === 0);
        });
    }

    celebrate(isMirrored = false) {
        confetti({
            particleCount: 100,
            angle: isMirrored ? 315 : 225,
            spread: 160,
            origin: { x: isMirrored ? 0 : 1, y: 0 },
        });
    }

    getInterviewDuration(interview: Interview): string {
        return getDuration(
            new Date(interview.startDateTime),
            new Date(interview.endDateTime),
        );
    }
}
