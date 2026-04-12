import { Component, inject, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CategoryService } from '../../../services/category.service';
import { MessageService, ConfirmationService } from 'primeng/api';
import { Category } from '../../../models/category.model';
import { TableModule } from 'primeng/table';
import { Button } from 'primeng/button';
import { Dialog } from 'primeng/dialog';
import { InputText } from 'primeng/inputtext';
import { ConfirmDialog } from 'primeng/confirmdialog';

@Component({
  selector: 'app-admin-categories',
  imports: [FormsModule, TableModule, Button, Dialog, InputText, ConfirmDialog],
  providers: [ConfirmationService],
  template: `
    <div class="admin-header">
      <h1>Gestion des Categories</h1>
      <p-button label="Nouvelle categorie" icon="pi pi-plus" (onClick)="openNew()" />
    </div>

    <p-table [value]="categories()" [tableStyle]="{ 'min-width': '30rem' }">
      <ng-template #header>
        <tr>
          <th>Nom</th>
          <th>Description</th>
          <th>Actions</th>
        </tr>
      </ng-template>
      <ng-template #body let-cat>
        <tr>
          <td>{{ cat.name }}</td>
          <td>{{ cat.description }}</td>
          <td>
            <p-button icon="pi pi-pencil" [rounded]="true" [text]="true" (onClick)="editCat(cat)" />
            <p-button icon="pi pi-trash" [rounded]="true" [text]="true" severity="danger" (onClick)="confirmDelete(cat)" />
          </td>
        </tr>
      </ng-template>
    </p-table>

    <p-dialog [(visible)]="dialogVisible" [header]="editing ? 'Modifier' : 'Nouvelle categorie'" [modal]="true" [style]="{ width: '400px' }">
      <div class="field">
        <label>Nom</label>
        <input pInputText [(ngModel)]="form.name" class="w-full" />
      </div>
      <div class="field">
        <label>Description</label>
        <input pInputText [(ngModel)]="form.description" class="w-full" />
      </div>
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
export class AdminCategories implements OnInit {
  private catService = inject(CategoryService);
  private messageService = inject(MessageService);
  private confirmService = inject(ConfirmationService);

  categories = signal<Category[]>([]);
  dialogVisible = false;
  editing = false;
  editId = '';
  form = { name: '', description: '' };

  ngOnInit() { this.load(); }

  load() { this.catService.getAll().subscribe(data => this.categories.set(data)); }

  openNew() {
    this.form = { name: '', description: '' };
    this.editing = false;
    this.dialogVisible = true;
  }

  editCat(cat: Category) {
    this.form = { name: cat.name, description: cat.description };
    this.editId = cat.id;
    this.editing = true;
    this.dialogVisible = true;
  }

  save() {
    if (this.editing) {
      this.catService.update({ id: this.editId, ...this.form }).subscribe({
        next: () => { this.dialogVisible = false; this.load(); this.messageService.add({ severity: 'success', summary: 'OK', detail: 'Categorie modifiee' }); },
        error: err => this.messageService.add({ severity: 'error', summary: 'Erreur', detail: err.error?.detail || 'Erreur' })
      });
    } else {
      this.catService.create(this.form).subscribe({
        next: () => { this.dialogVisible = false; this.load(); this.messageService.add({ severity: 'success', summary: 'OK', detail: 'Categorie creee' }); },
        error: err => this.messageService.add({ severity: 'error', summary: 'Erreur', detail: err.error?.detail || 'Erreur' })
      });
    }
  }

  confirmDelete(cat: Category) {
    this.confirmService.confirm({
      message: `Supprimer "${cat.name}" ?`,
      header: 'Confirmation',
      accept: () => {
        this.catService.delete(cat.id).subscribe({
          next: () => { this.load(); this.messageService.add({ severity: 'success', summary: 'OK', detail: 'Categorie supprimee' }); },
          error: err => this.messageService.add({ severity: 'error', summary: 'Erreur', detail: err.error?.detail || 'Erreur' })
        });
      }
    });
  }
}
