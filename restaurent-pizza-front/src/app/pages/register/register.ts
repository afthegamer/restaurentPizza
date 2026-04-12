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
  selector: 'app-register',
  imports: [FormsModule, RouterLink, InputText, Password, Button, Card],
  template: `
    <div class="auth-container">
      <p-card header="Inscription">
        <form (ngSubmit)="onSubmit()">
          <div class="field">
            <label for="firstName">Prenom</label>
            <input pInputText id="firstName" [(ngModel)]="firstName" name="firstName" class="w-full" />
          </div>
          <div class="field">
            <label for="lastName">Nom</label>
            <input pInputText id="lastName" [(ngModel)]="lastName" name="lastName" class="w-full" />
          </div>
          <div class="field">
            <label for="email">Email</label>
            <input pInputText id="email" [(ngModel)]="email" name="email" placeholder="votre@email.com" class="w-full" />
          </div>
          <div class="field">
            <label for="password">Mot de passe</label>
            <p-password id="password" [(ngModel)]="password" name="password" [toggleMask]="true" styleClass="w-full" inputStyleClass="w-full" />
          </div>
          <p-button type="submit" label="S'inscrire" [loading]="loading()" styleClass="w-full" />
        </form>
        <div class="auth-link">
          Deja un compte ? <a routerLink="/login">Se connecter</a>
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
export class Register {
  private auth = inject(AuthService);
  private messageService = inject(MessageService);

  firstName = '';
  lastName = '';
  email = '';
  password = '';
  loading = signal(false);

  onSubmit() {
    this.loading.set(true);
    this.auth.register({
      email: this.email,
      password: this.password,
      firstName: this.firstName,
      lastName: this.lastName
    }).subscribe({
      next: result => {
        this.auth.handleAuthSuccess(result);
        this.messageService.add({ severity: 'success', summary: 'Bienvenue !', detail: 'Compte cree avec succes' });
      },
      error: err => {
        this.loading.set(false);
        this.messageService.add({ severity: 'error', summary: 'Erreur', detail: err.error?.detail || 'Inscription impossible' });
      }
    });
  }
}
