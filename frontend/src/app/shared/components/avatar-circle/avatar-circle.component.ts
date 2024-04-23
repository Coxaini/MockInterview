import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgIconComponent, provideIcons } from '@ng-icons/core';
import { heroUserCircle } from '@ng-icons/heroicons/outline';

@Component({
    selector: 'app-avatar-circle',
    standalone: true,
    imports: [CommonModule, NgIconComponent],
    templateUrl: './avatar-circle.component.html',
    styleUrl: './avatar-circle.component.scss',
    providers: [provideIcons({ heroUserCircle })],
})
export class AvatarCircleComponent {
    @Input() avatarUrl: string | undefined;
    @Input() size = 48;
}
