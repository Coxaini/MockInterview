/* eslint-disable @typescript-eslint/no-explicit-any */
import {
    ChangeDetectionStrategy,
    Component,
    Input,
    OnInit,
    ViewChild,
    forwardRef,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { BehaviorSubject, combineLatest, map, Observable } from 'rxjs';

@Component({
    selector: 'app-multi-select',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './multi-select.component.html',
    styleUrl: './multi-select.component.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => MultiSelectComponent),
            multi: true,
        },
    ],
})
export class MultiSelectComponent implements ControlValueAccessor, OnInit {
    @Input() set options(values: string[]) {
        this.options$.next(values);
    }

    private options$ = new BehaviorSubject<string[]>([]);

    @ViewChild('searchInput') searchInput: any;

    public selectedOptions$ = new BehaviorSubject<string[]>([]);

    public filteredOptions$: Observable<string[]>;

    searchValue$ = new BehaviorSubject<string>('');

    onChange: (value: string[]) => void;
    onTouched: () => void;
    disabled: boolean;

    ngOnInit(): void {
        const availableOptions$ = combineLatest([
            this.options$,
            this.selectedOptions$,
        ]).pipe(
            map(([options, selectedOptions]) => {
                return options.filter(
                    (option) => !selectedOptions.includes(option),
                );
            }),
        );

        this.filteredOptions$ = combineLatest([
            availableOptions$,
            this.searchValue$,
        ]).pipe(
            map(([options, searchValue]) => {
                if (!searchValue) {
                    return options;
                }
                return options.filter((option) =>
                    option.toLowerCase().includes(searchValue.toLowerCase()),
                );
            }),
        );
    }

    writeValue(obj: string[]): void {
        this.selectedOptions$.next(obj);
    }

    registerOnChange(fn: any): void {
        this.onChange = fn;
    }

    registerOnTouched(fn: any): void {
        this.onTouched = fn;
    }

    setDisabledState?(isDisabled: boolean): void {
        this.disabled = isDisabled;
    }

    searchChanged($event: Event) {
        this.searchValue$.next(($event.target as HTMLInputElement).value);
    }

    removeOption(index: number) {
        this.selectedOptions$.next(
            this.selectedOptions$.value.filter((_, i) => i !== index),
        );

        this.searchInput.nativeElement.focus();

        this.onChange(this.selectedOptions$.value);
        this.onTouched();
    }

    addOption(option: string) {
        this.selectedOptions$.next([...this.selectedOptions$.value, option]);

        this.searchInput.nativeElement.value = '';
        this.searchInput.nativeElement.focus();

        this.searchValue$.next('');

        this.onChange(this.selectedOptions$.value);
        this.onTouched();
    }
}
