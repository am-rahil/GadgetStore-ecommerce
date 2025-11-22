import { ChangeDetectorRef, Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Categoryservice } from '../../core/services/categoryservice';

@Component({
  selector: 'app-manage-categories',
  standalone: false,
  templateUrl: './manage-categories.html',
  styleUrl: './manage-categories.css'
})
export class ManageCategories {
  categories: any[] = [];
  categoryForm!: FormGroup;
  editingCategoryId: number | null = null;
  loading = true;
  constructor(private categoryService: Categoryservice, private fb: FormBuilder, private cd: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.categoryForm = this.fb.group({
      categoryName: ['', Validators.required],
      description: ['']
    });
    this.loadCategories();
  }
  loadCategories() {
    this.categoryService.getallCategories().subscribe({
      next: (res) => {
        this.categories = res.response || [];
        this.loading = false;
        this.cd.detectChanges();
      },
      error: (err) => {
        console.error('Error fetching categories:', err);
        this.loading = false;
      }
    });
  }
  submitForm() {
    if (this.categoryForm.invalid) {
      alert('Please enter a category name');
      return;
    }

    const formData = this.categoryForm.value;

    if (this.editingCategoryId) {
      // Update existing category
      this.categoryService.updateCategory(this.editingCategoryId, formData).subscribe({
        next: () => {
          alert('Category updated successfully!');
          this.resetForm();
          this.loadCategories();
        },
        error: (err) => {
          console.error('Error updating category:', err);
          alert('Failed to update category');
        }
      });
    } else {
      // Add new category
      this.categoryService.addCategory(formData).subscribe({
        next: () => {
          alert('Category added successfully!');
          this.resetForm();
          this.loadCategories();
        },
        error: (err) => {
          console.error('Error adding category:', err);
          alert('Failed to add category');
        }
      });
    }
  }

  editCategory(cat: any) {
    this.editingCategoryId = cat.categoryId;
    this.categoryForm.patchValue({
      categoryName: cat.categoryName,
      description: cat.description
    });
  }

  deleteCategory(id: number) {
    if (!confirm('Are you sure you want to delete this category?')) return;

    this.categoryService.deleteCategory(id).subscribe({
      next: () => {
        alert('Category deleted successfully!');
        this.loadCategories();
      },
      error: (err) => {
        console.error('Error deleting category:', err);
        alert('Failed to delete category');
      }
    });
  }

  resetForm() {
    this.editingCategoryId = null;
    this.categoryForm.reset();
  }


}
