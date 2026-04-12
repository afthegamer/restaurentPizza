import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Pizza, CreatePizzaDto, UpdatePizzaDto } from '../models/pizza.model';

@Injectable({ providedIn: 'root' })
export class PizzaService {
  constructor(private http: HttpClient) {}

  getAll() {
    return this.http.get<Pizza[]>('/pizza');
  }

  getById(id: string) {
    return this.http.get<Pizza>(`/pizza/${id}`);
  }

  create(dto: CreatePizzaDto) {
    return this.http.post<Pizza>('/pizza', dto);
  }

  update(dto: UpdatePizzaDto) {
    return this.http.put<void>(`/pizza/${dto.id}`, dto);
  }

  delete(id: string) {
    return this.http.delete<void>(`/pizza/${id}`);
  }
}
