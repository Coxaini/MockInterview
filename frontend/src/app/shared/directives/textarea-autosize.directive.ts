import {
    AfterViewInit,
    Directive,
    ElementRef,
    HostListener,
    OnInit,
    Renderer2,
} from '@angular/core';

@Directive({
    selector: '[appTextareaAutosize]',
    standalone: true,
})
export class TextareaAutosizeDirective implements AfterViewInit {
    constructor(
        private elementRef: ElementRef,
        private renderer: Renderer2,
    ) {}

    @HostListener(':input')
    onInput() {
        this.resize();
    }

    @HostListener('window:resize')
    onResize() {
        this.resize();
    }

    ngAfterViewInit(): void {
        if (this.elementRef.nativeElement.scrollHeight) {
            this.resize();
        }
    }

    resize() {
        this.renderer.setStyle(this.elementRef.nativeElement, 'height', '0');
        this.renderer.setStyle(
            this.elementRef.nativeElement,
            'height',
            this.elementRef.nativeElement.scrollHeight + 2 + 'px',
        );
    }
}
