import { ChangeDetectorRef, Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { Productservice } from '../../core/services/productservice';

@Component({
  selector: 'app-update-product',
  standalone: false,
  templateUrl: './update-product.html',
  styleUrl: './update-product.css'
})
export class UpdateProduct {
  imageBaseUrl = 'http://localhost:5208/';
  productForm!: FormGroup;
  productId!: number; // ✅ single declaration
  loading = true;
  imagePreview: string | ArrayBuffer | null = null;
  currentImagePath: string | null = null;
  galleryImages: string[] = [];
  newGalleryImages: File[] = [];


  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private productService: Productservice,
    private cd: ChangeDetectorRef
  ) { }

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

  ngOnInit(): void {
    // Initialize form controls
    this.productForm = this.fb.group({
      productName: ['', Validators.required],
      description: [''],
      price: ['', Validators.required],
      stockQuantity: ['', Validators.required],
      categoryId: ['', Validators.required],
      supplierId: ['', Validators.required],
      imageFile: [null]
    });

    this.route.paramMap.subscribe(params => {
      const id = params.get('productId');
      if (id) {
        this.productId = +id;
        this.loadProductDetails(this.productId);
      }
    });

  }

  //  Load Product by ID
  loadProductDetails(id: number): void {
    this.productService.getProductsByid(id).subscribe({
      next: (res) => {
        const data = res.response || res; // handles both response-wrapped and plain objects

        this.productForm.patchValue({
          productName: data.productName,
          description: data.description,
          price: data.price,
          stockQuantity: data.stockQuantity,
          categoryId: data.categoryId,
          supplierId: data.supplierId
        });

        this.currentImagePath = data.imagePath
          ? this.imageBaseUrl + data.imagePath
          : null;

        this.galleryImages = data.imagePaths
          ? data.imagePaths.map((path: string) => this.imageBaseUrl + path)
          : [];

        this.loading = false;
        this.cd.detectChanges();
      },
      error: (err) => {
        console.error('Error loading product:', err);
        this.loading = false;
      }
    });
  }


  // When user selects a new image
  onImageChange(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.productForm.patchValue({ imageFile: file });

      // show a preview of selected image
      const reader = new FileReader();
      reader.onload = () => (this.imagePreview = reader.result);
      reader.readAsDataURL(file);
    }
  }


  updateProduct(): void {
    if (this.productForm.invalid) {
      alert('Please fill all required fields.');
      return;
    }

    // create a formData object to send text + file together
    const formData = new FormData();

    // add each field from form into formData
    Object.entries(this.productForm.value).forEach(([key, value]) => {
      if (value !== null) formData.append(key, value as any);
    });

    //  NEW ADD GALLERY IMAGES
    if (this.newGalleryImages.length > 0) {
      this.newGalleryImages.forEach((file) => {
        formData.append("GalleryImages", file);
      });
    }
    // call update API
    this.productService.updateProduct(this.productId, formData).subscribe({
      next: () => {
        alert('Product updated successfully!');
        this.router.navigate(['/admin/manage-products']); // go back to list
      },
      error: (err) => {
        console.error('Error updating product:', err);
        alert('Failed to update product.');
      }
    });
  }

  //Adding gallery images
  onGalleryImagesChange(event: any): void {
    const files = event.target.files;
    this.newGalleryImages = [];

    for (let file of files) {
      this.newGalleryImages.push(file);
    }
  }

  clearGallery() {
  if (!confirm("Are you sure you want to delete all gallery images?")) return;

  this.productService.clearGalleryImages(this.productId).subscribe({
    next: () => {
      alert("All gallery images deleted!");
      this.galleryImages = []; // update UI
    },
    error: (err) => console.error(err)
  });
}



}