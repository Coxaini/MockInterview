import { Injectable } from '@angular/core';
import { Dialog } from '@angular/cdk/dialog';
import { ComponentType } from '@angular/cdk/overlay';
import { DialogContainerComponent } from '@core/components/dialog-container/dialog-container.component';
import { Router } from '@angular/router';

@Injectable({
    providedIn: 'root',
})
export class ModalService {
    constructor(
        private cdkDialog: Dialog,
        private router: Router,
    ) {}

    openDialog<C, D = unknown>(
        component: ComponentType<C>,
        size?: { width: string; height: string },
        data?: D,
        closeOnNavigation = true,
    ) {
        const dialogRef = this.cdkDialog.open(component, {
            container: DialogContainerComponent,
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
}
