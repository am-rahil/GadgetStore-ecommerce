import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Checkout } from './checkout/checkout';
import { CartPage } from './cart-page/cart-page';
import { OrderList } from './order-list/order-list';
import { OrderDetails } from './order-details/order-details';

const routes: Routes = [
  { path: 'checkout', component: Checkout },
  { path: 'cartpage', component: CartPage },
  { path: 'orderlist', component: OrderList },
  { path: 'orderDetail/:orderId', component: OrderDetails },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OrdersRoutingModule { }
