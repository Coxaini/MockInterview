import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
    selector: 'app-selectable-role-card',
    templateUrl: './selectable-role-card.component.html',
    styleUrls: ['./selectable-role-card.component.scss'],
})
export class SelectableRoleCardComponent {
    @Input() role: string;
    @Input() description: string;
    @Input() selected = false;

    @Output() selectedChange = new EventEmitter<void>();

    onClick() {
        this.selected = true;
        this.selectedChange.emit();
    }
}
