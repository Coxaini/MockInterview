import { Component, Input } from '@angular/core';
import { DialogRef } from '@angular/cdk/dialog';

@Component({
    selector: 'app-modal-layout',
    standalone: true,
    templateUrl: './modal-layout.component.html',
    styleUrl: './modal-layout.component.scss',
})
export class ModalLayoutComponent {
    @Input() title: string;

    constructor(private dialogRef: DialogRef) {}

    closeModal() {
        this.dialogRef.close();
    }
}
