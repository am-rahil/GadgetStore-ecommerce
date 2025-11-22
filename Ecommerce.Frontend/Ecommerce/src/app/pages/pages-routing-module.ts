import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Home } from './home/home';
import { About } from './about/about';
import { Contact } from './contact/contact';

const routes: Routes = [
  { path: '', component: Home },        // ✅ loads at /home
  { path: 'about', component: About },  // /home/about
  { path: 'contact', component: Contact } // /home/contact
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PagesRoutingModule { }
