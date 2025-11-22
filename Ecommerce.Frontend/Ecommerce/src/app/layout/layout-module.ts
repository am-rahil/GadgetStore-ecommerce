import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LayoutRoutingModule } from './layout-routing-module';
import { Navbar } from './navbar/navbar';
import { Footer } from './footer/footer';
import { FormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    Navbar,
    Footer
  ],
  imports: [
    CommonModule,
    LayoutRoutingModule,
    FormsModule
  ],
  exports:[
    Navbar,Footer
  ]
})
export class LayoutModule { }
