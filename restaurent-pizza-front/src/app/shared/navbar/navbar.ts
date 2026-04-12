import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../core/auth.service';
import { CartService } from '../../core/cart.service';
import { Button } from 'primeng/button';
import { Badge } from 'primeng/badge';

@Component({
  selector: 'app-navbar',
  imports: [RouterLink, RouterLinkActive, Button, Badge],
  template: `
    <nav class="navbar">
      <div class="navbar-brand">
        <a routerLink="/menu" class="brand-link">Pizza Restaurant</a>
      </div>

      @if (auth.isLoggedIn()) {
        <div class="navbar-links">
          <a routerLink="/menu" routerLinkActive="active">Menu</a>
          <a routerLink="/cart" routerLinkActive="active">
            Panier
            @if (cart.itemCount() > 0) {
              <p-badge [value]="cart.itemCount().toString()" severity="danger" />
            }
          </a>
          <a routerLink="/orders" routerLinkActive="active">Mes commandes</a>

          @if (auth.isAdmin()) {
            <span class="separator">|</span>
            <a routerLink="/admin/pizzas" routerLinkActive="active">Pizzas</a>
            <a routerLink="/admin/categories" routerLinkActive="active">Categories</a>
            <a routerLink="/admin/orders" routerLinkActive="active">Commandes</a>
          }
        </div>

        <div class="navbar-user">
          <span class="user-name">{{ auth.user()?.firstName }}</span>
          <span class="user-role">({{ auth.user()?.role }})</span>
          <p-button label="Deconnexion" severity="secondary" [text]="true" size="small" (onClick)="auth.logout()" />
        </div>
      }
    </nav>
  `,
  styles: [`
    .navbar {
      display: flex;
      align-items: center;
      padding: 0.75rem 2rem;
      background: #1e293b;
      color: white;
      gap: 2rem;
    }
    .brand-link {
      color: #f97316;
      font-size: 1.25rem;
      font-weight: 700;
      text-decoration: none;
    }
    .navbar-links {
      display: flex;
      gap: 1rem;
      align-items: center;
      flex: 1;
    }
    .navbar-links a {
      color: #cbd5e1;
      text-decoration: none;
      padding: 0.25rem 0.5rem;
      border-radius: 4px;
      transition: color 0.2s;
      display: flex;
      align-items: center;
      gap: 0.35rem;
    }
    .navbar-links a:hover, .navbar-links a.active {
      color: white;
    }
    .separator { color: #475569; }
    .navbar-user {
      display: flex;
      align-items: center;
      gap: 0.5rem;
    }
    .user-name { font-weight: 600; }
    .user-role { color: #94a3b8; font-size: 0.85rem; }
  `]
})
export class Navbar {
  readonly auth = inject(AuthService);
  readonly cart = inject(CartService);
}
