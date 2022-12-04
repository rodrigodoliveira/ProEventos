import { Evento } from './Evento';

export interface Lote {

  id: number;
  nome: string;
  preco: number;
  dataInicio?: Date;
  dataFim?: Date;
  qauntidade: number;
  eventoId: number;
  evento: Evento;
}
