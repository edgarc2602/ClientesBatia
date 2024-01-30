import { Component, OnInit, Inject } from '@angular/core';
import * as Highcharts from 'highcharts';
import { fadeInOut } from 'src/app/fade-in-out';
import { HttpClient } from '@angular/common/http';
import { StoreUser } from 'src/app/stores/StoreUser';




@Component({
    selector: 'dashboard-comp',
    templateUrl: './dashboard.component.html',
    animations: [fadeInOut],
})
export class DashboardComponent implements OnInit {
    //mesesc: Catalogo[];
    


    constructor(@Inject('BASE_URL') private url: string, private http: HttpClient, public user: StoreUser) {
        //http.get<Catalogo[]>(`${url}api/catalogo/obtenermeses`).subscribe(response => {
        //    this.mesesc = response;
        //})
    }
    ngOnInit(): void {
        const fechaActual = new Date();
    }
}