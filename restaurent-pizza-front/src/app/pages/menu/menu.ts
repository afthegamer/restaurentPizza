import { Component, inject, OnInit, signal } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { PizzaService } from '../../services/pizza.service';
import { CartService } from '../../core/cart.service';
import { MessageService } from 'primeng/api';
import { Pizza } from '../../models/pizza.model';
import { Card } from 'primeng/card';
import { Button } from 'primeng/button';
import { Tag } from 'primeng/tag';

@Component({
  selector: 'app-menu',
  imports: [CurrencyPipe, Card, Button, Tag],
  template: `
    <h1>Notre Menu</h1>

    @if (loading()) {
      <p>Chargement...</p>
    } @else {
      <div class="pizza-grid">
        @for (pizza of pizzas(); track pizza.id) {
          <p-card>
            <ng-template #header>
              <div class="pizza-header">
                <span class="pizza-category">
                  <p-tag [value]="pizza.categoryName" severity="info" />
                </span>
              </div>
            </ng-template>
            <h3>{{ pizza.name }}</h3>
            <p class="pizza-desc">{{ pizza.description }}</p>
            <p class="pizza-price">{{ pizza.price | currency:'EUR':'symbol':'1.2-2' }}</p>
            <ng-template #footer>
              <p-button
                label="Ajouter au panier"
                icon="pi pi-shopping-cart"
                (onClick)="addToCart(pizza)"
                [disabled]="!pizza.isAvailable"
                styleClass="w-full" />
            </ng-template>
          </p-card>
        } @empty {
          <p>Aucune pizza disponible pour le moment.</p>
        }
      </div>
    }
  `,
  styles: [`
    h1 { color: #1e293b; margin-bottom: 1.5rem; }
    .pizza-grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
      gap: 1.5rem;
    }
    .pizza-header { padding: 1rem; background: #fff7ed; }
    .pizza-desc { color: #64748b; min-height: 2.5rem; }
    .pizza-price { font-size: 1.5rem; font-weight: 700; color: #f97316; }
    .w-full { width: 100%; }
  `]
})
export class Menu implements OnInit {
  private pizzaService = inject(PizzaService);
  private cartService = inject(CartService);
  private messageService = inject(MessageService);

  pizzas = signal<Pizza[]>([]);
  loading = signal(true);

  ngOnInit() {
    this.pizzaService.getAll().subscribe({
      next: data => {
        this.pizzas.set(data.filter(p => p.isAvailable));
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
        this.messageService.add({ severity: 'error', summary: 'Erreur', detail: 'Impossible de charger le menu' });
      }
    });
  }

  addToCart(pizza: Pizza) {
    this.cartService.addToCart(pizza);
    this.messageService.add({ severity: 'success', summary: 'Ajoutee !', detail: `${pizza.name} ajoutee au panier`, life: 2000 });
  }
}
