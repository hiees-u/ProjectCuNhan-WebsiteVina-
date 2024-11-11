export interface OrderRequestModule {
  phone: string;
  addressId: number;
  nameRecipient: string;
  products: ProductQuantity[];
}

export interface OrderResponseModel {
  orderId: number;
  phone: string;
  addressId: number;
  nameRecipient: string;
  createBy: number;
  totalPayment: number;
  createAt?: Date;
  state: number;
}

export interface ProductQuantity {
  PriceHistoryId: number;
  Quantity: number;
}

export function constructorOrderRequestModule() {
  return {
    phone: '',
    addressId: -1,
    nameRecipient: '',
  };
}

export function constructorOrderResponseModel() {
  return {
    orderId: 0,
    phone: '',
    addressId: 0,
    nameRecipient: '',
    createBy: 0,
    totalPayment: 0,
    createAt: new Date(),
    state: 0,
  };
}
