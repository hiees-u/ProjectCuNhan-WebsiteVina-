export interface CommuneResponseModel {
  communeId: number;
  communeName: string;
  districtId: number;
}

export function ConstructorCommune() {
  return {
    communeId: -1,
    communeName: '',
    districtId: 0
  }
}