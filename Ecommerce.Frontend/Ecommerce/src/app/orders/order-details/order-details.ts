import { ChangeDetectorRef, Component } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { Orderservice } from '../../core/services/orderservice';
import jsPDF from 'jspdf';
import 'jspdf-autotable';
import autoTable from 'jspdf-autotable';


@Component({
  selector: 'app-order-details',
  standalone: false,
  templateUrl: './order-details.html',
  styleUrl: './order-details.css'
})
export class OrderDetails {
  order: any;
  loading = true;
  imageBaseUrl = 'http://localhost:5208/';
  constructor(
    private route: ActivatedRoute,
    private orderService: Orderservice,
    private router: Router,
    private cd: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    const orderId = this.route.snapshot.paramMap.get('orderId');
    if (orderId) {
      this.loadOrderDetails(+orderId);
    }
  }


  loadOrderDetails(orderId: number): void {
    this.orderService.getOrderById(orderId).subscribe({
      next: (res) => {
        this.order = res?.response || res;
        this.loading = false;
        this.cd.detectChanges();
      },
      error: (err) => {
        console.error('Error fetching order:', err);
        this.loading = false;
      }
    });
  }

  backToOrders(): void {
    this.router.navigate(['/orders/orderlist']);
  }

  downloadReceipt() {
    const doc = new jsPDF({
      compress: false,
      unit: "pt",
      format: "a4"
    });

    doc.setFont("helvetica", "normal");

    // Title
    doc.setFontSize(16);
    doc.text("Order Receipt", 300, 40, { align: 'center' });

    doc.setFontSize(11);
    doc.text(`Order ID: ${this.order.orderId}`, 40, 70);
    doc.text(`Date: ${new Date(this.order.orderDate).toLocaleString()}`, 40, 90);
    doc.text(`Status: ${this.order.status}`, 40, 110);

    const totalAmount = this.cleanNumber(this.order.totalAmount);
    doc.text(`Total Amount: ₹${this.formatNumber(totalAmount)}`, 40, 130);

    // Shipping
    doc.text("Shipping Address:", 40, 160);
    const addressLines = this.order.address.split("\n");
    let y = 180;
    addressLines.forEach((line: string) => {
      doc.text(line, 40, y);
      y += 15;
    });

    doc.text(`Pincode: ${this.order.pincode}`, 40, y + 10);
    doc.text(`Phone: ${this.order.phone}`, 40, y + 30);

    // Table rows
    const rows = this.order.orderDetails.map((item: any) => [
      item.productName,
      `₹${this.formatNumber(item.unitPrice)}`,
      item.quantity,
      `₹${this.formatNumber(item.subTotal)}`
    ]);

    // Add Grand Total
    rows.push([
      { content: "Grand Total", colSpan: 3, styles: { halign: "right", fontStyle: "bold" } },
      `₹${this.formatNumber(totalAmount)}`
    ]);

    autoTable(doc, {
      startY: y + 60,
      head: [['Product', 'Unit Price', 'Qty', 'Subtotal']],
      body: rows,
      styles: {
        font: "helvetica",
        fontSize: 10,
      },
      headStyles: {
        fillColor: [40, 116, 166],
        textColor: 255
      }
    });

    doc.save(`order-${this.order.orderId}.pdf`);
  }



  cleanNumber(value: any): number {
    if (value == null) return 0;

    // Convert to string
    let val = String(value);

    // Normalize unicode (removes superscripts etc)
    val = val.normalize("NFKD");

    // Remove ALL invisible unicode spaces
    val = val.replace(/[\u00A0\u200B\u200C\u200D]/g, "");

    // Remove any remaining non-numeric characters except .
    val = val.replace(/[^0-9.]/g, "");

    return Number(val) || 0;
  }



formatNumber(value: number): string {
  return value.toLocaleString("en-US", {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2
  });
}





}
