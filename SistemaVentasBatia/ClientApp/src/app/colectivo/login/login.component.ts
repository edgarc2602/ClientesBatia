import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Acceso } from 'src/app/models/acceso';
import { Usuario } from 'src/app/models/usuario';
import { Subject } from 'rxjs';

import { fadeInOut } from 'src/app/fade-in-out';

@Component({
    selector: 'login-comp',
    templateUrl: './login.component.html',
    animations: [fadeInOut],
})
export class LoginComponent {
    model: Acceso = { usuario: '', contrasena: '' };
    usu: Usuario = {} as Usuario;
    lerr: any = {};
    evenSub: Subject<void> = new Subject<void>();
    isErr: boolean = false;
    errMessage: string = '';
    isLoading: boolean = false;

    constructor(@Inject('BASE_URL') private url: string, private http: HttpClient, private rtr: Router) {}

    onLogin() {
        this.isLoading = true;
        this.lerr = {};
        if (this.valida()) {
            this.http.post<Usuario>(`${this.url}api/usuario/login`, this.model).subscribe(response => {
                setTimeout(() => {
                    this.isLoading = false;
                    console.log(response);
                    this.usu = response;
                    localStorage.setItem('singaUser', JSON.stringify(this.usu));
                    this.rtr.navigate(['/exclusivo']);
                }, 500);


            }, err => {
                this.isLoading = false;
                console.log(err);
                if (err.error) {
                    if (err.error.errors) {
                        this.lerr = err.error.errors;
                    } else if (err.error.message) {
                        this.isErr = true;
                        this.errMessage = err.error.message;
                        this.evenSub.next();
                    }
                }
            });
        }
    }

    review(event: any) {
        if (event.keyCode == 13) {
            this.onLogin();
        }
    }

    ferr(nm: string) {
        let fld = this.lerr[nm];
        if (fld)
            return true;
        else
            return false;
    }

    terr(nm: string) {
        let fld = this.lerr[nm];
        let msg: string = fld.map((x: string) => "-" + x);
        return msg;
    }

    valida() {
        return true;
    }
}