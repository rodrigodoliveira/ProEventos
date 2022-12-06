import { Lote } from './Lote';
import { Palestrante } from './Palestrante';
import { RedeSocial } from './RedeSocial';

export interface Evento {

  id: number;
  local: string;
  dataEvento: Date;
  tema: string;
  qtdPessoas: number;
  imageUrl: string;
  telefone: string;
  email: string;
  imagemUrl: string;
  lotes: Lote[];
  redesSociais: RedeSocial[];
  palestrantesEventos: Palestrante[];
}
