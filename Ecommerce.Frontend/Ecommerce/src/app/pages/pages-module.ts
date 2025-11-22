import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PagesRoutingModule } from './pages-routing-module';
import { Home } from './home/home';
import { About } from './about/about';
import { Contact } from './contact/contact';
import { LayoutModule } from '../layout/layout-module';


@NgModule({
  declarations: [
    Home,
    About,
    Contact
  ],
  imports: [
    CommonModule,
    PagesRoutingModule,
     LayoutModule
  ]
})
export class PagesModule { }
