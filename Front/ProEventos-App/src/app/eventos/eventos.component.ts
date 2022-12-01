import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})
export class EventosComponent implements OnInit {

  eventosOriginal: any = [];
  eventosFiltrados: any = [];
  imagemLargura = 150;
  imagemMargem = 2;
  exibirImagem = false;
  private filtroLista = '';

  public get FiltroLista(): string {
    return this.filtroLista;
  }

  public set FiltroLista(value: string) {
    this.filtroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventosOriginal;
  }

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getEventos();
  }

  public getEventos(): void {

    this.http.get('https://localhost:5001/api/eventos').subscribe(
      response => {
        this.eventosOriginal = response;
        this.eventosFiltrados = response;
      },
      error => console.error(error)
    );

  }

  public filtrarEventos(filtrarPor: string): any{

    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventosOriginal.filter(
      (ev: any) =>
      (ev.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1 || ev.local.toLocaleLowerCase().indexOf(filtrarPor) !== -1));
  }

}
