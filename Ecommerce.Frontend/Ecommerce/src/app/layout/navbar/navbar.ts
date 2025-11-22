import { Component } from '@angular/core';
import { Category } from '../../core/models/category.model';
import { Categoryservice } from '../../core/services/categoryservice';
import { Router } from '@angular/router';
import { Authservice } from '../../core/services/authservice';


@Component({
  selector: 'app-navbar',
  standalone: false,
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class Navbar {

  isLoggedIn = false;
  isAdmin = false;
  userName: string | null = null;
  searchQuery: string = '';

  categories: Category[] = [
    { categoryId: 1, categoryName: 'Smartphones' },
    { categoryId: 2, categoryName: 'Laptops' },
    { categoryId: 3, categoryName: 'Headphones' },
    { categoryId: 4, categoryName: 'Smart Watches' }
  ];

  constructor(
    private router: Router,
    private authService: Authservice
  ) { }

  // ngOnInit(): void {
  //   this.loadCategories();
  //   console.log('cat', this.categories)
  // }

  // loadCategories() {
  //   this.categoryservice.getallCategories().subscribe({
  //     next: (res: any) => {
  //       this.categories = res.response || [];
  //     },
  //     error: (err) => {
  //       console.error('Error fetching categories:', err);
  //     }
  //   });
  // }

  ngOnInit(): void {
    this.updateAuthStatus();
  }

  updateAuthStatus(): void {
    this.isLoggedIn = this.authService.isLoggedIn();
    this.userName = this.authService.getUserName();
    this.isAdmin = this.authService.getUserRole() === 'Admin'; // Check if admin
    // Redirect admin to admin dashboard
    if (this.isAdmin && !this.router.url.startsWith('/admin')) {
      this.router.navigate(['/admin/dashboard']);
    }
  }

  filterByCategory(categoryId: number): void {
    this.router.navigate(['/products/category', categoryId]);
  }

  logout(): void {
    this.authService.logout();
    this.isLoggedIn = false;
    this.userName = null;
    this.router.navigate(['/home']);
  }

  //search function
  searchProduct() {
    const q = this.searchQuery.trim();

    // 🔥 Always emit search — even empty string
    window.dispatchEvent(new CustomEvent('global-search', { detail: q }));
  }



}