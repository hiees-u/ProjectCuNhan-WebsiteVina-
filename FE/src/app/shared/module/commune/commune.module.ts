export interface CommuneResponseModel {
  communeId: number;
  communeName: string;
  districtId: number;
}

// @NgModule({
//   declarations: [],
//   imports: [CommonModule],
// })
// export class CommuneModule {}
export function ConstructorCommune() {
  return {
    communeId: -1,
    communeName: '',
    districtId: 0
  }
}