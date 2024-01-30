import { Component, Inject, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { fadeInOut } from 'src/app/fade-in-out';
import { StoreUser } from 'src/app/stores/StoreUser';

@Component({
    selector: 'entrega-comp',
    templateUrl: './entrega.component.html',
    animations: [fadeInOut],
})
export class EntregaComponent {
   /* meses: Catalogo[];*/
    //@ViewChild(DetalleMaterialesListadoWidget, { static: false }) matLis: DetalleMaterialesListadoWidget;

    constructor(@Inject('BASE_URL') private url: string, private http: HttpClient, public user: StoreUser) {
        //http.get<Catalogo[]>(`${url}api/catalogo/obtenermeses`).subscribe(response => {
        //    this.meses = response;
        //})
    }
    ngOnInit() {
        const fechaActual = new Date();
    }
}
