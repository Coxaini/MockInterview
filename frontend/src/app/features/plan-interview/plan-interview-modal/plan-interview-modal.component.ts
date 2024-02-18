import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { UserRole } from '@core/models/users/user-role';
import { StepStatus } from '@shared/components/stepper/models/StepStatus';
import { InterviewPlanService } from '@features/plan-interview/services/interview-plan.service';
import { InterviewPlan } from '@features/plan-interview/models/interview-plan';
import { Router } from '@angular/router';
import { filter } from 'rxjs';
import { InterviewScheduleService } from '@core/services/interviews/interview-schedule.service';

@Component({
    selector: 'app-plan-interview-modal',
    templateUrl: './plan-interview-modal.component.html',
    styleUrl: './plan-interview-modal.component.scss',
    providers: [InterviewPlanService],
})
export class PlanInterviewModalComponent implements OnInit {
    public modalTitle: string = 'Plan Interview';

    constructor(
        private fb: FormBuilder,
        private interviewPlanService: InterviewPlanService,
        private router: Router,
        private interviewService: InterviewScheduleService,
    ) {}

    ngOnInit(): void {
        this.interviewForm
            .get('programmingLanguage')!
            .valueChanges.pipe(
                filter((language): language is string => !!language),
            )
            .subscribe((language) => {
                this.interviewPlanService.selectProgrammingLanguage(language);
            });
    }

    public interviewForm = this.fb.group({
        programmingLanguage: ['', [Validators.required]],
        technologies: [[] as string[]],
        role: this.fb.control(undefined as UserRole | undefined, [
            Validators.required,
            Validators.min(1),
        ]),
        startTime: ['', [Validators.required]],
    });

    getStepStatus(controlName: string): StepStatus {
        const control = this.interviewForm.get(controlName)!;
        if (!control.touched) {
            return StepStatus.Untouched;
        }
        return control.valid ? StepStatus.Completed : StepStatus.Invalid;
    }

    submit() {
        if (this.interviewForm.invalid) {
            return;
        }
        const request = this.interviewForm.value as InterviewPlan;

        this.interviewPlanService.planInterview(request).subscribe((order) => {
            this.interviewService.addUpcomingInterview(order);
            this.router.navigate(['/home']);
        });
    }
}
