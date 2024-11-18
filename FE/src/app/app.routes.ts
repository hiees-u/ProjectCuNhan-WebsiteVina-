import { Routes } from '@angular/router';

import { LoginComponent } from './auth/login/login.component';
import { RegistreComponent } from './auth/registre/registre.component';
import CustomerRoutingModule from './customer/customer-routing.module';
import { ModeratorComponent } from './moderator/moderator.component';
import { OrderApproverComponent } from './order-approver/order-approver.component';
import { WarehouseEmployeeComponent } from './warehouse-employee/warehouse-employee.component';

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
<<<<<<< HEAD
    path: 'warehouse',
    component: WarehouseComponent,
  },
  {
    path: 'order-approver',
    component: OrderApproverComponent,
=======
    path: 'order-approver',
    component: OrderApproverComponent,
  },
  {
    path: 'warehouse-employee',
    component: WarehouseEmployeeComponent,
>>>>>>> 6d062dfc0f5a3c38a50e53d3f8e0cdcf59c5850f
  },
  {
    path: '',
    redirectTo: '/customer',
    pathMatch: 'full',
  },
];
