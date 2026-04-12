import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../core/auth.service';
import { MessageService } from 'primeng/api';
import { InputText } from 'primeng/inputtext';
import { Password } from 'primeng/password';
import { Button } from 'primeng/button';
import { Card } from 'primeng/card';

@Component({
  selector: 'app-login',
  imports: [FormsModule, RouterLink, InputText, Password, Button, Card],
  template: `
    <div class="auth-container">
      <p-card header="Connexion">
        <form (ngSubmit)="onSubmit()">
          <div class="field">
            <label for="email">Email</label>
            <input pInputText id="email" [(ngModel)]="email" name="email" placeholder="votre@email.com" class="w-full" />
          </div>
          <div class="field">
            <label for="password">Mot de passe</label>
            <p-password id="password" [(ngModel)]="password" name="password" [feedback]="false" [toggleMask]="true" styleClass="w-full" inputStyleClass="w-full" />
          </div>
          <p-button type="submit" label="Se connecter" [loading]="loading()" styleClass="w-full" />
        </form>
        <div class="auth-link">
          Pas encore de compte ? <a routerLink="/register">S'inscrire</a>
        </div>
      </p-card>
    </div>
  `,
  styles: [`
    .auth-container {
      display: flex;
      justify-content: center;
      align-items: center;
      min-height: 60vh;
    }
    :host ::ng-deep .p-card { width: 400px; }
    .field { margin-bottom: 1.25rem; }
    .field label { display: block; margin-bottom: 0.5rem; font-weight: 600; }
    .w-full { width: 100%; }
    .auth-link { margin-top: 1rem; text-align: center; }
    .auth-link a { color: #f97316; text-decoration: none; font-weight: 600; }
  `]
})
export class Login {
  private auth = inject(AuthService);
  private messageService = inject(MessageService);

  email = '';
  password = '';
  loading = signal(false);

  onSubmit() {
    this.loading.set(true);
    this.auth.login({ email: this.email, password: this.password }).subscribe({
      next: result => {
        this.auth.handleAuthSuccess(result);
        this.messageService.add({ severity: 'success', summary: 'Bienvenue !', detail: `Bonjour ${result.firstName}` });
      },
      error: err => {
        this.loading.set(false);
        this.messageService.add({ severity: 'error', summary: 'Erreur', detail: err.error?.detail || 'Connexion impossible' });
      }
    });
  }
}
