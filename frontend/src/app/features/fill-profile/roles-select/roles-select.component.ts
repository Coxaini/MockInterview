/* eslint-disable @typescript-eslint/no-explicit-any */
import { Component } from '@angular/core';
import { UserRole } from '@core/models/profile/user-role';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
    selector: 'app-roles-select',
    templateUrl: './roles-select.component.html',
    styleUrls: ['./roles-select.component.scss'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: RolesSelectComponent,
            multi: true,
        },
    ],
})
export class RolesSelectComponent implements ControlValueAccessor {
    public selectedRole: UserRole;

    onChange: (value: UserRole) => void;
    onTouched: () => void;
    disabled: boolean;

    writeValue(obj: UserRole): void {
        this.selectedRole = obj;
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

    select = (role: UserRole) => {
        if ((this.selectedRole | role) === this.selectedRole) {
            this.selectedRole = this.selectedRole & ~role;
        } else {
            this.selectedRole = this.selectedRole | role;
        }

        this.onTouched();
        this.onChange(this.selectedRole);
    };

    isSelected = (role: UserRole) => (this.selectedRole & role) === role;
    protected readonly UserRole = UserRole;
}
