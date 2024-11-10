import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CustomCurrencyPipe } from '../../module/customCurrency';
import { Product } from '../../module/product/product.module';

@Component({
  selector: 'app-product-item',
  standalone: true,
  imports: [
    CommonModule,
    CustomCurrencyPipe
  ],
  templateUrl: './product-item.component.html',
  styleUrl: './product-item.component.css'
})
export class ProductItemComponent {
  @Input() data: Product | undefined;
  @Output() dataChange: EventEmitter<any> = new EventEmitter<any>();

  sendData(Product: string | undefined) {   
    this.dataChange.emit(Product);
  }

}
