import { Routes } from '@angular/router';

import { LoginComponent } from './auth/login/login.component';
import { RegistreComponent } from './auth/registre/registre.component';
import CustomerRoutingModule from './customer/customer-routing.module';

import { ModeratorComponent } from './moderator/moderator.component';
import { WarehouseComponent } from './warehouse/warehouse.component';
import { OrderApproverComponent } from './order-approver/order-approver.component';

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
    // component: CustomerDashboardComponent,
    loadChildren: () =>
      import('./customer/customer-routing.module').then(
        (m) => m.default
      )
  },
  {
    path: 'moderator',
    component: ModeratorComponent,
  },
  {
    path: 'warehouse',
    component: WarehouseComponent,
  },
  {
    path: 'order-approver',
    component: OrderApproverComponent,
  },
  {
    path: '',
    redirectTo: '/customer',
    pathMatch: 'full',
  },
];
