import { Component, inject, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { OrderService } from '../../../services/order.service';
import { MessageService } from 'primeng/api';
import { Order } from '../../../models/order.model';
import { TableModule } from 'primeng/table';
import { Tag } from 'primeng/tag';
import { Select } from 'primeng/select';
import { Button } from 'primeng/button';

@Component({
  selector: 'app-admin-orders',
  imports: [FormsModule, CurrencyPipe, DatePipe, TableModule, Tag, Select, Button],
  template: `
    <h1>Toutes les Commandes</h1>

    <p-table [value]="orders()" [tableStyle]="{ 'min-width': '50rem' }" [expandedRowKeys]="expandedRows">
      <ng-template #header>
        <tr>
          <th style="width: 3rem"></th>
          <th>Date</th>
          <th>Statut</th>
          <th>Total</th>
          <th>Changer statut</th>
        </tr>
      </ng-template>
      <ng-template #body let-order let-expanded="expanded">
        <tr>
          <td>
            <p-button
              type="button"
              [icon]="expanded ? 'pi pi-chevron-down' : 'pi pi-chevron-right'"
              [rounded]="true"
              [text]="true"
              (onClick)="toggleRow(order)" />
          </td>
          <td>{{ order.createdOn | date:'dd/MM/yyyy HH:mm' }}</td>
          <td><p-tag [value]="order.status" [severity]="getStatusSeverity(order.status)" /></td>
          <td>{{ order.total | currency:'EUR':'symbol':'1.2-2' }}</td>
          <td>
            <div class="status-change">
              <p-select
                [(ngModel)]="order._newStatus"
                [options]="statuses"
                optionLabel="label"
                optionValue="value"
                placeholder="Nouveau statut"
                styleClass="status-select" />
              <p-button
                icon="pi pi-check"
                [rounded]="true"
                [disabled]="!order._newStatus"
                (onClick)="changeStatus(order)" />
            </div>
          </td>
        </tr>
      </ng-template>
      <ng-template #rowexpansion let-order>
        <tr>
          <td colspan="5">
            <div class="expanded-content">
              <p-table [value]="order.items">
                <ng-template #header>
                  <tr>
                    <th>Pizza</th>
                    <th>Quantite</th>
                    <th>Prix unitaire</th>
                    <th>Total</th>
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
            </div>
          </td>
        </tr>
      </ng-template>
    </p-table>
  `,
  styles: [`
    h1 { color: #1e293b; margin-bottom: 1.5rem; }
    .status-change { display: flex; gap: 0.5rem; align-items: center; }
    .status-select { width: 160px; }
    .expanded-content { padding: 1rem; }
  `]
})
export class AdminOrders implements OnInit {
  private orderService = inject(OrderService);
  private messageService = inject(MessageService);

  orders = signal<(Order & { _newStatus?: string })[]>([]);
  expandedRows: { [key: string]: boolean } = {};

  statuses = [
    { label: 'En attente', value: 'Pending' },
    { label: 'Preparation', value: 'Preparing' },
    { label: 'Prete', value: 'Ready' },
    { label: 'Livree', value: 'Delivered' },
    { label: 'Annulee', value: 'Cancelled' }
  ];

  ngOnInit() { this.load(); }

  load() {
    this.orderService.getAll().subscribe(data => this.orders.set(data));
  }

  toggleRow(order: Order) {
    if (this.expandedRows[order.id]) {
      delete this.expandedRows[order.id];
    } else {
      this.expandedRows[order.id] = true;
    }
  }

  changeStatus(order: Order & { _newStatus?: string }) {
    if (!order._newStatus) return;
    this.orderService.updateStatus(order.id, order._newStatus).subscribe({
      next: () => {
        this.load();
        this.messageService.add({ severity: 'success', summary: 'OK', detail: 'Statut mis a jour' });
      },
      error: err => this.messageService.add({ severity: 'error', summary: 'Erreur', detail: err.error?.detail || 'Erreur' })
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
