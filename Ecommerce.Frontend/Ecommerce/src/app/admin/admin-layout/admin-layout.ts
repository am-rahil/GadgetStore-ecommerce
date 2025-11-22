import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Authservice } from '../../core/services/authservice';

@Component({
  selector: 'app-admin-layout',
  standalone: false,
  templateUrl: './admin-layout.html',
  styleUrl: './admin-layout.css'
})
export class AdminLayout {
  constructor(private auth: Authservice, private router: Router) { }


  logout() {
    this.auth.logout();
    this.router.navigate(['/auth/login']);
  }
}
