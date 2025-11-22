import { ChangeDetectorRef, Component } from '@angular/core';
import { Cartservice } from '../../core/services/cartservice';
import { Authservice } from '../../core/services/authservice';
import { Route, Router } from '@angular/router';
import { Product } from '../../core/models/product.model';

@Component({
  selector: 'app-cart-page',
  standalone: false,
  templateUrl: './cart-page.html',
  styleUrl: './cart-page.css'
})
export class CartPage {
  cartItems: any[] = [];
  total = 0;
  loading = true;
  errorMsg = '';
  userId: number | null = null;
  imageBaseUrl = 'http://localhost:5208/';
  searchTerm: string = '';
  filteredCartItems: any[] = [];

  constructor(private cartService: Cartservice, private authService: Authservice, private cd: ChangeDetectorRef, private router: Router) { }

  ngOnInit(): void {
    window.addEventListener('global-search', (event: any) => {
      this.searchTerm = event.detail;
      this.filterProducts();     // filter inside category
    });
    //  Get logged-in user ID from localStorage
    this.userId = this.authService.getUserId();

    if (this.authService.isLoggedIn() && this.userId) {
      this.loadCart();
    } else {
      alert('Please login to view your cart.');
      this.loading = false;
    }
  }

  //  Fetch user's cart from backend
  loadCart(): void {
    if (!this.userId) return;

    this.loading = true;

    this.cartService.getUserCart(this.userId).subscribe({
      next: (res: any) => {
        const response = Array.isArray(res) ? res : res.response || [];
        this.cartItems = response.map((productData: any) => ({
          ...productData,
          imagePath: productData.productImagePath ? this.imageBaseUrl + productData.productImagePath : 'assets/default-product.png',
          categoryName: productData.category?.categoryName,
          supplierName: productData.supplier?.supplierName || 'N/A'
        }));
        this.filteredCartItems = [...this.cartItems];
        this.calculateTotal();
        this.loading = false;
        this.cd.detectChanges();
      },
      error: (err) => {
        console.error('Error fetching cart:', err);
        this.errorMsg = 'Failed to load cart. Please try again.';
        this.loading = false;
        this.cartItems = [];
        this.cd.detectChanges();
      }
    });
  }

  //  Calculate total price
  calculateTotal(): void {
    this.total = this.cartItems.reduce(
      (sum, item) => sum + (item.totalPrice || 0),
      0
    );
  }

  //  Remove a specific item
  removeItem(cartId: number): void {
    if (!confirm('Are you sure you want to remove this item?')) return;

    this.cartItems = this.cartItems.filter(item => item.cartId !== cartId);
    this.filteredCartItems = [...this.cartItems];
    this.calculateTotal();
    this.cd.detectChanges();

    this.cartService.removeCartItem(cartId).subscribe({
      next: (response) => {
        console.log('Success response:', response);

        setTimeout(() => this.loadCart(), 500);
        this.cd.detectChanges();
        alert('Item removed from cart.');


      },
      error: (err) => {
        console.error('Error removing item:', err);
      }
    });
  }

  clearCart(): void {
    if (!this.userId) {
      alert('Please login.');
      return;
    }

    if (this.cartItems.length === 0) {
      alert('Your cart is already empty.');
      return;
    }

    if (!confirm("Are you sure you want to clear your cart?")) return;

    this.cartItems = [];
    this.filteredCartItems = [];
    this.total = 0;

    this.cartService.clearCart(this.userId).subscribe({
      next: () => {
        alert("Cart cleared successfully!");
        this.cd.detectChanges();
      },
      error: (err) => console.error("Error clearing cart:", err)
    });
  }


  //update quantity
  updateQuantity(item: any, change: number): void {
    const newQuantity = item.quantity + change;

    if (newQuantity <= 0) {
      this.removeItem(item.cartId);
      return;
    }

    const payload = {
      userId: this.userId,
      productId: item.productId,
      quantity: newQuantity
    };

    // Instant frontend update
    item.quantity = newQuantity;
    item.totalPrice = item.productPrice * newQuantity;
    this.calculateTotal();
    this.cd.detectChanges();

    // Send update to backend
    this.cartService.addOrUpdateCartItem(payload).subscribe({
      next: () => console.log('✅ Quantity updated'),
      error: (err) => console.error('❌ Error updating quantity:', err)
    });
  }

  //checkout 

  checkout(): void {
    if (!this.userId || this.cartItems.length == 0) {
      alert('cart is empty');
      return;
    }
    if (this.cartItems.length === 0) {
      alert('Your cart is empty.');
      return;
    }

    this.router.navigate(['/orders/checkout']);
  }


  // filter 
  filterProducts() {
    const term = this.searchTerm.toLowerCase().trim();

    // If search box empty → show full cart again
    if (!term) {
      this.filteredCartItems = [...this.cartItems];
      return;
    }

    // Filter
    this.filteredCartItems = this.cartItems.filter(item =>
      item.productName.toLowerCase().includes(term) ||
      item.categoryName?.toLowerCase().includes(term) ||
      item.description?.toLowerCase().includes(term)
    );
  }



}
