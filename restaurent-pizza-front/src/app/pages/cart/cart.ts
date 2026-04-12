import { Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { CurrencyPipe } from '@angular/common';
import { CartService } from '../../core/cart.service';
import { OrderService } from '../../services/order.service';
import { MessageService } from 'primeng/api';
import { Button } from 'primeng/button';
import { Card } from 'primeng/card';
import { InputNumber } from 'primeng/inputnumber';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-cart',
  imports: [CurrencyPipe, FormsModule, Button, Card, InputNumber],
  template: `
    <h1>Mon Panier</h1>

    @if (cart.cartItems().length === 0) {
      <p-card>
        <p>Votre panier est vide.</p>
        <p-button label="Voir le menu" icon="pi pi-arrow-left" (onClick)="goToMenu()" />
      </p-card>
    } @else {
      <div class="cart-items">
        @for (item of cart.cartItems(); track item.pizza.id) {
          <p-card styleClass="cart-item">
            <div class="cart-row">
              <div class="cart-info">
                <h3>{{ item.pizza.name }}</h3>
                <p class="price">{{ item.pizza.price | currency:'EUR':'symbol':'1.2-2' }} / unite</p>
              </div>
              <div class="cart-quantity">
                <p-inputNumber [(ngModel)]="item.quantity" [min]="1" [max]="20" [showButtons]="true"
                  (ngModelChange)="cart.updateQuantity(item.pizza.id, $event)" />
              </div>
              <div class="cart-total">
                {{ item.pizza.price * item.quantity | currency:'EUR':'symbol':'1.2-2' }}
              </div>
              <p-button icon="pi pi-trash" severity="danger" [text]="true" (onClick)="cart.removeFromCart(item.pizza.id)" />
            </div>
          </p-card>
        }
      </div>

      <p-card styleClass="cart-summary">
        <div class="summary-row">
          <span class="summary-label">Total :</span>
          <span class="summary-total">{{ cart.total() | currency:'EUR':'symbol':'1.2-2' }}</span>
        </div>
        <div class="summary-actions">
          <p-button label="Continuer les achats" severity="secondary" [outlined]="true" (onClick)="goToMenu()" />
          <p-button label="Commander" icon="pi pi-check" [loading]="ordering()" (onClick)="placeOrder()" />
        </div>
      </p-card>
    }
  `,
  styles: [`
    h1 { color: #1e293b; margin-bottom: 1.5rem; }
    .cart-items { display: flex; flex-direction: column; gap: 0.75rem; margin-bottom: 1.5rem; }
    .cart-row { display: flex; align-items: center; gap: 1.5rem; }
    .cart-info { flex: 1; }
    .cart-info h3 { margin: 0; }
    .price { color: #64748b; margin: 0.25rem 0 0; }
    .cart-total { font-size: 1.2rem; font-weight: 700; color: #f97316; min-width: 80px; text-align: right; }
    .summary-row { display: flex; justify-content: space-between; align-items: center; margin-bottom: 1.5rem; }
    .summary-label { font-size: 1.25rem; font-weight: 600; }
    .summary-total { font-size: 1.75rem; font-weight: 700; color: #f97316; }
    .summary-actions { display: flex; justify-content: flex-end; gap: 1rem; }
  `]
})
export class Cart {
  readonly cart = inject(CartService);
  private orderService = inject(OrderService);
  private messageService = inject(MessageService);
  private router = inject(Router);

  ordering = signal(false);

  goToMenu() {
    this.router.navigate(['/menu']);
  }

  placeOrder() {
    this.ordering.set(true);
    const dto = {
      items: this.cart.cartItems().map(i => ({
        pizzaId: i.pizza.id,
        quantity: i.quantity
      }))
    };

    this.orderService.create(dto).subscribe({
      next: () => {
        this.cart.clear();
        this.ordering.set(false);
        this.messageService.add({ severity: 'success', summary: 'Commande passee !', detail: 'Votre commande a ete envoyee' });
        this.router.navigate(['/orders']);
      },
      error: err => {
        this.ordering.set(false);
        this.messageService.add({ severity: 'error', summary: 'Erreur', detail: err.error?.detail || 'Commande impossible' });
      }
    });
  }
}
