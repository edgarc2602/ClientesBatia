import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { StoreUser } from 'src/app/stores/StoreUser';

@Component({
    selector: 'ex-menu',
    templateUrl: './menu.component.html'
})
export class ExMenuComponent {
    isExpanded = false;
    visibilidadLatMenu

    constructor(public user: StoreUser, private rtr: Router) {
        if (user.idPersonal == undefined) {
            rtr.navigate(['/']);
        }
    }

    logout() {
        this.quitarFocoDeElementos();
        localStorage.removeItem('singaUser');
        this.user = null;
        this.rtr.navigate(['']);
    }

  collapse() {
    this.isExpanded = false;
  }

    toggle() {
            this.isExpanded = !this.isExpanded;
    }
    button() {
        this.visibilidadLatMenu = 0;
        this.quitarFocoDeElementos();
    }
    quitarFocoDeElementos(): void {
        const elementos = document.querySelectorAll('button, input[type="text"]');
        elementos.forEach((elemento: HTMLElement) => {
            elemento.blur();
        });
    }
}