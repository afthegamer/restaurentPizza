import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Category, CreateCategoryDto, UpdateCategoryDto } from '../models/category.model';

@Injectable({ providedIn: 'root' })
export class CategoryService {
  constructor(private http: HttpClient) {}

  getAll() {
    return this.http.get<Category[]>('/category');
  }

  create(dto: CreateCategoryDto) {
    return this.http.post<Category>('/category', dto);
  }

  update(dto: UpdateCategoryDto) {
    return this.http.put<void>(`/category/${dto.id}`, dto);
  }

  delete(id: string) {
    return this.http.delete<void>(`/category/${id}`);
  }
}
