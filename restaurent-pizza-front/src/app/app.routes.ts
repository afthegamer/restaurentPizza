import { Routes } from '@angular/router';
import { authGuard } from './core/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'menu', pathMatch: 'full' },
  { path: 'login', loadComponent: () => import('./pages/login/login').then(m => m.Login) },
  { path: 'register', loadComponent: () => import('./pages/register/register').then(m => m.Register) },
  { path: 'menu', loadComponent: () => import('./pages/menu/menu').then(m => m.Menu), canActivate: [authGuard] },
  { path: 'cart', loadComponent: () => import('./pages/cart/cart').then(m => m.Cart), canActivate: [authGuard] },
  { path: 'orders', loadComponent: () => import('./pages/orders/orders').then(m => m.Orders), canActivate: [authGuard] },
  { path: 'admin/pizzas', loadComponent: () => import('./pages/admin/admin-pizzas/admin-pizzas').then(m => m.AdminPizzas), canActivate: [authGuard] },
  { path: 'admin/categories', loadComponent: () => import('./pages/admin/admin-categories/admin-categories').then(m => m.AdminCategories), canActivate: [authGuard] },
  { path: 'admin/orders', loadComponent: () => import('./pages/admin/admin-orders/admin-orders').then(m => m.AdminOrders), canActivate: [authGuard] },
  { path: '**', redirectTo: 'menu' }
];
