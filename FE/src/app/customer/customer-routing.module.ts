import { Routes } from '@angular/router';

import { CustomerDashboardComponent } from './customer-dashboard/customer-dashboard.component';
import { ViewProductsComponent } from './view-products/view-products.component';
import { OrderProductsComponent } from './order-products/order-products.component';
import { CartDetailComponent } from './cart-detail/cart-detail.component';
import { UserDetailComponent } from './user-detail/user-detail.component';
import { ContactComponent } from './contact/contact.component';
import { AboutUsComponent } from './about-us/about-us.component';
import { PaymentComponent } from './payment/payment.component';

const routes: Routes = [
  {
    path: 'customer',
    component: CustomerDashboardComponent,
    children: [
      { path: 'view-product', component: ViewProductsComponent},
      { path: 'order-product', component: OrderProductsComponent},
      { path: 'about-us', component: AboutUsComponent},
      { path: 'cart-details', component: CartDetailComponent},
      { path: 'user-details', component: UserDetailComponent},
      { path: 'contact', component: ContactComponent},
      { path: 'payment', component: PaymentComponent },
      { path: '', redirectTo: 'about-us', pathMatch: 'full'}
    ]
  }
];

export default routes;
