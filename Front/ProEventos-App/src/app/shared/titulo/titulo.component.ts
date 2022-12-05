import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-titulo',
  templateUrl: './titulo.component.html',
  styleUrls: ['./titulo.component.scss']
})
export class TituloComponent implements OnInit {

  @Input() link: string = '';
  @Input() tituloPrincipal: string = 'Dashboard';
  @Input() subtitulo: string = 'Acompanhamento';
  @Input() iconClass: string = 'fa fa-globe'
  @Input() exibeBotao: boolean = false;

  constructor() { }

  ngOnInit() {
  }

}
