export interface Category {
  id: string;
  name: string;
  description: string;
}

export interface CreateCategoryDto {
  name: string;
  description: string;
}

export interface UpdateCategoryDto {
  id: string;
  name: string;
  description: string;
}
