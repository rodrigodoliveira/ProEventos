import { Evento } from './Evento';

export interface Lote {

  id: number;
  nome: string;
  preco: number;
  dataInicio: string;
  dataFim: string;
  quantidade: number;
  eventoId: number;
  evento: Evento;
}
