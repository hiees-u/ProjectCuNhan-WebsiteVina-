import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'customCurrency',
  standalone: true
})
export class CustomCurrencyPipe implements PipeTransform {
  transform(value: number): string {
    return value
      .toLocaleString('vi-VN', { style: 'currency', currency: 'VND' })
      .replace('₫', '₫');
  }
}
