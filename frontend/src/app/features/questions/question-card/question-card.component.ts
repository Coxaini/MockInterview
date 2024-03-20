import { Component, Input } from '@angular/core';

@Component({
    selector: 'app-question-card',
    templateUrl: './question-card.component.html',
    styleUrl: './question-card.component.scss',
})
export class QuestionCardComponent {
    @Input() question: string;
    @Input() tag: string;
    @Input() title = 'Question';
}
