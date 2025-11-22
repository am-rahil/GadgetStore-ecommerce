import { ChangeDetectorRef, Component } from '@angular/core';
import { Authservice } from '../../core/services/authservice';
import { ColDef } from 'ag-grid-community';

@Component({
  selector: 'app-manage-users',
  standalone: false,
  templateUrl: './manage-users.html',
  styleUrl: './manage-users.css'
})
export class ManageUsers {
  users: any[] = [];
  loading = true;
  // AG-Grid Column Definitions
  columnDefs: ColDef[] = [
    { headerName: 'ID', field: 'userId', filter: false, sortable: true, width: 90 },

    { headerName: 'Full Name', field: 'fullName', filter: true, sortable: true },

    { headerName: 'Email', field: 'email', filter: true, sortable: true },

    {
      headerName: 'Role',
      field: 'role',
      filter: false,
      sortable: true,
      cellRenderer: (params: any) => {
        const role = params.value.toLowerCase();
        const span = document.createElement('span');
        span.classList.add('role-badge', role);
        span.textContent = params.value;
        return span;
      },
    },

    {
      headerName: 'Actions',
      width: 130,
      filter:false,
      cellRenderer: (params: any) => {
        return `
          <button class="btn btn-sm btn-danger" data-action="delete" data-id="${params.data.userId}">
            <i class="bi bi-trash"></i> Delete
          </button>
        `;
      },
    },
  ];

  defaultColDef: ColDef = {
    resizable: true,
    sortable: true,
    filter: true,
    flex: 1,
  };
  constructor(private authservice: Authservice, private cd: ChangeDetectorRef) { }

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    this.authservice.getAllUsers().subscribe({
      next: (res) => {
        this.users = res;
        this.loading = false;
        this.cd.detectChanges();
      },
      error: (err) => {
        console.error('Error loading user', err);
      }
    });
  }

  // Handle delete button clicks inside AG Grid
  onGridClick(event: any) {
    const action = event.event.target.getAttribute('data-action');
    const userId = event.event.target.getAttribute('data-id');

    if (action === 'delete' && userId) {
      this.deleteUser(Number(userId));
    }
  }

  deleteUser(userId: number) {
    if (!confirm('Are you sure you want to delete this user?')) return;

    this.authservice.deleteUser(userId).subscribe({
      next: () => {
        alert('User deleted successfully!');
        this.loadUsers();
      },
      error: (err) => {
        console.error('Error deleting user:', err);
        alert('Failed to delete user');
      }
    });
  }

}
