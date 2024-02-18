import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { StepStatus } from 'src/app/shared/components/stepper/models/StepStatus';
import { UsersService } from '@core/services/users/users.service';
import { FillProfileRequest } from 'src/app/core/services/users/requests/fill-profile-request';
import { UserSkillsService } from '@core/services/skills/user-skills.service';
import { UserSkills } from '@core/models/skills/user-skills';
import { combineLatest } from 'rxjs';
import { Router } from '@angular/router';

@Component({
    selector: 'app-fill-profile-page',
    templateUrl: './fill-profile-page.component.html',
    styleUrls: ['./fill-profile-page.component.scss'],
})
export class FillProfilePageComponent {
    constructor(
        private fb: FormBuilder,
        private profileService: UsersService,
        private userSkillsService: UserSkillsService,
        private router: Router,
    ) {}

    public profileForm = this.fb.group({
        baseInfo: this.fb.group({
            name: ['', [Validators.required, Validators.minLength(3)]],
            bio: ['', [Validators.maxLength(300)]],
            yearsOfExperience: [1],
            location: [''],
        }),
        programmingLanguages: this.fb.control([] as string[], [
            Validators.required,
            Validators.minLength(1),
        ]),
        technologies: this.fb.control([] as string[]),
    });

    submit() {
        const { baseInfo, programmingLanguages, technologies } =
            this.profileForm.value;

        const profileRequest = {
            ...baseInfo,
            yearsOfExperience: Number(baseInfo?.yearsOfExperience),
        };

        combineLatest([
            this.profileService.fillProfile(
                profileRequest as FillProfileRequest,
            ),
            this.userSkillsService.setUserSkills({
                programmingLanguages,
                technologies,
            } as UserSkills),
        ]).subscribe({
            next: () => {
                this.router.navigate(['/home']);
            },
            error: (err) => {
                console.error(err);
            },
        });
    }

    getStepStatus(controlName: string): StepStatus {
        const control = this.profileForm.get(controlName)!;
        if (!control.touched) {
            return StepStatus.Untouched;
        }
        return control.valid ? StepStatus.Completed : StepStatus.Invalid;
    }
}
