export interface UserInfoResponseModel {
  accountName: string;
  fullName?: string;
  email?: string;
  address?: string;
  addressId: number;
  customerType?: string;
  phone?: string;
  gender?: number;
  commune: number; // Thuộc tính communeId
  district?: number; // Thuộc tính district
  province?: number;
}

export interface UserInfoRequestModel {
  fullName?: string;
  email?: string;
  phone?: string;
  addressId: number | 0;
  gender: number | 0;
  houseNumber: string;
  note: string;
  communeName: string;
  commune: number | 0;
  districtId: number | 0;
}

export function ConstructerUserInfoResponseModel() {
  return {
    accountName: '',
    fullName: '',
    email: '',
    address: '',
    addressId: 0,
    customerType: '',
    phone: '',
    gender: 0,
    commune: 0, // Thuộc tính communeId
    district: 0, // Thuộc tính district
    province: 0,
  };
}

export function ConstructorUserInfoRequestModel() {
  return {
    fullName: '',
    email: '',
    phone: '',
    addressId: 0,
    gender: 0,
    houseNumber: '',
    note: '',
    communeName: '',
    commune: 0,
    districtId: 0,
  };
}

