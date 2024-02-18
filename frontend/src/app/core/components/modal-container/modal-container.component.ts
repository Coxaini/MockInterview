import { Component } from '@angular/core';
import { CdkDialogContainer, DialogModule } from '@angular/cdk/dialog';

@Component({
    selector: 'app-modal-container',
    standalone: true,
    imports: [DialogModule],
    template: ` <ng-template cdkPortalOutlet></ng-template> `,
    styleUrl: './modal-container.component.scss',
})
export class ModalContainerComponent extends CdkDialogContainer {}
