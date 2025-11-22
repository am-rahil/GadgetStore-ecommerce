import { ChangeDetectorRef, Component } from '@angular/core';
import { Authservice } from '../../core/services/authservice';
import { Orderservice } from '../../core/services/orderservice';
import { Router } from '@angular/router';

@Component({
  selector: 'app-order-list',
  standalone: false,
  templateUrl: './order-list.html',
  styleUrl: './order-list.css'
})
export class OrderList {

  orders: any[] = [];
  loading = true;
  userId: number | null = null;
  statusFilter: string = '';
  filteredOrders: any[] = [];

  constructor(private orderService: Orderservice, private authService: Authservice, private cd: ChangeDetectorRef, private router: Router) { }

  ngOnInit(): void {
    this.userId = this.authService.getUserId();
    if (this.userId) {
      this.loadOrders();
    }
  }

  loadOrders(): void {
    this.orderService.getUserOrders(this.userId!).subscribe({
      next: (res) => {
        let allOrders = res;
        this.orders = [
          ...allOrders.filter((o: any) => o.status === 'Pending'),
          ...allOrders.filter((o: any) => o.status === 'Shipped'),
          ...allOrders.filter((o: any) => o.status === 'Delivered'),
          ...allOrders.filter((o: any) => o.status === 'Cancelled')
        ];
        this.filteredOrders = this.orders; 
        this.loading = false;
        this.cd.detectChanges();
      },
      error: (err) => {
        console.error('Error fetching orders:', err);
        this.loading = false;
      }
    });
  }

  cancelOrder(orderId: number) {
    if (!confirm('Are you sure you want to cancel this order?')) return;

    this.orderService.updateOrderStatus(orderId, 'Cancelled').subscribe({
      next: (res) => {
        console.log(' Success:', res);
        alert('Order cancelled successfully!');
        setTimeout(() => this.loadOrders(), 400);
      },
      error: (err) => {
        console.error(' Error:', err);
        alert('Failed to cancel order');
      }
    });
  }

  viewOrderDetails(orderId: number) {
    console.log('clicked')
    this.router.navigate(['/orders/orderDetail', orderId]);
  }
  // SET FILTER
  setFilter(status: string) {
    this.statusFilter = status;

    if (status === '') {
      this.filteredOrders = this.orders;   // all orders
    } else {
      this.filteredOrders = this.orders.filter(o => o.status === status);
    }
  }

}
