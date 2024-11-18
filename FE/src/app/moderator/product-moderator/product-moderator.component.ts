import { Component } from '@angular/core';
import { ModeratorService } from '../moderator.service';
import { ProductModerator } from '../moderator.module';

@Component({
  selector: 'app-product-moderator',
  standalone: true,
  imports: [],
  templateUrl: './product-moderator.component.html',
  styleUrl: './product-moderator.component.css',
})
export class ProductModeratorComponent {

  constructor(private service: ModeratorService) {}

  products: ProductModerator[] = [];

  ngOnInit(): void {
    this.getProduct();
  }
  
  getProduct(): void {
    this.service.getProducts(null,null,null, null, null, 1, 10, 0, 0)
      .then(data => {
        this.products = data.data;
        console.log(this.products);
      }).catch(error => {
        console.error('Error fetching product', error);
      })

  }
}
