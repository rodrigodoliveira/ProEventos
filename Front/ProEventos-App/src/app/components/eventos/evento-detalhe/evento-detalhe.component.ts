import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from 'src/app/models/Evento';
import { EventoService } from 'src/app/services/evento.service';


@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {

  locale: string = 'pt-br';
  evento = {} as Evento;
  form: FormGroup = new FormGroup({});
  modoSalvar = 'post';
  metodoSalvar: any;

  get f(): any {

    return this.form.controls;
  }

  get bsConfig(): any {

    return {
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY h:mm a',
      containerClass: 'theme-default',
      showWeekNumbers: false
    }
  }

  constructor(private fb: FormBuilder,
    private localeService: BsLocaleService,
    private router: ActivatedRoute,
    private eventoService: EventoService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService) {

    this.validation();
  }

  public ngOnInit(): void {

    this.localeService.use(this.locale);
    this.carregarEvento();
  }

  public validation(): void {

    this.form = this.fb.group({
      tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.max(120000), Validators.min(1)]],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      imageUrl: ['', Validators.required]
    });
  }

  public resetForm(): void {

    this.form.reset();
  }

  public cssValidator(campoForm: FormControl): any {

    return { 'is-invalid': campoForm.errors && campoForm.touched }
  }

  public carregarEvento(): void {

    this.spinner.show();
    const eventoId = this.router.snapshot.paramMap.get('id');

    if (eventoId !== null) {

      this.modoSalvar = 'put'; //set o salvamento como edit.

      this.eventoService.getEventoById(+eventoId).subscribe(
        (ev: Evento) => {

          this.evento = { ...ev };
          this.form.patchValue(this.evento); //faz o bind do form com os campos do evento
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
        this.form.reset();
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
        this.form.reset();
        this.toastr.success('Evento salvo com sucesso!');
      },
      (erro: any) => {
        console.error(erro);
        this.toastr.error('Houve um erro ao tentar salvar o evento');
      }
    ).add(() => this.spinner.hide());
  }

}
