import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Productservice } from '../../core/services/productservice';
import { Product } from '../../core/models/product.model';
import { Router } from '@angular/router';
import { Authservice } from '../../core/services/authservice';
import { Cartservice } from '../../core/services/cartservice';
import { Category } from '../../core/models/category.model';

@Component({
  selector: 'app-home',
  standalone: false,
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home implements OnInit {


  products: Product[] = [];
  loading = true;
  errorMsg = '';
  shuffledProducts: Product[] = [];
  searchTerm: string = '';
  filteredProducts: Product[] = [];


  constructor(
    private productService: Productservice,
    private cd: ChangeDetectorRef,
    private router: Router,
    private authService: Authservice,
    private cartService: Cartservice
  ) { }

  ngOnInit(): void {

    window.addEventListener('global-search', (event: any) => {
      this.searchTerm = event.detail;
      this.filterProducts();     // filter inside category
    });

    this.fetchProducts();
  }

  imageBaseUrl = 'http://localhost:5208/';

  fetchProducts(): void {
    this.productService.getAllProducts().subscribe({
      next: (res) => {
        this.products = (res.response || []).map((p: any) => ({
          ...p,
          imagePath: p.imagePath ? this.imageBaseUrl + p.imagePath : 'assets/',
          categoryName: p.category?.categoryName || 'N/A',// keep backend field name consistent
          supplierName: p.supplier?.supplierName || 'N/A'
        }));
        //shufling products
        this.shuffledProducts = this.shuffleArray([...this.products]);
        this.filteredProducts = [...this.shuffledProducts];  
        this.loading = false;
        this.cd.detectChanges();

      },
      error: (err) => {
        this.loading = false;
        this.errorMsg = 'Failed to load products. Please try again later.';
        console.error('Error fetching products:', err);
      }
    });
  }

  //shuffle products
  shuffleArray(array: any[]): any[] {
    const shuffled = [...array];
    for (let i = shuffled.length - 1; i > 0; i--) {
      const j = Math.floor(Math.random() * (i + 1));
      [shuffled[i], shuffled[j]] = [shuffled[j], shuffled[i]];
    }
    return shuffled.slice(0, 15);
  }

  //navigate single product
  viewProductDetails(productId: number) {
    this.router.navigate(['/products/productdetail', productId]);
  }

  //  Add product to cart
  addToCart(event: Event, productId: number): void {
    event.stopPropagation();

    // Check if logged in
    if (!this.authService.isLoggedIn()) {
      alert('Please log in to add items to your cart.');
      this.router.navigate(['/auth/login']);
      return;
    }

    //  Get userId from AuthService (stored in localStorage)
    const userId = this.authService.getUserId();
    if (!userId) {
      alert('Error: User ID not found.');
      return;
    }

    //  Prepare data for backend
    const payload = {
      userId: userId,
      productId: productId,
      quantity: 1
    };

    //  Send API request
    this.cartService.addOrUpdateCartItem(payload).subscribe({
      next: () => {
        alert(' Item added to your cart!');
        //  this.router.navigate(['/cart/cartpage']);
      },
      error: (err) => {
        console.error('Error adding to cart:', err);
        alert(' Failed to add item. Please try again.');
      }
    });
  }


  //category here
  categories: Category[] = [
    { categoryId: 1, categoryName: 'Smartphones' },
    { categoryId: 2, categoryName: 'Laptops' },
    { categoryId: 3, categoryName: 'Headphones' },
    { categoryId: 4, categoryName: 'Smart Watches' }
  ];
  
  filterByCategory(categoryId: number): void {
    this.router.navigate(['/products/category', categoryId]);
  }


  //search products
  filterProducts() {
    const term = this.searchTerm.toLowerCase().trim();

    // ⭐ If search box becomes empty → reset and scroll UP
    if (!term) {
      this.filteredProducts = [...this.shuffledProducts];
      // Scroll UP to hero/banner
      // setTimeout(() => {
      //   window.scrollTo({ top: 0, behavior: 'smooth' });
      // }, 50);
      return;
    }

    // ⭐ Filter normally
    this.filteredProducts = this.products.filter(p =>
      p.productName.toLowerCase().includes(term) ||
      p.description.toLowerCase().includes(term)
    );

    // Scroll to product section
    setTimeout(() => {
      const section = document.getElementById('productSection');
      if (section) {
        section.scrollIntoView({ behavior: 'smooth' });
      }
    }, 50);
  }

  //when no product backto top
  backtotop() {
        setTimeout(() => {
      const section = document.getElementById('top');
      if (section) {
        section.scrollIntoView({ behavior: 'smooth' });
      }
    }, 50);
  }
  }

