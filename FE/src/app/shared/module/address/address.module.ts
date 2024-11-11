export interface Address {
  addressId: number;
  communeId: number;
  houseNumber: string;
  note: string;
}

export interface AddressRequest {
  communeId: number;
  houseNumber: string;
  note: string;
  communeName: string;
  districtId: number;
}

export interface AddressString {
  key: number,
  value: string
}

export function ConstructorAddress() {
  return {
    addressId: 0,
    communeId: 0,
    houseNumber: '',
    note: '',
  };
}
