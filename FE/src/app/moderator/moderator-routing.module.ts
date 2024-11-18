import { Routes } from '@angular/router';
import { ModeratorDashboardComponent } from './moderator-dashboard/moderator-dashboard.component';
import { ProductModeratorComponent } from './product-moderator/product-moderator.component';

const routes: Routes = [
  {
    path: 'moderator',
    component: ModeratorDashboardComponent,
    children: [
      { path: 'product-moderator', component: ProductModeratorComponent },
      { path: '', redirectTo: 'product-moderator', pathMatch: 'full' },
    ],
  },
];

export default routes;