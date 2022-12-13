import { Component, OnInit, TemplateRef } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { concatMapTo } from 'rxjs/operators';

import { Evento } from 'src/app/models/Evento';
import { Lote } from 'src/app/models/Lote';

import { EventoService } from 'src/app/services/evento.service';
import { LoteService } from 'src/app/services/lote.service';
import { environment } from 'src/environments/environment';


@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {

  public modalRef: BsModalRef = new BsModalRef();
  eventoId = 0;
  loteAtual = { id: 0, nome: '', index: -1 };
  locale: string = 'pt-br';
  evento = {} as Evento;
  form: FormGroup = new FormGroup({});
  modoSalvar = 'post';
  imagemURL = 'assets/images/upload.png'
  file: File

  get f(): any {

    return this.form.controls;
  }

  get lotes(): FormArray {
    return this.form.get('lotes') as FormArray;
  }

  get bsConfig(): any {

    return {
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY hh:mm a',
      containerClass: 'theme-default',
      showWeekNumbers: false,
    }
  }

  get bsConfigLote(): any {

    return {
      adaptivePosition: true,
      containerClass: 'theme-default',
    }
  }

  get modoEditar(): boolean {
    return this.modoSalvar === 'put';
  }

  constructor(private fb: FormBuilder,
    private localeService: BsLocaleService,
    private activatedRouter: ActivatedRoute,
    private eventoService: EventoService,
    private loteService: LoteService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService,
    private router: Router,
    private modalService: BsModalService) {

    this.validation();
  }

  public ngOnInit(): void {

    this.localeService.use(this.locale);
    this.carregarEvento();
  }

  public openModal(event: any, template: TemplateRef<any>, eventoId: number): void {
    event.stopPropagation();
    this.eventoId = eventoId;
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  public retornaNomeLote(nome: string): string {
    return nome === null || nome === ''
      ? 'Nome do lote'
      : nome;
  }

  public validation(): void {

    this.form = this.fb.group({
      tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.max(120000), Validators.min(1)]],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      imageUrl: [''],
      lotes: this.fb.array([]) //sub form dentro do form principal
    });
  }

  public adicionarLote(): void {
    this.lotes.push(this.criarLote({ id: 0 } as Lote)); //cria um novo form de lote e adiciona ao form principal
  }

  public mudarValorData(data: Date, i: number, campo: string): void {
    this.lotes.value[i][campo] = data
  }


  public criarLote(lote: Lote): FormGroup {
    return this.fb.group({
      id: [lote.id, Validators.required],
      nome: [lote.nome, Validators.required],
      preco: [lote.preco, Validators.required],
      quantidade: [lote.quantidade, Validators.required],
      dataInicio: [lote.dataInicio],
      dataFim: [lote.dataFim],
    })
  }

  public resetForm(): void {

    this.form.reset();
  }

  public cssValidator(campoForm: FormControl | AbstractControl): any {

    return { 'is-invalid': campoForm.errors && campoForm.touched }
  }

  public carregarEvento(): void {

    this.spinner.show();
    this.eventoId = +this.activatedRouter.snapshot.paramMap.get('id');


    if (this.eventoId !== null && this.eventoId > 0) {

      this.modoSalvar = 'put'; //set o salvamento como edit.

      this.eventoService.getEventoById(this.eventoId).subscribe(
        (ev: Evento) => {
          this.evento = { ...ev };
          this.form.patchValue(this.evento); //faz o bind do form com os campos do evento
          if (this.evento.imageUrl != "") {
            this.imagemURL = environment.apiURL + '/resources/images/' + this.evento.imageUrl
          }

          this.carregarLotes();

        },
        (erro: any) => {
          this.toastr.error('Houve um erro ao tentar carregar os eventos');
          console.error(erro);
        }
      ).add(() => this.spinner.hide());
    } else {

      this.spinner.hide();
    }

  }

  public salvar(): void {

    this.spinner.show();

    if (this.form.valid) {

      if (this.modoSalvar === 'post') {
        this.salvarEvento();
      } else {
        this.editarEvento();
      }
    }
  }

  public salvarEvento(): void {

    this.evento = { ...this.form.value }; //converte o formulario em um evento com spread operator ...
    this.eventoService.post(this.evento).subscribe(
      (evento: Evento) => {
        this.router.navigate([`eventos/detalhe/${evento.id}`]);
        this.toastr.success('Evento salvo com sucesso!');
      },
      (erro: any) => {
        console.error(erro);
        this.toastr.error('Houve um erro ao tentar salvar o evento');
      }
    ).add(() => this.spinner.hide());
  }

  public editarEvento(): void {

    this.evento = { id: this.evento.id, ...this.form.value } //converte o formulario em um evento defindo o id e com spread operator ...
    this.eventoService.put(this.evento).subscribe(
      (evento: Evento) => {
        this.router.navigate([`eventos/detalhe/${evento.id}`]);
        this.toastr.success('Evento salvo com sucesso!');
      },
      (erro: any) => {
        console.error(erro);
        this.toastr.error('Houve um erro ao tentar salvar o evento');
      }
    ).add(() => this.spinner.hide());
  }

  public salvarLotes(): void {


    if (this.form.controls.lotes.valid) {
      this.spinner.show();
      this.loteService.put(this.evento.id, this.form.controls.lotes.value)
        .subscribe(
          (lotes: Lote[]) => {
            this.toastr.success('Lotes salvos com sucesso', 'Lotes');

          },
          (error: any) => {
            console.log(error);
            this.toastr.error('Houve um erro ao tentar salvar lotes');
          }
        )
        .add(() => this.spinner.hide());
    }
  }

  private carregarLotes(): void {
    if (this.evento.lotes) {
      this.evento.lotes.forEach(lote => {
        this.lotes.push(this.criarLote(lote));
      });
    }

  }

  public removerLote(event: any, template: TemplateRef<any>, i: number): void {
    event.stopPropagation();
    const loteRemover = this.lotes.at(i).value as Lote
    this.loteAtual = { id: loteRemover.id, nome: loteRemover.nome, index: i };
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  public confirm() {
    this.spinner.show();
    this.modalRef.hide();

    this.loteService.delete(this.eventoId, this.loteAtual.id).subscribe(
      (ret: any) => {
        if (ret.message === 'Lote deletado') {
          this.lotes.removeAt(this.loteAtual.index);
          this.router.navigate([`eventos/detalhe/${this.eventoId}`]);
          this.toastr.success('Lote deletado com sucesso', 'Delete');
        }
      },
      (err: any) => {
        this.toastr.error('Houve um erro ao tentar deletar o lote', 'Delete');
        console.error(err);
      }
    ).add(() => this.spinner.hide());
  }

  public decline() {
    this.modalRef.hide();
  }

  public onFileChange(ev: any): void {
    const reader = new FileReader();
    reader.onload = (event: any) => this.imagemURL = event.target.result;

    this.file = ev.target.files;
    reader.readAsDataURL(this.file[0]);

    this.uploadImage()

  }

  public uploadImage(): void {
    this.spinner.show();
    this.eventoService.postUpload(this.eventoId, this.file).subscribe(
      (evento: Evento) => {
        this.carregarEvento();
        this.toastr.success("Imagem atualizada com sucesso");
      },
      (error: any) => {
        this.toastr.error("Houve um erro ao tentar fazer o upload da imagem");
        console.error(error);
      }
    ).add(() => this.spinner.hide());
  }

}
