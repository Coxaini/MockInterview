import {
    ChangeDetectionStrategy,
    Component,
    ContentChildren,
    EventEmitter,
    Input,
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
