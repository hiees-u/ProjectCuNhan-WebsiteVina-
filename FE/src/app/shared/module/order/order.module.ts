export interface OrderRequestModule {
  phone: string;
  addressId: number;
  nameRecipient: string;
}

export function constructorOrderRequestModule() {
  return {
    phone: '',
    addressId: -1,
    nameRecipient: '',
  };
}
