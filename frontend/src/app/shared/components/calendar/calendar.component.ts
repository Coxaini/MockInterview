import {
    ChangeDetectionStrategy,
    Component,
    computed,
    EventEmitter,
    Input,
    Output,
    signal,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { DateSlot } from '@shared/components/calendar/date-slot';
import { format, isEqual } from 'date-fns';
import { getDateWithoutTime } from '@core/utils/get-date-without-time';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { NgIcon, provideIcons } from '@ng-icons/core';
import {
    heroChevronDoubleLeft,
    heroChevronDoubleRight,
} from '@ng-icons/heroicons/outline';

@Component({
    selector: 'app-calendar',
    standalone: true,
    imports: [CommonModule, NgIcon],
    templateUrl: './calendar.component.html',
    styleUrl: './calendar.component.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: CalendarComponent,
            multi: true,
        },
        provideIcons({ heroChevronDoubleLeft, heroChevronDoubleRight }),
    ],
})
export class CalendarComponent implements ControlValueAccessor {
    @Input() public dateSlots: DateSlot[] = [];
    @Output() public timeSlotSelected = new EventEmitter<Date>();

    private dateNow = getDateWithoutTime(new Date());

    public selectedDateTime: Date | null = null;

    public currentDateIndex = signal(0);
    public leftDaysOffset = computed(() => {
        return (
            -this.currentDateIndex() * (50 / (this.dateSlots.length - 1)) + 25
        );
    });

    public leftTimesOffset = computed(() => {
        return `calc(${this.currentDateIndex()} * -100%)`;
    });

    public currentDateSlot = computed(() => {
        return this.dateSlots[this.currentDateIndex()];
    });

    public isToday(date: Date): boolean {
        return isEqual(date, this.dateNow);
    }

    public getShortDayName(date: Date): string {
        return format(date, 'dd');
    }

    public changeCurrentDateIndex(index: number): void {
        this.currentDateIndex.set(index);
        this.onTouched();
    }

    public selectDateTime(dateTime: Date): void {
        this.selectedDateTime = dateTime;
        this.timeSlotSelected.emit(dateTime);
        this.onTouched();
        this.onChange(dateTime);
    }

    onChange: (value: Date) => void;
    onTouched: () => void;
    disabled: boolean;

    writeValue(obj: Date): void {
        this.selectedDateTime = obj;
    }

    registerOnChange(fn: typeof this.onChange): void {
        this.onChange = fn;
    }

    registerOnTouched(fn: typeof this.onTouched): void {
        this.onTouched = fn;
    }

    setDisabledState?(isDisabled: boolean): void {
        this.disabled = isDisabled;
    }

    protected readonly navigator = navigator;

    isLastDayIndex = computed(
        () => this.currentDateIndex() === this.dateSlots.length - 1,
    );

    isFirstDayIndex = computed(() => this.currentDateIndex() === 0);

    prev() {
        if (this.currentDateIndex() > 0) {
            this.currentDateIndex.update((index) => index - 1);
        }
    }

    next() {
        if (this.currentDateIndex() < this.dateSlots.length - 1) {
            this.currentDateIndex.update((index) => index + 1);
        }
    }
}
