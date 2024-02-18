import {
    Component,
    ContentChildren,
    effect,
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

    @Input() isStepBarVisible: boolean = true;

    @Input() confirmButtonText: string = 'Confirm';

    @Input() isNextButtonActiveWhenInvalid: boolean = true;

    @Output() submitEvent = new EventEmitter<void>();

    @Output() labelUpdated = new EventEmitter<string>();

    public selectedStep = signal(0);

    StepStatus = StepStatus;

    constructor() {
        effect(() => {
            this.labelUpdated.emit(this.steps.get(this.selectedStep())?.label);
        });
    }

    get isNextButtonValid() {
        return (
            this.isNextButtonActiveWhenInvalid ||
            this.steps.get(this.selectedStep())?.stepStatus ===
                StepStatus.Completed
        );
    }

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
