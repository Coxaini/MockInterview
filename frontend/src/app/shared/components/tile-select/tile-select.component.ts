/* eslint-disable @typescript-eslint/no-explicit-any */
import { CommonModule } from '@angular/common';
import {
    ChangeDetectionStrategy,
    Component,
    Input,
    WritableSignal,
    signal,
    computed,
} from '@angular/core';
import { Tile } from './tile';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
    selector: 'app-tile-select',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './tile-select.component.html',
    styleUrls: ['./tile-select.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: TileSelectComponent,
            multi: true,
        },
    ],
})
export class TileSelectComponent implements ControlValueAccessor {
    @Input() public tiles: Tile[] = [];

    public selectedTiles: WritableSignal<string[]> = signal([]);

    onChange: (value: string[]) => void;
    onTouched: () => void;
    disabled: boolean;

    writeValue(obj: string[]): void {
        this.selectedTiles.set(obj);
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

    isSelected = (tile: string) =>
        computed(() => this.selectedTiles().includes(tile));

    select(tile: string) {
        if (this.disabled) {
            return;
        }

        this.selectedTiles.update((tiles) => {
            if (tiles.includes(tile)) {
                return tiles.filter((s) => s !== tile);
            } else {
                return [...tiles, tile];
            }
        });
        this.onTouched();
        this.onChange(this.selectedTiles());
    }
}
