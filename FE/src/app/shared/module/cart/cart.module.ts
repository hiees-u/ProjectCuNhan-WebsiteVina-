export interface CartItem {
  CartId: number;
  image: string;
  productName: string;
  price: number;
  quantity: number;
  totalPrice: number;
  productId: number;
  checked?: boolean;
  priceHistoryId: number;
}

export interface CartResponse {
  productId: number;
  quantity: number;
}

export function constructorCartResponse(): CartResponse {
  return {
    productId: 0,
    quantity: 0,
  };
}

export function constructorCartItem(): CartItem {
  return {
    CartId: 0, // or assign a specific ID
    image: '', // provide a default image path or leave as empty string
    productName: '', // default name or empty
    price: 0, // default price
    quantity: 1, // default quantity
    totalPrice: 0,
    productId: 0,
    priceHistoryId: 0,
  };
}
