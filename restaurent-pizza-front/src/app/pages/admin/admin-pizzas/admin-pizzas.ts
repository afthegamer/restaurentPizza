import { Component, inject, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CurrencyPipe } from '@angular/common';
import { PizzaService } from '../../../services/pizza.service';
import { CategoryService } from '../../../services/category.service';
import { MessageService, ConfirmationService } from 'primeng/api';
import { Pizza, CreatePizzaDto } from '../../../models/pizza.model';
import { Category } from '../../../models/category.model';
import { TableModule } from 'primeng/table';
import { Button } from 'primeng/button';
import { Dialog } from 'primeng/dialog';
import { InputText } from 'primeng/inputtext';
import { InputNumber } from 'primeng/inputnumber';
import { Select } from 'primeng/select';
import { ConfirmDialog } from 'primeng/confirmdialog';
import { Tag } from 'primeng/tag';

@Component({
  selector: 'app-admin-pizzas',
  imports: [FormsModule, CurrencyPipe, TableModule, Button, Dialog, InputText, InputNumber, Select, ConfirmDialog, Tag],
  providers: [ConfirmationService],
  template: `
    <div class="admin-header">
      <h1>Gestion des Pizzas</h1>
      <p-button label="Nouvelle pizza" icon="pi pi-plus" (onClick)="openNew()" />
    </div>

    <p-table [value]="pizzas()" [tableStyle]="{ 'min-width': '50rem' }">
      <ng-template #header>
        <tr>
          <th>Nom</th>
          <th>Description</th>
          <th>Categorie</th>
          <th>Prix</th>
          <th>Disponible</th>
          <th>Actions</th>
        </tr>
      </ng-template>
      <ng-template #body let-pizza>
        <tr>
          <td>{{ pizza.name }}</td>
          <td>{{ pizza.description }}</td>
          <td><p-tag [value]="pizza.categoryName" severity="info" /></td>
          <td>{{ pizza.price | currency:'EUR':'symbol':'1.2-2' }}</td>
          <td>
            <p-tag [value]="pizza.isAvailable ? 'Oui' : 'Non'" [severity]="pizza.isAvailable ? 'success' : 'danger'" />
          </td>
          <td>
            <p-button icon="pi pi-pencil" [rounded]="true" [text]="true" (onClick)="editPizza(pizza)" />
            <p-button icon="pi pi-trash" [rounded]="true" [text]="true" severity="danger" (onClick)="confirmDelete(pizza)" />
          </td>
        </tr>
      </ng-template>
    </p-table>

    <p-dialog [(visible)]="dialogVisible" [header]="editing ? 'Modifier la pizza' : 'Nouvelle pizza'" [modal]="true" [style]="{ width: '450px' }">
      <div class="field">
        <label>Nom</label>
        <input pInputText [(ngModel)]="form.name" class="w-full" />
      </div>
      <div class="field">
        <label>Description</label>
        <input pInputText [(ngModel)]="form.description" class="w-full" />
      </div>
      <div class="field">
        <label>Prix</label>
        <p-inputNumber [(ngModel)]="form.price" mode="currency" currency="EUR" locale="fr-FR" class="w-full" />
      </div>
      @if (!editing) {
        <div class="field">
          <label>Categorie</label>
          <p-select [(ngModel)]="form.categoryId" [options]="categories()" optionLabel="name" optionValue="id" placeholder="Choisir..." styleClass="w-full" />
        </div>
      }
      <ng-template #footer>
        <p-button label="Annuler" severity="secondary" [text]="true" (onClick)="dialogVisible = false" />
        <p-button label="Sauvegarder" (onClick)="save()" />
      </ng-template>
    </p-dialog>

    <p-confirmDialog />
  `,
  styles: [`
    .admin-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 1.5rem; }
    h1 { color: #1e293b; }
    .field { margin-bottom: 1.25rem; }
    .field label { display: block; margin-bottom: 0.5rem; font-weight: 600; }
    .w-full { width: 100%; }
  `]
})
export class AdminPizzas implements OnInit {
  private pizzaService = inject(PizzaService);
  private categoryService = inject(CategoryService);
  private messageService = inject(MessageService);
  private confirmService = inject(ConfirmationService);

  pizzas = signal<Pizza[]>([]);
  categories = signal<Category[]>([]);
  dialogVisible = false;
  editing = false;
  editId = '';
  form: any = { name: '', description: '', price: 0, categoryId: '', isAvailable: true };

  ngOnInit() {
    this.load();
    this.categoryService.getAll().subscribe(data => this.categories.set(data));
  }

  load() {
    this.pizzaService.getAll().subscribe(data => this.pizzas.set(data));
  }

  openNew() {
    this.form = { name: '', description: '', price: 0, categoryId: '', isAvailable: true };
    this.editing = false;
    this.dialogVisible = true;
  }

  editPizza(pizza: Pizza) {
    this.form = { name: pizza.name, description: pizza.description, price: pizza.price, categoryId: pizza.categoryId, isAvailable: pizza.isAvailable };
    this.editId = pizza.id;
    this.editing = true;
    this.dialogVisible = true;
  }

  save() {
    if (this.editing) {
      this.pizzaService.update({ id: this.editId, ...this.form }).subscribe({
        next: () => { this.dialogVisible = false; this.load(); this.messageService.add({ severity: 'success', summary: 'Modifiee', detail: 'Pizza modifiee' }); },
        error: err => this.messageService.add({ severity: 'error', summary: 'Erreur', detail: err.error?.detail || 'Erreur' })
      });
    } else {
      const dto: CreatePizzaDto = { name: this.form.name, description: this.form.description, price: this.form.price, categoryId: this.form.categoryId };
      this.pizzaService.create(dto).subscribe({
        next: () => { this.dialogVisible = false; this.load(); this.messageService.add({ severity: 'success', summary: 'Creee', detail: 'Pizza creee' }); },
        error: err => this.messageService.add({ severity: 'error', summary: 'Erreur', detail: err.error?.detail || 'Erreur' })
      });
    }
  }

  confirmDelete(pizza: Pizza) {
    this.confirmService.confirm({
      message: `Supprimer "${pizza.name}" ?`,
      header: 'Confirmation',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.pizzaService.delete(pizza.id).subscribe({
          next: () => { this.load(); this.messageService.add({ severity: 'success', summary: 'Supprimee', detail: 'Pizza supprimee' }); },
          error: err => this.messageService.add({ severity: 'error', summary: 'Erreur', detail: err.error?.detail || 'Erreur' })
        });
      }
    });
  }
}
