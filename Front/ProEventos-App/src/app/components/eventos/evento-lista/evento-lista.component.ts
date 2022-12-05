import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from 'src/app/models/Evento';
import { EventoService } from 'src/app/services/evento.service';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent implements OnInit {

  public tituloTela = "Eventos";

  public modalRef: BsModalRef = new BsModalRef();

  public eventosOriginal: Evento[] = [];
  public eventosFiltrados: Evento[] = [];
  public imagemLargura = 150;
  public imagemMargem = 2;
  public exibirImagem = false;
  private filtroListado = '';

  public get FiltroLista(): string {
    return this.filtroListado;
  }

  public set FiltroLista(value: string) {
    this.filtroListado = value;
    this.eventosFiltrados = this.filtroListado ? this.filtrarEventos(this.filtroListado) : this.eventosOriginal;
  }

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router) { }

  public ngOnInit(): void {
    this.getEventos();

    this.spinner.show();
  }

  public getEventos(): void {

    this.eventoService.getEventos().subscribe({
        next: (eventos: Evento[]) => {
          this.eventosOriginal = eventos;
          this.eventosFiltrados = this.eventosOriginal;
        },
        error: (err: any) => {
          this.spinner.hide()
          this.showError('Erro ao carregar eventos', 'Erro');
          console.error(err.message);
        },
        complete:() => {
          this.spinner.hide();
        }
      });
  }

  public filtrarEventos(filtrarPor: string): Evento[] {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventosOriginal.filter(
      (ev: Evento) =>
      (ev.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1 || ev.local.toLocaleLowerCase().indexOf(filtrarPor) !== -1));
  }

  public showSuccess(mensagem: string, titulo: string): void {
    this.toastr.success(mensagem, titulo);
  }
  public showError(mensagem: string, titulo: string): void {
    this.toastr.error(mensagem, titulo);
  }

  public openModal(template: TemplateRef<any>): void {
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  public confirm(): void {
    this.modalRef.hide();
    this.showSuccess("Evento excluído com sucesso", "Delete")
  }

  public decline(): void {
    console.log('Não confirme');
    this.modalRef.hide();
  }

  public detalheEvento(id: number): void {
    this.router.navigate([`eventos/detalhe/${id}`]);
  }

}
