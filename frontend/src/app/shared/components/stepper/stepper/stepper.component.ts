import {
    Component,
    ContentChildren,
    EventEmitter,
    Input,
    Optional,
    Output,
    QueryList,
    signal,
} from '@angular/core';
import { StepComponent } from '../step/step.component';
import { StepStatus } from 'src/app/shared/components/stepper/models/StepStatus';

@Component({
    selector: 'app-stepper',
    //changeDetection: ChangeDetectionStrategy.OnPush,
    templateUrl: './stepper.component.html',
    styleUrls: ['./stepper.component.scss'],
})
export class StepperComponent {
    @ContentChildren(StepComponent) steps: QueryList<StepComponent>;

    @Input() isValid: boolean;

    @Optional() @Input() isStepBarVisible: boolean = true;

    @Output() submitEvent = new EventEmitter<void>();

    public selectedStep = signal(0);

    StepStatus = StepStatus;

    previousStep() {
        this.selectedStep.update((i) => i - 1);
    }

    nextStep() {
        this.selectedStep.update((i) => i + 1);
    }

    selectStep(i: number) {
        this.selectedStep.set(i);
    }

    submit() {
        this.submitEvent.emit();
    }
}
