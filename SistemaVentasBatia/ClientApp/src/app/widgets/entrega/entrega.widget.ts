import { Component, OnChanges, Output, EventEmitter, SimpleChanges, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import Swal from 'sweetalert2';
import { StoreUser } from 'src/app/stores/StoreUser';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ParamDashboard } from '../../models/paramdashboard';
import { Catalogo } from 'src/app/models/catalogo';
import { ListadoMateriales } from '../../models/listadomateriales';
declare var bootstrap: any;

@Component({
    selector: 'entrega-widget',
    templateUrl: './entrega.widget.html'
})
export class EntregaWidget {
    @Output('supEvent') sendEvent = new EventEmitter<boolean>();
    model: ListadoMateriales = {
        listas: [], pagina: 1, numPaginas: 0, rows: 0
    }
    param: ParamDashboard = {
        mes: 0, anio: 0, idCliente: 0, idInmueble: 0
    }
    mesesc: Catalogo[];
    isLoading: boolean = false;
    idStatus: number = 0;
    formato: string = '';

    constructor(@Inject('BASE_URL') private url: string, private http: HttpClient, public user: StoreUser, private fb: FormBuilder) {
        http.get<Catalogo[]>(`${url}api/catalogo/obtenermeses`).subscribe(response => {
            this.mesesc = response;
        })
    }

    open(anio: number, mes: number, idSucursal: number) {
        this.idStatus = 0;
        this.limpiarParam();
        this.param.anio = anio;
        this.param.mes = mes;
        this.param.idCliente = this.user.idInterno;
        this.param.idInmueble = idSucursal;
        this.obtenerEntregas(1);
        let docModal = document.getElementById('modalEntrega');
        let myModal = bootstrap.Modal.getOrCreateInstance(docModal);
        myModal.show();
    }

    limpiarParam() {
        this.param = {
            mes: 0, anio: 0, idCliente: 0, idInmueble: 0
        }
    }

    obtenerEntregas(paginaini: number) {
        //if (paginaini == 1) {
            this.isLoading = true;
            this.model.listas = [];
            this.model.pagina = 1;
            this.http.post<ListadoMateriales>(`${this.url}api/entrega/ObtenerListados/${this.idStatus}/${this.model.pagina}`, this.param).subscribe(response => {
                setTimeout(() => {
                    this.model = response;
                    this.isLoading = false;
                }, 300);
            }, err => {
                setTimeout(() => {
                    this.isLoading = false;
                }, 300);
            });
        //}
        //else {
            //this.model.listas = [];
            //this.http.post<ListadoMateriales>(`${this.url}api/entrega/ObtenerListados/${this.idStatus}/${this.model.pagina}`, this.param).subscribe(response => {
                
            //    this.model = response;
            //    this.isLoading = false;
            //}, err => {
            //    this.isLoading = false;
            //});
        //}
    }

    acepta() {
        this.sendEvent.emit(true);
        this.close();
    }

    cancela() {
        this.close();
    }

    close() {
        let docModal = document.getElementById('modalEntrega');
        let myModal = bootstrap.Modal.getOrCreateInstance(docModal);
        myModal.hide();
    }

    quitarFocoDeElementos(): void {
        const elementos = document.querySelectorAll('button, input[type="text"]');
        elementos.forEach((elemento: HTMLElement) => {
            elemento.blur();
        });
    }

    muevePagina(event) {
        this.model.pagina = event;
        this.obtenerEntregas(0);
    }

    getImage(archivo: string, carpeta: string) {
        this.isLoading = true;
        this.http.get(`${this.url}api/entrega/GetImage/${archivo}/${carpeta}`, { responseType: 'blob' })
            .subscribe((data: Blob) => {
                const extension = this.obtenerExtension(archivo);
                switch (extension) {
                    case 'pdf':
                        this.formato = 'application/pdf'
                        break;
                    case 'jpeg':
                        this.formato = 'image/jpeg'
                        break;
                    case 'jpg':
                        this.formato = 'image/jpg'
                        break;
                    case 'png':
                        this.formato = 'image/png'
                        break;
                    case 'PDF':
                        this.formato = 'application/pdf'
                        break;
                    case 'JPEG':
                        this.formato = 'image/jpeg'
                        break;
                    case 'JPG':
                        this.formato = 'image/jpg'
                        break;
                    case 'PNG':
                        this.formato = 'image/png'
                        break;
                    default:
                        break;
                }
                const file = new Blob([data], { type: this.formato });
                const fileURL = URL.createObjectURL(file);
                const width = 800;
                const height = 550;
                const left = window.innerWidth / 2 - width / 2;
                const top = window.innerHeight / 2 - height / 2;
                const newWindow = window.open(fileURL, '_blank', `width=${width}, height=${height}, top=${top}, left=${left}`);
                if (newWindow) {
                    newWindow.focus();
                } else {
                    alert('La ventana emergente ha sido bloqueada. Por favor, permite ventanas emergentes para este sitio.');
                }
                this.isLoading = false;
            }, error => {
                console.error('Error al obtener el documento', error);
                this.isLoading = false;
            });
        this.quitarFocoDeElementos();
        this.isLoading = false;
    }

    obtenerExtension(archivo: string): string {
        const partes = archivo.split('.');
        const extension = partes[partes.length - 1];
        return extension;
    }
}