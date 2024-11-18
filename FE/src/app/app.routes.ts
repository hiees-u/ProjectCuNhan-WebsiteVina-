import { Routes } from '@angular/router';

import { LoginComponent } from './auth/login/login.component';
import { RegistreComponent } from './auth/registre/registre.component';
import CustomerRoutingModule from './customer/customer-routing.module';
import { OrderApproverComponent } from './order-approver/order-approver.component';
import { WarehouseEmployeeComponent } from './warehouse-employee/warehouse-employee.component';
import { ModeratorDashboardComponent } from './moderator/moderator-dashboard/moderator-dashboard.component';
import { ProductModeratorComponent } from './moderator/product-moderator/product-moderator.component';

export const routes: Routes = [
  ...CustomerRoutingModule,
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: 'register',
    component: RegistreComponent,
  },
  {
    path: 'customer',
    loadChildren: () =>
      import('./customer/customer-routing.module').then((m) => m.default),
  },
  {
    path: 'moderator',
    component: ModeratorDashboardComponent,
    children: [
      { path: 'product-moderator', component: ProductModeratorComponent },
      { path: '', redirectTo: 'product-moderator', pathMatch: 'full' },
    ],
  },
  {
    path: 'order-approver',
    component: OrderApproverComponent,
  },
  {
    path: 'warehouse-employee',
    component: WarehouseEmployeeComponent,
  },
  {
    path: '',
    redirectTo: '/customer',
    pathMatch: 'full',
  },
];
