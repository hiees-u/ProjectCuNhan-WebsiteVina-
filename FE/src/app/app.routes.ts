import { Routes } from '@angular/router';

import { LoginComponent } from './auth/login/login.component';
import { RegistreComponent } from './auth/registre/registre.component';
import CustomerRoutingModule from './customer/customer-routing.module';
import { OrderApproverDashboardComponent } from './order-approver/order-approver-dashboard/order-approver-dashboard.component';
// import { OrderApproverComponent } from './order-approver/order-approver-dashboard.component';
import { WarehouseEmployeeComponent } from './warehouse-employee/warehouse-employee.component';
import { ModeratorDashboardComponent } from './moderator/moderator-dashboard/moderator-dashboard.component';
import { ProductModeratorComponent } from './moderator/product-moderator/product-moderator.component';
import { CateModeratorComponent } from './moderator/cate-moderator/cate-moderator.component';
import { SubCateModeratorComponent } from './moderator/sub-cate-moderator/sub-cate-moderator.component';
import { CustomerModeratorComponent } from './moderator/customer-moderator/customer-moderator.component';
import { DepartmentModeratorComponent } from './moderator/department-moderator/department-moderator.component';
import { EmployeeModeratorComponent } from './moderator/employee-moderator/employee-moderator.component';
import { SupplierModeratorComponent } from './moderator/supplier-moderator/supplier-moderator.component';
import { WarehouseManagementComponent } from './warehouse-employee/warehouse-management/warehouse-management.component';
import { AddWarehouseComponent } from './warehouse-employee/add-warehouse/add-warehouse.component';

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
      { path: 'cate-moderator', component: CateModeratorComponent },
      { path: 'sub-cate-moderator', component: SubCateModeratorComponent },
      { path: 'customer-moderator', component: CustomerModeratorComponent },
      { path: 'department-moderator', component: DepartmentModeratorComponent },
      { path: 'employee-moderator', component: EmployeeModeratorComponent },
      { path: 'supplier-moderator', component: SupplierModeratorComponent },
      { path: '', redirectTo: 'product-moderator', pathMatch: 'full' },
    ],
  },
  {
    path: 'order-approver',
    component: OrderApproverDashboardComponent,
  },
  {
    path: 'warehouse-employee',
    component: WarehouseEmployeeComponent,
    children: [
      { path: 'warehouse-management', component: WarehouseManagementComponent },
      { path: 'add-warehouse', component: AddWarehouseComponent },
      { path: '', redirectTo: 'warehouse-management', pathMatch: 'full' }
    ]
  },
  {
    path: '',
    redirectTo: '/customer',
    pathMatch: 'full',
  },
];
