import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { Lote } from '../models/Lote';

@Injectable()
export class LoteService {

  private baseUrl: string = 'https://localhost:5001/api/lotes'

  constructor(private http: HttpClient) { }

  public get(eventoId: number): Observable<Lote[]> {

    return this.http
      .get<Lote[]>(this.baseUrl)
      .pipe(take(1));
  }

  public put(eventoId: number, lotes: Lote[]): Observable<Lote[]> {
    return this.http
      .put<Lote[]>(`${this.baseUrl}/${eventoId}`, lotes)
      .pipe(take(1));
  }

  public delete(eventoId: number, loteId: number): Observable<any>{
    return this.http
      .delete(`${this.baseUrl}/${eventoId}/${loteId}`)
      .pipe(take(1));
  }

}
