export interface Product {
  productId: number;
  productName: string;
  description: string;
  price: number;
  stockQuantity: number;
  categoryId: number;
  supplierId: number;
  imagePath?: string; // main image
  categoryName?: string;
  supplierName?: string;
  imagePaths?: string[];
}
