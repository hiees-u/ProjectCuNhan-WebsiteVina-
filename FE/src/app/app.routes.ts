import { Routes } from '@angular/router';

import { LoginComponent } from './auth/login/login.component';
import { RegistreComponent } from './auth/registre/registre.component';
import { EmployeeDashboardComponent } from './employee/employee-dashboard/employee-dashboard.component';
import CustomerRoutingModule from './customer/customer-routing.module';

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
    path: 'employee',
    component: EmployeeDashboardComponent,
  },
  {
    path: '',
    redirectTo: '/customer',
    pathMatch: 'full',
  },
];
