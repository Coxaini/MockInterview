import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ModalLayoutComponent } from '../modal-layout/modal-layout.component';
import { DIALOG_DATA, DialogRef } from '@angular/cdk/dialog';
import { ConfirmationDialogData } from '@core/common/confirmation-dialog-data';

@Component({
    selector: 'app-confirmation-dialog',
    standalone: true,
    templateUrl: './confirmation-dialog.component.html',
    styleUrl: './confirmation-dialog.component.scss',
    imports: [CommonModule, ModalLayoutComponent],
})
export class ConfirmationDialogComponent {
    constructor(
        @Inject(DIALOG_DATA) public data: ConfirmationDialogData,
        public dialogRef: DialogRef<boolean>,
    ) {}

    close() {
        this.dialogRef.close(false);
    }

    confirm() {
        this.dialogRef.close(true);
    }
}
