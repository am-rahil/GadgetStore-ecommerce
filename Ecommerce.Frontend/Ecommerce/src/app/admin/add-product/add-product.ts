import { ChangeDetectorRef, Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Productservice } from '../../core/services/productservice';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-add-product',
  standalone: false,
  templateUrl: './add-product.html',
  styleUrl: './add-product.css'
})
export class AddProduct {
  productForm!: FormGroup;
  productId!: number;
  galleryFiles: File[] = [];
  galleryPreview: any[] = [];

  loading = true;

  imageBaseUrl = 'http://localhost:5208/';
  imagePreview: string | ArrayBuffer | null = null;
  currentImagePath: string | null = null;

  selectedFile: File | null = null; // NEW – same as Add product


  categories = [
    { id: 1, name: 'Smartphones' },
    { id: 2, name: 'Laptops' },
    { id: 3, name: 'Headphones' },
    { id: 4, name: 'Smart Watches' }
  ];

  suppliers = [
    { id: 1, name: 'SmartPhone Distributer' },
    { id: 2, name: 'HeadPhone Distributer' },
    { id: 3, name: 'SmartWatch Distributer' },
    { id: 4, name: 'Laptop Distributer' }
  ];

  constructor(
    private fb: FormBuilder,
    private productService: Productservice,
    private toastr: ToastrService,
    private route: ActivatedRoute,
    private router: Router,
    private cd: ChangeDetectorRef
  ) { }

  ngOnInit(): void {

    // Create form
    this.productForm = this.fb.group({
      productName: ['', Validators.required],
      description: [''],
      price: [null, Validators.required],
      stockQuantity: [null, Validators.required],
      categoryId: [null, Validators.required],
      supplierId: [null, Validators.required],
    });

    // Read the product ID from route
    this.route.paramMap.subscribe(params => {
      const id = params.get('productId');
      if (id) {
        this.productId = parseInt(id);
        this.loadProductDetails(this.productId);
      }
    });
  }


  // Load the selected product's data
  loadProductDetails(id: number): void {
    this.productService.getProductsByid(id).subscribe({
      next: (res) => {
        const p = res.response || res;

        this.productForm.patchValue({
          productName: p.productName,
          description: p.description,
          price: p.price,
          stockQuantity: p.stockQuantity,
          categoryId: p.categoryId,
          supplierId: p.supplierId
        });

        this.currentImagePath = p.imagePath
          ? this.imageBaseUrl + p.imagePath
          : null;

        this.loading = false;
        this.cd.detectChanges();
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  // When user selects a new image
  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];

    if (this.selectedFile) {
      const reader = new FileReader();
      reader.onload = () => (this.imagePreview = reader.result);
      reader.readAsDataURL(this.selectedFile);
    }
  }

  // Submit update
  onSubmit(): void {

    if (this.productForm.invalid) {
      alert('Please fill all required fields.');
      return;
    }

    // Prepare formData manually (BEGINNER FRIENDLY)
    const values = this.productForm.value;
    const formData = new FormData();

    formData.append('productName', values.productName);
    formData.append('description', values.description);
    formData.append('price', String(values.price));             // Convert to string
    formData.append('stockQuantity', String(values.stockQuantity));
    formData.append('categoryId', String(values.categoryId));
    formData.append('supplierId', String(values.supplierId));


    // Main image
    if (this.selectedFile) {
      formData.append('imageFile', this.selectedFile);
    }

    // Gallery Images
    if (this.galleryFiles.length > 0) {
      this.galleryFiles.forEach(f => {
        formData.append('GalleryImages', f);
      });
    }

    // Call service
    this.productService.addProduct(formData).subscribe({
      next: () => {
        alert('Product added successfully!');
        this.router.navigate(['/admin/manage-products']);
      },
      error: (err) => {
        console.error('Add failed:', err);
        alert('Failed to add product.');
      }
    });
  }

  onGallerySelected(event: any) {
    this.galleryFiles = Array.from(event.target.files);

    // Previews
    this.galleryPreview = [];
    this.galleryFiles.forEach(file => {
      const reader = new FileReader();
      reader.onload = () => this.galleryPreview.push(reader.result);
      reader.readAsDataURL(file);
    });
  }


}

