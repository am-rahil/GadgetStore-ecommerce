import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminRoutingModule } from './admin-routing-module';
import { Dashboard } from './dashboard/dashboard';
import { ManageProducts } from './manage-products/manage-products';
import { ManageCategories } from './manage-categories/manage-categories';
import { ManageOrders } from './manage-orders/manage-orders';
import { ManageUsers } from './manage-users/manage-users';
import { AddProduct } from './add-product/add-product';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { UpdateProduct } from './update-product/update-product';
import { AdminLayout } from './admin-layout/admin-layout';
import { AgGridModule } from 'ag-grid-angular';



@NgModule({
  declarations: [
    Dashboard,
    ManageProducts,
    ManageCategories,
    ManageOrders,
    ManageUsers,
    AddProduct,
    UpdateProduct,
    AdminLayout,
  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    ReactiveFormsModule,
    FormsModule,
    AgGridModule
  ]
})
export class AdminModule { }
