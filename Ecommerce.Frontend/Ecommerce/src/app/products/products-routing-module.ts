import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CategoryProducts } from './category-products/category-products';
import { ProductDetails } from './product-details/product-details';

const routes: Routes = [
  { path: 'category/:categoryId', component: CategoryProducts },
  { path: 'product', component: ProductDetails },
  { path: 'productdetail/:productId', component: ProductDetails },
  // Search route
  { path: 'search/:query', component: CategoryProducts }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProductsRoutingModule { }
