import { NgModule } from '@angular/core';
import { CommonModule, NgOptimizedImage } from '@angular/common';
import { MainLayoutComponent } from './main-layout/main-layout.component';
import { NavbarComponent } from './navbar/navbar.component';
import { RouterModule, RouterOutlet } from '@angular/router';
import { NgIconsModule } from '@ng-icons/core';
import {
    heroArchiveBox,
    heroBars3BottomRight,
    heroBell,
    heroBellAlert,
    heroClipboardDocumentList,
    heroHome,
    heroUserCircle,
} from '@ng-icons/heroicons/outline';

@NgModule({
    declarations: [MainLayoutComponent, NavbarComponent],
    imports: [
        CommonModule,
        NgOptimizedImage,
        RouterOutlet,
        RouterModule,
        NgIconsModule.withIcons({
            heroUserCircle,
            heroBell,
            heroBellAlert,
            heroArchiveBox,
            heroClipboardDocumentList,
            heroHome,
            heroBars3BottomRight,
        }),
    ],
    exports: [MainLayoutComponent],
})
export class LayoutModule {}
