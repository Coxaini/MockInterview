import { Component, Input, TemplateRef, ViewChild } from '@angular/core';
import { Observable } from 'rxjs';
import { StepStatus } from '../models/StepStatus';

@Component({
    selector: 'app-step',
    templateUrl: './step.component.html',
    styleUrls: ['./step.component.scss'],
    //changeDetection: ChangeDetectionStrategy.OnPush,
})
export class StepComponent {
    @Input() label: string;
    @Input() stepStatus: StepStatus = StepStatus.Completed;

    @ViewChild(TemplateRef, { static: true }) content: TemplateRef<unknown>;

    public completed$: Observable<boolean>;
}
