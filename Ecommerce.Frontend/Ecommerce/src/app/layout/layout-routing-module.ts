import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Navbar } from './navbar/navbar';
import { Footer } from './footer/footer';

const routes: Routes = [
  { path: 'navbar', component: Navbar },
  { path: 'footer', component: Footer }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LayoutRoutingModule { }
