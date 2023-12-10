import { Component } from '@angular/core';
import { CdkDialogContainer, DialogModule } from '@angular/cdk/dialog';

@Component({
    selector: 'app-dialog-container',
    standalone: true,
    imports: [DialogModule],
    template: ` <ng-template cdkPortalOutlet></ng-template> `,
    styleUrl: './dialog-container.component.scss',
})
export class DialogContainerComponent extends CdkDialogContainer {}
