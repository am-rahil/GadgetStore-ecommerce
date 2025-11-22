import { ChangeDetectorRef, Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Cartservice } from '../../core/services/cartservice';
import { Authservice } from '../../core/services/authservice';
import { Orderservice } from '../../core/services/orderservice';
import { Router } from '@angular/router';

@Component({
  selector: 'app-checkout',
  standalone: false,
  templateUrl: './checkout.html',
  styleUrl: './checkout.css'
})
export class Checkout {
  checkoutForm!: FormGroup;
  cartItems: any[] = [];
  totalAmount = 0;
  loading = false;

  constructor(
    private fb: FormBuilder,
    private cartService: Cartservice,
    private authService: Authservice,
    private orderService: Orderservice,
    private router: Router,
    private cd: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.checkoutForm = this.fb.group({
      address: ['', Validators.required],
      pincode: ['', Validators.required],
      phone: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]],
      paymentMethod: ['COD', Validators.required],
    });

    this.loadCart();
  }

  loadCart() {
    const userId = this.authService.getUserId();
    this.cartService.getUserCart(userId).subscribe({
      next: (res: any) => {
        this.cartItems = res;
        this.totalAmount = this.cartItems.reduce(
          (sum, item) => sum + (item.totalPrice || 0),
          0
        );
        this.cd.detectChanges();
      },
      error: (err) => console.error('Error loading cart:', err)
    });
  }
  placeOrder() {
    if (this.checkoutForm.invalid) {
      alert('Please fill in all fields correctly.');
      return;
    }

    const userId = this.authService.getUserId();

    const orderDetails = this.cartItems.map((item) => ({
      productId: item.productId,
      quantity: item.quantity,
      unitPrice: item.totalPrice / item.quantity,
      imagePath: item.productImagePath
    }));
    console.log('Cart items for order:', this.cartItems);
     console.log('Order details being sent:', orderDetails);

    const payload = {
      userId: userId,
      totalAmount: this.totalAmount,
      address: this.checkoutForm.value.address,
      pincode: this.checkoutForm.value.pincode,
      phone: this.checkoutForm.value.phone,
      status: 'Pending',
      paymentMethod: this.checkoutForm.value.paymentMethod,
      orderDetails: orderDetails
    };
    console.log('paylod',payload)

    this.loading = true;
    this.orderService.createOrder(payload).subscribe({
      next: (res) => {
        alert(' Order placed successfully!');
        this.loading = false;
        this.router.navigate(['/orders/myorders']); 
      },
      error: (err) => {
        console.error('Error placing order:', err);
        alert(' Failed to place order');
        this.loading = false;
      }
    });
  }


}
