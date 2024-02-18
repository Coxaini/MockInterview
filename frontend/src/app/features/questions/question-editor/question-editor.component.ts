import {
    Component,
    ElementRef,
    EventEmitter,
    Input,
    OnInit,
    Output,
    ViewChild,
} from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { EditorQuestion } from '../models/editor-question';

@Component({
    selector: 'app-question-editor',
    templateUrl: './question-editor.component.html',
    styleUrl: './question-editor.component.scss',
})
export class QuestionEditorComponent implements OnInit {
    @Input() tags: string[];
    @Input() question?: EditorQuestion;
    @Output() save = new EventEmitter<EditorQuestion>();
    @Output() cancel = new EventEmitter<void>();

    @ViewChild('textInput') input: ElementRef<HTMLElement>;

    constructor(private fb: FormBuilder) {}

    ngOnInit(): void {
        if (this.question) {
            this.questionForm.patchValue(this.question);
        }
    }

    public questionForm = this.fb.group({
        text: ['', Validators.required],
        tag: [''],
    });

    cancelEdit() {
        this.cancel.emit();
    }

    public focusOnInput() {
        this.input.nativeElement.focus();
    }

    saveQuestion() {
        const question: EditorQuestion = {
            id: this.question?.id,
            text: this.questionForm.value.text || '',
            tag: this.questionForm.value.tag || '',
        };
        if (
            question.text !== this.question?.text ||
            question.tag !== this.question?.tag
        ) {
            this.save.emit(question);
        } else {
            this.cancel.emit();
        }
        this.questionForm.reset({ tag: '' });
    }
}
