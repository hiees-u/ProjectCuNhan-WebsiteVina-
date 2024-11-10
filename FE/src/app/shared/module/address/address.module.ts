export interface Address {
  addressId: number;
  communeId: number;
  houseNumber: string;
  note: string;
}

export function ConstructorAddress() {
  return {
    addressId: 0,
    communeId: 0,
    houseNumber: '',
    note: '',
  };
}
