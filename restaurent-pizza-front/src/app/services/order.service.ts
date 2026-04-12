import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Order, CreateOrderDto } from '../models/order.model';

@Injectable({ providedIn: 'root' })
export class OrderService {
  constructor(private http: HttpClient) {}

  create(dto: CreateOrderDto) {
    return this.http.post<Order>('/order', dto);
  }

  getMyOrders() {
    return this.http.get<Order[]>('/order');
  }

  getById(id: string) {
    return this.http.get<Order>(`/order/${id}`);
  }

  getAll() {
    return this.http.get<Order[]>('/order/all');
  }

  updateStatus(id: string, status: string) {
    return this.http.put<void>(`/order/${id}/status`, { status });
  }
}
