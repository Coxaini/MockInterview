import { Component, inject } from '@angular/core';
import { ControlContainer } from '@angular/forms';

@Component({
    selector: 'app-fill-profile-info',
    templateUrl: './fill-profile-info.component.html',
    styleUrls: ['./fill-profile-info.component.scss'],
    viewProviders: [
        {
            provide: ControlContainer,
            useFactory: () => inject(ControlContainer, { skipSelf: true }),
        },
    ],
})
export class FillProfileInfoComponent {
    public yearsOfExperience = [
        { value: 0, viewValue: '1-3' },
        { value: 1, viewValue: '3-5' },
        { value: 2, viewValue: '5-10' },
        { value: 3, viewValue: '10+' },
    ];
}
