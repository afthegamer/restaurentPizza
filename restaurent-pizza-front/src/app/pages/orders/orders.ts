import { Component, inject, OnInit, signal } from '@angular/core';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { OrderService } from '../../services/order.service';
import { Order } from '../../models/order.model';
import { Card } from 'primeng/card';
import { Tag } from 'primeng/tag';
import { TableModule } from 'primeng/table';

@Component({
  selector: 'app-orders',
  imports: [CurrencyPipe, DatePipe, Card, Tag, TableModule],
  template: `
    <h1>Mes Commandes</h1>

    @if (loading()) {
      <p>Chargement...</p>
    } @else if (orders().length === 0) {
      <p-card>
        <p>Aucune commande pour le moment.</p>
      </p-card>
    } @else {
      @for (order of orders(); track order.id) {
        <p-card styleClass="order-card">
          <div class="order-header">
            <div>
              <strong>Commande du {{ order.createdOn | date:'dd/MM/yyyy HH:mm' }}</strong>
            </div>
            <div class="order-meta">
              <p-tag [value]="order.status" [severity]="getStatusSeverity(order.status)" />
              <span class="order-total">{{ order.total | currency:'EUR':'symbol':'1.2-2' }}</span>
            </div>
          </div>
          <p-table [value]="order.items" [tableStyle]="{ 'min-width': '30rem' }">
            <ng-template #header>
              <tr>
                <th>Pizza</th>
                <th>Quantite</th>
                <th>Prix unitaire</th>
                <th>Total ligne</th>
              </tr>
            </ng-template>
            <ng-template #body let-item>
              <tr>
                <td>{{ item.pizzaName }}</td>
                <td>{{ item.quantity }}</td>
                <td>{{ item.unitPrice | currency:'EUR':'symbol':'1.2-2' }}</td>
                <td>{{ item.lineTotal | currency:'EUR':'symbol':'1.2-2' }}</td>
              </tr>
            </ng-template>
          </p-table>
        </p-card>
      }
    }
  `,
  styles: [`
    h1 { color: #1e293b; margin-bottom: 1.5rem; }
    .order-card { margin-bottom: 1rem; }
    .order-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 1rem; }
    .order-meta { display: flex; align-items: center; gap: 1rem; }
    .order-total { font-size: 1.25rem; font-weight: 700; color: #f97316; }
  `]
})
export class Orders implements OnInit {
  private orderService = inject(OrderService);

  orders = signal<Order[]>([]);
  loading = signal(true);

  ngOnInit() {
    this.orderService.getMyOrders().subscribe({
      next: data => { this.orders.set(data); this.loading.set(false); },
      error: () => this.loading.set(false)
    });
  }

  getStatusSeverity(status: string): 'success' | 'info' | 'warn' | 'danger' | 'secondary' {
    switch (status) {
      case 'Delivered': return 'success';
      case 'Preparing': return 'info';
      case 'Ready': return 'warn';
      case 'Cancelled': return 'danger';
      default: return 'secondary';
    }
  }
}
