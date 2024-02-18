import { Injectable } from '@angular/core';
import { Dialog } from '@angular/cdk/dialog';
import { ComponentType } from '@angular/cdk/overlay';
import { ModalContainerComponent } from '@core/components/modal-container/modal-container.component';
import { Router } from '@angular/router';
import { ConfirmationDialogComponent } from '@core/components/confirmation-dialog/confirmation-dialog.component';
import { ConfirmationDialogData } from '@core/common/confirmation-dialog-data';

@Injectable({
    providedIn: 'root',
})
export class ModalService {
    constructor(
        private cdkDialog: Dialog,
        private router: Router,
    ) {}

    openCustomModal<C, D = unknown>(
        component: ComponentType<C>,
        size?: { width: string; height: string },
        data?: D,
        closeOnNavigation = true,
    ) {
        const dialogRef = this.cdkDialog.open(component, {
            container: ModalContainerComponent,
            height: size?.height || '500px',
            width: size?.width || '500px',
            data,
        });

        if (closeOnNavigation) {
            this.router.events.subscribe(() => {
                dialogRef.close();
            });
        }

        return dialogRef;
    }

    openConfirmationDialog(
        title: string,
        message: string,
        confirmText: string = 'Yes',
        cancelText: string = 'No',
    ) {
        const dialogRef = this.cdkDialog.open<boolean>(
            ConfirmationDialogComponent,
            {
                container: ModalContainerComponent,
                height: 'auto',
                width: '500px',
                data: {
                    title,
                    message,
                    confirmText,
                    cancelText,
                } as ConfirmationDialogData,
            },
        );

        this.router.events.subscribe(() => {
            dialogRef.close();
        });

        return dialogRef;
    }
}
