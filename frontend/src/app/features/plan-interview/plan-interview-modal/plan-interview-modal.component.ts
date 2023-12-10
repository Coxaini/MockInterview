import { Component } from '@angular/core';
import { DialogRef } from '@angular/cdk/dialog';

@Component({
    selector: 'app-plan-interview-modal',
    templateUrl: './plan-interview-modal.component.html',
    styleUrl: './plan-interview-modal.component.scss',
})
export class PlanInterviewModalComponent {
    constructor(public dialogRef: DialogRef) {}
}
