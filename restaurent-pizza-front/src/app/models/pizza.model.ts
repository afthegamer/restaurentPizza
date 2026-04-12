export interface Pizza {
  id: string;
  name: string;
  description: string;
  price: number;
  isAvailable: boolean;
  isArchived: boolean;
  categoryId: string;
  categoryName: string;
}

export interface CreatePizzaDto {
  name: string;
  description: string;
  price: number;
  categoryId: string;
}

export interface UpdatePizzaDto {
  id: string;
  name: string;
  description: string;
  price: number;
  isAvailable: boolean;
}
