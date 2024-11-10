import { Routes } from '@angular/router';

import { CustomerDashboardComponent } from './customer-dashboard/customer-dashboard.component';
import { ViewProductsComponent } from './view-products/view-products.component';
import { OrderProductsComponent } from './order-products/order-products.component';
import { CartDetailComponent } from './cart-detail/cart-detail.component';
import { UserDetailComponent } from './user-detail/user-detail.component';

const routes: Routes = [
  {
    path: 'customer',
    component: CustomerDashboardComponent,
    children: [
      { path: 'view-product', component: ViewProductsComponent},
      { path: 'order-product', component: OrderProductsComponent},
      { path: 'cart-details', component: CartDetailComponent},
      { path: 'user-details', component: UserDetailComponent},
      { path: '', redirectTo: 'view-product', pathMatch: 'full'}
    ]
  }
];

export default routes;
