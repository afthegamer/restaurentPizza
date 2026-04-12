export interface Order {
  id: string;
  status: string;
  total: number;
  createdOn: string;
  items: OrderItem[];
}

export interface OrderItem {
  id: string;
  pizzaId: string;
  pizzaName: string;
  quantity: number;
  unitPrice: number;
  lineTotal: number;
}

export interface CreateOrderDto {
  items: CreateOrderItemDto[];
}

export interface CreateOrderItemDto {
  pizzaId: string;
  quantity: number;
}
