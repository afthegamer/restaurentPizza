import { Injectable, signal, computed } from '@angular/core';
import { Pizza } from '../models/pizza.model';

export interface CartItem {
  pizza: Pizza;
  quantity: number;
}

@Injectable({ providedIn: 'root' })
export class CartService {
  private items = signal<CartItem[]>([]);

  readonly cartItems = computed(() => this.items());
  readonly itemCount = computed(() => this.items().reduce((sum, i) => sum + i.quantity, 0));
  readonly total = computed(() => this.items().reduce((sum, i) => sum + i.pizza.price * i.quantity, 0));

  addToCart(pizza: Pizza) {
    const current = this.items();
    const existing = current.find(i => i.pizza.id === pizza.id);

    if (existing) {
      this.items.set(current.map(i =>
        i.pizza.id === pizza.id ? { ...i, quantity: i.quantity + 1 } : i
      ));
    } else {
      this.items.set([...current, { pizza, quantity: 1 }]);
    }
  }

  removeFromCart(pizzaId: string) {
    this.items.set(this.items().filter(i => i.pizza.id !== pizzaId));
  }

  updateQuantity(pizzaId: string, quantity: number) {
    if (quantity <= 0) {
      this.removeFromCart(pizzaId);
      return;
    }
    this.items.set(this.items().map(i =>
      i.pizza.id === pizzaId ? { ...i, quantity } : i
    ));
  }

  clear() {
    this.items.set([]);
  }
}
