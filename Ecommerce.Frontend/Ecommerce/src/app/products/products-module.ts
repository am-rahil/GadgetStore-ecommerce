import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProductsRoutingModule } from './products-routing-module';
import { ProductList } from './product-list/product-list';
import { ProductDetails } from './product-details/product-details';
import { CategoryProducts } from './category-products/category-products';
import { LayoutModule } from '../layout/layout-module';
import { FormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    ProductList,
    ProductDetails,
    CategoryProducts
  ],
  imports: [
    CommonModule,
    ProductsRoutingModule,
    LayoutModule,
    FormsModule
  ]
})
export class ProductsModule { }
