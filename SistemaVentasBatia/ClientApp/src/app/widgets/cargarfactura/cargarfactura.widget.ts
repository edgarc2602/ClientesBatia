import { Component, OnChanges, Output, EventEmitter, Inject, ViewChild, ElementRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ListadoAcuseEntrega } from '../../models/listadoacuseentrega';
declare var bootstrap: any;

@Component({
    selector: 'cargarfactura-widget',
    templateUrl: './cargarfactura.widget.html'
})
export class CargarFacturaWidget {
    @ViewChild('fileInput', { static: false }) fileInput!: ElementRef;
    @Output('ansEvent') sendEvent = new EventEmitter<boolean>();

    model: ListadoAcuseEntrega = {
        acuses: [], carpeta: '', idListado: 0
    }
    sucursal: string;
    tipo: string;
    idListado: number;
    selectedFileName: string | null = null;
    selectedFile: File | null = null;
    public imageUrl: string = '';
    formato: string = '';
    prefijo: string = '';

    constructor(@Inject('BASE_URL') private url: string, private http: HttpClient) { }
    nuevo() {
        this.model = {
            acuses: [], carpeta: '', idListado: 0
        }
        this.selectedFileName = null;
        this.selectedFile = null;
        /*this.resetFileInput();*/
    }

    open(idOrden: number) {
        this.nuevo();
        //this.idListado = idListado;
        //this.sucursal = sucursal;
        //this.tipo = tipo;
        //this.prefijo = prefijo;
        /*this.obtenerAcusesListado(idListado);*/
        let docModal = document.getElementById('modalCargarFactura');
        let myModal = bootstrap.Modal.getOrCreateInstance(docModal);
        myModal.show();
    }
    //obtenerAcusesListado(idListado: number) {
    //    this.http.get<ListadoAcuseEntrega>(`${this.url}api/entrega/obteneracuseslistado/${idListado}`).subscribe(response => {
    //        this.model = response;
    //    })
    //}

    acepta() {
        this.sendEvent.emit(true);
        this.close();
    }

    cancela() {
        this.close();
    }

    close() {
        let docModal = document.getElementById('modalCargarFactura');
        let myModal = bootstrap.Modal.getOrCreateInstance(docModal);
        myModal.hide();
    }

    concluirEntrega() {

    }
    guardarArchivo(): void {
        if (this.selectedFile) {
            const formData = new FormData();
            formData.append('file', this.selectedFile);

            this.http.post<boolean>(`${this.url}api/entrega/guardaracuse/${this.idListado}/${this.selectedFileName}`, formData).subscribe((response) => {
                console.log('Archivo guardado con �xito:', response);
                this.selectedFileName = null;
                this.selectedFile = null;
                //this.obtenerAcusesListado(this.idListado);
                this.resetFileInput();
            }, (error) => {
                console.error('Error al guardar el archivo:', error);
            });
        } else {
            console.error('No se ha seleccionado ning�n archivo.');
        }
    }
    eliminaAcuse(archivo: string, carpeta: string) {
        this.http.delete<boolean>(`${this.url}api/entrega/eliminaacuse/${archivo}/${carpeta}/${this.idListado}`).subscribe(response => {
            console.log('Archivo eliminado con �xito:', response);
            this.selectedFileName = null;
            this.selectedFile = null;
            //this.obtenerAcusesListado(this.idListado);
            this.resetFileInput();
        }, (error) => {
            console.error('Error al eliminar el archivo:', error);
        });

    }

    limpiarDocumento() {
        this.selectedFileName = null;
        this.resetFileInput();
    }

    onFileSelected(event: any): void {
        this.selectedFile = event.target.files[0];
        this.selectedFileName = this.selectedFile.name;
    }

    


    openDocument(archivo: string, carpeta: string) {
        this.getImage(archivo, carpeta);
    }

    getImage(archivo: string, carpeta: string) {
        this.http.get(`${this.url}api/entrega/getimage/${archivo}/${carpeta}`, { responseType: 'blob' })
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
            }, error => {
                console.error('Error al obtener el documento', error);
            });
    }
    obtenerExtension(archivo: string): string {
        const partes = archivo.split('.');
        const extension = partes[partes.length - 1];
        return extension;
    }

    resetFileInput(): void {
        this.fileInput.nativeElement.value = '';
    }
}
