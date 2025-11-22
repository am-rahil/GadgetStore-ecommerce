import { ChangeDetectorRef, Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Productservice } from '../../core/services/productservice';
import { Authservice } from '../../core/services/authservice';
import { Cartservice } from '../../core/services/cartservice';

@Component({
  selector: 'app-product-details',
  standalone: false,
  templateUrl: './product-details.html',
  styleUrl: './product-details.css'
})
export class ProductDetails {
  imageBaseUrl = 'http://localhost:5208/';
  product: any;
  loading = true;
  errorMsg = '';
  selectedImage: string | null = null;
  similarProducts: any[] = [];


  constructor(
    private route: ActivatedRoute,
    private productservice: Productservice,
    private authService: Authservice,
    private router: Router,
    private cd: ChangeDetectorRef,
    private cartService: Cartservice) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const productId = params.get('productId');
      if (productId) {
        this.loadProductDetails(Number(productId));
      }
    });
  }

  loadProductDetails(id: number) {
    this.productservice.getProductsByid(id).subscribe({
      next: (res) => {
        const productData = res.response;
        this.product = {
          ...productData,
          imagePath: productData.imagePath ? this.imageBaseUrl + productData.imagePath : 'assets/default-product.png',
          imagePaths: (productData.imagePaths || []).map((p: string) => this.imageBaseUrl + p),
          categoryName: productData.categoryName || 'N/A',
          supplierName: productData.supplierName || 'N/A'
        };
        this.selectedImage = null;
        window.scrollTo({ top: 0, behavior: 'smooth' });
        this.loadSimilarProducts(productData.categoryId);

        console.log(this.product);
        this.loading = false;
        this.cd.detectChanges();

      },
      error: (err) => {
        console.error(err);
        this.errorMsg = 'Unable to load product details.';
        this.loading = false;
      }
    });
  }
  //  Add product to cart
  addToCart(productId: number): void {
    // Check if logged in
    if (!this.authService.isLoggedIn()) {
      alert('Please log in to add items to your cart.');
      this.router.navigate(['/cart/cartpage']);
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
        alert('✅ Item added to your cart!');
        // this.router.navigate(['/cart/cartpage']);
      },
      error: (err) => {
        console.error('Error adding to cart:', err);
        alert(' Failed to add item. Please try again.');
      }
    });
  }

  loadSimilarProducts(categoryId: number) {
    this.productservice.getProductsBycategoryId(categoryId).subscribe({
      next: (res) => {
        this.similarProducts = res.response.filter((p: any) => p.productId !== this.product.productId);

        // Convert image path for UI
        this.similarProducts = this.similarProducts.map(sp => ({
          ...sp,
          imagePath: this.imageBaseUrl + sp.imagePath
        }));

        this.cd.detectChanges();
      },
      error: () => {
        console.error("Failed to load similar products");
      }
    });
  }

  buyNow(productId: number) {
    if (!this.authService.isLoggedIn()) {
      alert('Please log in to continue.');
      this.router.navigate(['/login']);
      return;
    }

    this.router.navigate(['/checkout'], {
      queryParams: { productId }
    });

  }
  viewProduct(id: number) {
    this.router.navigate(['/products/productdetail', id]).then(() => {
      this.backtotop();
    })
  }
  backtotop() {
 window.scrollTo({ top: 0, behavior: 'smooth' });
  }
}