import { Component } from '@angular/core';
import { StoreUser } from 'src/app/stores/StoreUser';

@Component({
    selector: 'exclusivo',
    templateUrl: './exclusivo.component.html'
})
export class ExclusivoComponent {
    constructor(public user: StoreUser) {
        let uST = eval('(' + localStorage.getItem('singaUser') + ')');
        if (uST != undefined) {
            // this.user = uST as StoreUser;
            this.user.idPersonal = uST.idPersonal;
            this.user.identificador = uST.identificador;
            this.user.nombre = uST.nombre;
            this.user.idInterno = uST.idInterno;
            this.user.idEmpleado = uST.idEmpleado;
            this.user.estatus = uST.estatus;
            this.user.idProveedor = uST.idProveedor;
        }
    }

    goBack() {
        window.history.back();
        this.quitarFocoDeElementos();
    }

    quitarFocoDeElementos(): void {
        const elementos = document.querySelectorAll('button, input[type="text"]');

        elementos.forEach((elemento: HTMLElement) => {
            elemento.blur();
        });
    }
}