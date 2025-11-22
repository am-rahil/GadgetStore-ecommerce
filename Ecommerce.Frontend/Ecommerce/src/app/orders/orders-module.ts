import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { OrdersRoutingModule } from './orders-routing-module';
import { OrderList } from './order-list/order-list';
import { OrderDetails } from './order-details/order-details';
import { Checkout } from './checkout/checkout';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LayoutModule } from '../layout/layout-module';
import { PagesRoutingModule } from '../pages/pages-routing-module';
import { CartPage } from './cart-page/cart-page';
import { HttpClientModule } from '@angular/common/http';


@NgModule({
  declarations: [
    OrderList,
    OrderDetails,
    Checkout,
    CartPage
  ],
  imports: [
    CommonModule,
    OrdersRoutingModule,
    FormsModule,
    LayoutModule,
    PagesRoutingModule,
    ReactiveFormsModule,
    HttpClientModule
  ]
})
export class OrdersModule { }
