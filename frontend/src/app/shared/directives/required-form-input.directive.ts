import { AfterViewInit, Directive, ElementRef, Renderer2 } from '@angular/core';

@Directive({
    selector: '[appRequiredFormInput]',
    standalone: true,
})
export class RequiredFormInputDirective implements AfterViewInit {
    constructor(
        private el: ElementRef,
        private renderer: Renderer2,
    ) {}

    ngAfterViewInit(): void {
        // Create the asterisk element
        const asterisk = this.renderer.createElement('span');
        this.renderer.setAttribute(
            asterisk,
            'class',
            'text-error ml-1 text-lg',
        );
        const text = this.renderer.createText('*');
        this.renderer.appendChild(asterisk, text);

        // Append the asterisk element after the label text
        const label = this.el.nativeElement;

        this.renderer.appendChild(label, asterisk);
    }
}
