import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { ValidatorField } from 'src/app/helpers/ValidatorField';
import { UserUpdate } from 'src/app/models/identity/UserUpdate';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss']
})
export class PerfilComponent implements OnInit {

  userUpdate = {} as UserUpdate;
  form: FormGroup = new FormGroup({});

  get f(): any {
    return this.form.controls;
  }

  constructor(public fb: FormBuilder,
    public accountService: AccountService,
    private router: Router,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService) { }

  public ngOnInit() {
    this.validation();
    this.carregarUsuario();
  }

  private carregarUsuario(): void {
    this.spinner.show();

    this.accountService.getUser().subscribe(
      (userRetorno: UserUpdate) => {
        this.userUpdate = userRetorno;
        this.form.patchValue(this.userUpdate);
      },
      (error: any) => {
        console.error(error);
        this.toastr.error("Usuário não carregado", "Erro");
        this.router.navigate(['/dashboard'])
      }
    ).add(() => this.spinner.hide());
  }

  public validation(): void {

    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('password', 'confirmePassword')
    };

    this.form = this.fb.group({
      userName: [''],
      titulo: ['NaoInformado', Validators.required],
      primeiroNome: ['', Validators.required],
      ultimoNome: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', Validators.required],
      funcao: ['NaoInformado', Validators.required],
      descricao: ['', Validators.required],
      password: [null, [Validators.nullValidator, Validators.minLength(4)]],
      confirmePassword: [null, Validators.nullValidator],
    }, formOptions)
  }

  public resetForm(event: any): void {
    event.preventDefault();
    this.form.reset();
  }

  public onSubmit(event: any): void {
    event.preventDefault();
    this.atualizarUsuario();
  }

  public atualizarUsuario() {
    this.spinner.show()
    this.userUpdate = {... this.form.value};
    this.accountService.updateUser(this.userUpdate).subscribe(
      ()=>{
        this.toastr.success("Usuário atualizado", "Successo");
      },
      (erro: any)=> {
        this.toastr.error("Erro ao atualiar usário", "Erro");
        console.error(erro);
      }
    ).add(()=> this.spinner.hide());

  }

}
