import { ChangeDetectorRef, Component } from '@angular/core';
import { Orderservice } from '../../core/services/orderservice';
import { Router } from '@angular/router';
import { ColDef } from 'ag-grid-community';

@Component({
  selector: 'app-manage-orders',
  standalone: false,
  templateUrl: './manage-orders.html',
  styleUrl: './manage-orders.css'
})
export class ManageOrders {
  orders: any[] = [];
  selectedOrder: any = null; // Store the selected order details
  showModal = false; // Control modal visibility
  loadingDetails = false; // Loading state for modal
  imageBaseUrl = 'http://localhost:5208/';

  columnDefs: ColDef[] = [
    { headerName: 'Order ID', field: 'orderId', sortable: true, filter: false, width: 125, flex: 0 },
    { headerName: 'User', field: 'userName', sortable: true, filter: true },
    { headerName: 'Total', field: 'totalAmount', filter: false, valueFormatter: p => '₹' + p.value },
    { headerName: 'Status', field: 'status', sortable: true, filter: true,},
    {
      headerName: 'Update Status',
      field: 'status', filter: false,
      cellEditor: 'agSelectCellEditor',
      cellEditorParams: {
        values: ['Pending', 'Shipped', 'Delivered', 'Cancelled'],
      },
      editable: true,
      cellRenderer: (params: any) => {

        return `
      <div class="d-flex justify-content-between align-items-center">
        <span class="status-badge">${params.value || 'Click to edit'}</span>
        <i class="bi bi-chevron-down text-muted ms-2"></i>
      </div>
    `;
      }
    },
    {
      headerName: 'View', filter: false,
      field: 'orderId',
      cellRenderer: (params: any) =>
        `<button class="btn btn-sm btn-outline-success" data-action="view" data-id="${params.data.orderId}">View</button>`
    }
  ];

  defaultColDef: ColDef = {
    sortable: true,
    filter: true,
    resizable: true,
    flex: 1
  };


  onCellValueChanged(event: any) {
    console.log('Cell value changed:', event);
    if (event.colDef.field === 'status') {
      this.updateStatus(event.data.orderId, event.newValue);
    }
  }

  onGridClick(event: any) {
    const action = event.event.target.getAttribute('data-action');
    const id = event.event.target.getAttribute('data-id');
    const value = event.event.target.value;

    if (action === 'update' && value) {
      this.updateStatus(Number(id), value);
    }

    if (action === 'view') {
      this.viewOrder(Number(id));
    }
  }

  constructor(private orderService: Orderservice, private cd: ChangeDetectorRef, private router: Router) { }

  ngOnInit(): void {
    this.loadOrders();
  }

  loadOrders() {
    this.orderService.getAllOrders().subscribe({
      next: (res: any) => {
        this.orders = Array.isArray(res) ? res : res.response || [];
        this.cd.detectChanges();
      },
      error: (err) => console.error('Error loading orders:', err)
    });
  }

  updateStatus(orderId: number, status: string) {
    if (!confirm(`Change order #${orderId} status to ${status}?`)) return;

    this.orderService.updateOrderStatus(orderId, status).subscribe({
      next: () => {
        alert(`Order #${orderId} updated to ${status}`);
        this.loadOrders();
        this.cd.detectChanges();
      },
      error: (err) => {
        console.error(err);
        alert('Failed to update order status.');
      }
    });
  }


  // View order in modal
  viewOrder(orderId: number) {
    this.loadingDetails = true;
    this.showModal = true;

    this.orderService.getOrderById(orderId).subscribe({
      next: (res: any) => {
        this.selectedOrder = res.response || res;
        this.loadingDetails = false;
        this.cd.detectChanges();
      },
      error: (err) => {
        console.error('Error loading order details:', err);
        this.loadingDetails = false;
        this.closeModal();
      }
    });
  }

  // Close modal
  closeModal() {
    this.showModal = false;
    this.selectedOrder = null;
    this.loadingDetails = false;
  }

}
