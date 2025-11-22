import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Product } from '../../core/models/product.model';
import { Productservice } from '../../core/services/productservice';
import { Router } from '@angular/router';
import { ColDef } from 'ag-grid-community';


@Component({
  selector: 'app-manage-products',
  standalone: false,
  templateUrl: './manage-products.html',
  styleUrls: ['./manage-products.css']
})
export class ManageProducts implements OnInit {
  products: Product[] = [];
  imageBaseUrl = 'http://localhost:5208/';

  //  Define AG Grid columns
  columnDefs: ColDef[] = [
    { headerName: '#', valueGetter: 'node.rowIndex + 1', width: 60, filter: false ,flex :0},
    { headerName: 'Product Name', field: 'productName', sortable: true, filter: true },
    { headerName: 'Price', field: 'price', sortable: true, filter: true, valueFormatter: p => '₹' + p.value },
    { headerName: 'Stock', field: 'stockQuantity', sortable: true, filter: false },
    { headerName: 'Category', field: 'categoryName' },
    { headerName: 'Supplier', field: 'supplierName' },
    {
      headerName: 'Image',
      field: 'imagePath', filter: false,
      cellRenderer: (params: any) =>
        `<img src="${params.value}" width="40" height="40" style="border-radius:8px; object-fit:cover;" />`
    },
    {
      headerName: 'Actions', filter: false,
      cellRenderer: (params: any) => `
        <button class="btn btn-sm btn-outline-success" data-action="edit">Edit</button>
        <button class="btn btn-sm btn-outline-danger ms-2" data-action="delete">Delete</button>
      `
    }
  ];

  //  Default column behavior
  defaultColDef: ColDef = {
    resizable: true,
    filter: true,
    sortable: true,
    flex: 1
  };

  constructor(private productService: Productservice, private router: Router, private cd: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.loadProducts();
  }


  loadProducts() {
    this.productService.getAllProducts().subscribe({
      next: (res) => {
        this.products = (res.response || []).map((p: any) => ({
          ...p,
          imagePath: p.imagePath ? this.imageBaseUrl + p.imagePath : 'assets/',
          categoryName: p.category?.categoryName || 'N/A',// keep backend field name consistent
          supplierName: p.supplier?.supplierName || 'N/A'
        }));
        this.cd.detectChanges();
      },
      error: (err) => console.error('Error fetching products:', err)
    });
  }

  //  Handle edit/delete button clicks
  onGridClick(event: any) {
    const action = event.event.target.getAttribute('data-action');
    const data = event.data;

    if (action === 'edit') this.editProduct(data.productId);
    if (action === 'delete') this.deleteProduct(data.productId);
  }

  addproduct() {
    this.router.navigate(['/admin/add-product']);
  }

  editProduct(productId: number) {
    this.router.navigate(['/admin/updateProduct', productId]);
  }

  deleteProduct(id?: number) {
    if (!id) return;
    if (confirm('Are you sure to delete this product?')) {
      this.productService.deleteProduct(id).subscribe({
        next: () => {
          alert('Product deleted!');
          this.loadProducts(); // Reload the products after deleting
        },
        error: (err) => console.error('Error deleting product:', err)
      });
    }
  }
}
