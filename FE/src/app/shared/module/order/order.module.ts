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

export interface OrderDetailModel {
  productname: string;
  image: string;
  price: number;
  quantity: number;
  totalprice: number;
  state: number;
  orderid: number;  // Thêm thuộc tính orderid
  pricehistoryid: number;
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

export function constructorOrderDetailModel() {
  return {
    productname: '',
    image: '',
    price: -1,
    quantity: -1,
    totalprice: -1,
    state: -1,
    pricehistoryid: -1
  };
}
