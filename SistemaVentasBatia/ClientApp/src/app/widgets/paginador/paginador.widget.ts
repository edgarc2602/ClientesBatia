import { Component, OnChanges, Input, SimpleChanges, Output, EventEmitter } from '@angular/core';

@Component({
    selector: 'pagina-widget',
    templateUrl: './paginador.widget.html'
})
export class PaginaWidget implements OnChanges {
    @Input() pagina: number = 0;
    @Input() numPaginas: number = 0;
    @Input() rows: number = 0;
    @Output('chgEvent') changeEvent = new EventEmitter<number>();
    bloqueActual: number = 1;

    constructor() {}

    makePages(): number[] {
        const paginas = [];
        const inicioBloque = (this.bloqueActual - 1) * 5 + 1;
        const finBloque = Math.min(this.bloqueActual * 5, this.numPaginas);
        for (let i = inicioBloque; i <= finBloque; i++) {
            paginas.push(i);
        }
        return paginas;
    }

    toPrev() {
        if (this.pagina % 5 === 1 && this.bloqueActual > 1) {
            this.toPrevBlock();
        }
        this.move(this.pagina - 1);
        this.quitarFocoDeElementos();
    }

    toNext() {
        if (this.pagina === this.bloqueActual * 5 && this.bloqueActual * 5 <= this.numPaginas) {
            this.toNextBlock();
            this.move((this.bloqueActual - 1) * 5 + 1);
        } else if (this.pagina < this.numPaginas) {
            this.move(this.pagina + 1);
        }
        this.quitarFocoDeElementos();
    }

    move(p: number) {
        this.changeEvent.emit(p);
    }

    ngOnChanges(changes: SimpleChanges): void {
    }

    toPrevBlock() {
        if (this.bloqueActual > 1) {
            this.bloqueActual--;
        }
        this.quitarFocoDeElementos();
    }

    toNextBlock() {
        const ultimaPaginaBloque = this.bloqueActual * 5;
        if (ultimaPaginaBloque <= this.numPaginas) {
            this.bloqueActual++;
        }
        this.quitarFocoDeElementos();
    }

    quitarFocoDeElementos(): void {
        const elementos = document.querySelectorAll('button, input[type="text"]');
        elementos.forEach((elemento: HTMLElement) => {
            elemento.blur();
        });
    }
}