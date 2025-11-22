import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './core/gaurds/auth.guard';

const routes: Routes = [

  { path: '', redirectTo: '/home', pathMatch: 'full' },


  { path: 'home', loadChildren: () => import('./pages/pages-module').then(m => m.PagesModule) },
  { path: 'products', loadChildren: () => import('./products/products-module').then(m => m.ProductsModule) },
  { path: 'orders', loadChildren: () => import('./orders/orders-module').then(m => m.OrdersModule) },


  { path: 'admin', loadChildren: () => import('./admin/admin-module').then(m => m.AdminModule), canActivate: [AuthGuard], data: { roles: ['Admin'] } },
  { path: 'auth', loadChildren: () => import('./auth/auth-module').then(m => m.AuthModule) },

  { path: 'layout', loadChildren: () => import('./layout/layout-module').then(m => m.LayoutModule) },
  { path: '**', redirectTo: '/home' }


];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
