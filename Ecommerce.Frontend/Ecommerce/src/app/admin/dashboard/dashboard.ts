import { ChangeDetectorRef, Component } from '@angular/core';
import { Authservice } from '../../core/services/authservice';
import { Router } from '@angular/router';
import { Orderservice } from '../../core/services/orderservice';
import { Productservice } from '../../core/services/productservice';
import { Categoryservice } from '../../core/services/categoryservice';

@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard {

  totalOrders = 0;
  totalUsers = 0;
  totalProducts = 0;
  totalCategories = 0;
  totalSales = 0;
  recentOrders: any[] = [];
  loading = true;

  constructor(
    private orderService: Orderservice,
    private productService: Productservice,
    private categoryService: Categoryservice,
    private authService: Authservice,
    private cd: ChangeDetectorRef
  ) { }


  ngOnInit(): void {
    this.loadDashboardData();
    this.cd.detectChanges();
  }
  loadDashboardData(): void {
    this.loading = true;
    this.cd.detectChanges();

    Promise.all([
      this.orderService.getAllOrders().toPromise(),
      this.productService.getAllProducts().toPromise(),
      this.categoryService.getallCategories().toPromise(),
      this.authService.getAllUsers().toPromise()
    ])
      .then(([ordersRes, productsRes, categoriesRes, usersRes]) => {
        const orders = ordersRes.response || ordersRes || [];
        const products = productsRes.response || productsRes || [];
        const categories = categoriesRes.response || categoriesRes || [];
        const users = usersRes.response || usersRes || [];

        this.totalOrders = orders.length;
        this.totalProducts = products.length;
        this.totalCategories = categories.length;
        this.totalUsers = users.length;

        this.totalSales = orders
          .filter((o: any) => o.status === 'Delivered')
          .reduce((sum: number, o: any) => sum + (o.totalAmount || 0), 0);

        this.recentOrders = orders
          .sort((a: any, b: any) => new Date(b.orderDate).getTime() - new Date(a.orderDate).getTime())
          .slice(0, 5);
          this.cd.detectChanges();
      })
      .catch(err => {
        console.error('Error loading dashboard:', err);
      })
      .finally(() => {
        this.loading = false;
        this.cd.detectChanges(); //  Angular will now detect this correctly
      });
  }
}
