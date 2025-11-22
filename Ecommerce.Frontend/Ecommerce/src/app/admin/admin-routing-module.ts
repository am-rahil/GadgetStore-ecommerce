import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Dashboard } from './dashboard/dashboard';
import { ManageProducts } from './manage-products/manage-products';
import { ManageCategories } from './manage-categories/manage-categories';
import { AddProduct } from './add-product/add-product';
import { ManageOrders } from './manage-orders/manage-orders';
import { AuthGuard } from '../core/gaurds/auth.guard';
import { ManageUsers } from './manage-users/manage-users';
import { UpdateProduct } from './update-product/update-product';
import { AdminLayout } from './admin-layout/admin-layout';



const routes: Routes = [
  {
    path: '',
    component: AdminLayout,
    canActivate: [AuthGuard],
    data: { roles: ['Admin'] },
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: Dashboard },
      { path: 'manage-products', component: ManageProducts },
      { path: 'manage-categories', component: ManageCategories },
      { path: 'add-product', component: AddProduct },
      { path: 'manageOrders', component: ManageOrders },
      { path: 'manageUsers', component: ManageUsers },
      { path: 'updateProduct/:productId', component: UpdateProduct },]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
