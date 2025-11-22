import { ChangeDetectorRef, Component } from '@angular/core';
import { Product } from '../../core/models/product.model';
import { ActivatedRoute, Router } from '@angular/router';
import { Productservice } from '../../core/services/productservice';
import { Authservice } from '../../core/services/authservice';
import { Cartservice } from '../../core/services/cartservice';
import { Category } from '../../core/models/category.model';

@Component({
  selector: 'app-category-products',
  standalone: false,
  templateUrl: './category-products.html',
  styleUrl: './category-products.css'
})
export class CategoryProducts {

  products: Product[] = [];
  filteredProducts: Product[] = [];

  categoryId!: number;
  loading = true;
  errorMsg = '';
  imageBaseUrl = 'http://localhost:5208/';

  searchTerm: string = '';

  // Filters
  sortType: string = '';
  minPrice: number | null = null;
  maxPrice: number | null = null;

  // Categories
  categories: Category[] = [
    { categoryId: 1, categoryName: 'Smartphones' },
    { categoryId: 2, categoryName: 'Laptops' },
    { categoryId: 3, categoryName: 'Headphones' },
    { categoryId: 4, categoryName: 'Smart Watches' }
  ];

  constructor(
    private route: ActivatedRoute,
    private productService: Productservice,
    private cd: ChangeDetectorRef,
    private router: Router,
    private authService: Authservice,
    private cartService: Cartservice
  ) {}

  ngOnInit(): void {
    // Listen for search from navbar
    window.addEventListener('global-search', (event: any) => {
      this.searchTerm = event.detail;
      this.filterProducts();
    });

    // Read category ID from route
    this.route.paramMap.subscribe(params => {
      const id = params.get('categoryId');
      if (id) {
        this.categoryId = +id;
        this.loadProductsByCategory(this.categoryId);
      }
    });
  }

  loadProductsByCategory(id: number) {
    this.productService.getProductsBycategoryId(id).subscribe({
      next: (res: any) => {
        this.products = (res.response || []).map((p: any) => ({
          ...p,
          imagePath: p.imagePath ? this.imageBaseUrl + p.imagePath : 'assets/no-image.png',
          categoryName: p.category?.categoryName,
          supplierName: p.supplier?.supplierName
        }));

        this.filteredProducts = [...this.products];
        this.loading = false;
        this.cd.detectChanges();
      },
      error: () => {
        this.errorMsg = 'Failed to load products. Please try again later.';
        this.loading = false;
      }
    });
  }

  getCategoryName(): string {
    if (this.products.length > 0 && this.products[0].categoryName) {
      return this.products[0].categoryName;
    }

    const map: any = {
      1: 'Smartphones',
      2: 'Laptops',
      3: 'Headphones',
      4: 'Smart Watches'
    };

    return map[this.categoryId] || 'Products';
  }

  // Search filter
  filterProducts() {
    const term = this.searchTerm.toLowerCase().trim();

    if (!term) {
      this.filteredProducts = [...this.products];
      return;
    }

    this.filteredProducts = this.products.filter(p =>
      p.productName.toLowerCase().includes(term) ||
      p.description.toLowerCase().includes(term)
    );
  }

  // Apply sorting + price filters
  applyFilters() {
    let filtered = [...this.products];

    // Sorting
    if (this.sortType === 'low') {
      filtered.sort((a, b) => a.price - b.price);
    } else if (this.sortType === 'high') {
      filtered.sort((a, b) => b.price - a.price);
    }

    // Min price
    if (this.minPrice !== null) {
      filtered = filtered.filter(p => p.price >= this.minPrice!);
    }

    // Max price
    if (this.maxPrice !== null) {
      filtered = filtered.filter(p => p.price <= this.maxPrice!);
    }

    this.filteredProducts = filtered;
  }

  viewProductDetails(productId: number) {
    this.router.navigate(['/products/productdetail', productId]);
  }

  // Add to cart
  addToCart(event: Event, productId: number) {
    event.stopPropagation();

    if (!this.authService.isLoggedIn()) {
      alert('Please log in to add items to your cart.');
      this.router.navigate(['/cart/cartpage']);
      return;
    }

    const userId = this.authService.getUserId();
    if (!userId) return;

    const payload = {
      userId,
      productId,
      quantity: 1
    };

    this.cartService.addOrUpdateCartItem(payload).subscribe({
      next: () => alert('Item added to your cart!'),
      error: () => alert('Failed to add item. Try again.')
    });
  }

  // Change category
  filterByCategory(categoryId: number) {
    this.router.navigate(['/products/category', categoryId]);
  }
}
