🛒 GadgetStore – Full Stack Ecommerce Application

GadgetStore is a **full-stack ecommerce web application** built using **ASP.NET Core Web API** for the backend and **Angular** for the frontend.  
It supports **role-based authentication (Admin & Customer)**, secure **JWT authorization**, and real-world ecommerce workflows.

This project follows **clean architecture**, **scalable design principles**, and is suitable for **production-level applications**.

---

 🚀 Key Features

🔐 Authentication & Authorization
- JWT-based authentication
- Role-based access (**Admin / Customer**)
- Secure login & registration
- Protected API endpoints
- Angular route guards & interceptors

 🛍️ Customer Features
- Browse products by category
- View detailed product information
- Add / update cart items
- Place orders
- Track order status
- View order history

🧑‍💼 Admin Features
- Admin dashboard with analytics
- Manage products (Add / Edit / Delete)
- Manage categories
- Manage users
- Manage orders & update order status
- Stock auto-adjustment logic

### 📦 Inventory Management
- Stock decreases when order is **Delivered**
- Stock restores when order is **Cancelled**
- Supports multiple quantities per product
- Prevents negative stock values

---

 🧱 Tech Stack

#Backend
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **SQL Server**
- **ASP.NET Identity**
- **JWT Authentication**
- Repository Pattern

# Frontend
- **Angular**
- **Reactive Forms**
- **Lazy-Loaded Modules**
- **JWT Interceptor**
- **Route Guards**
- Bootstrap UI

---

## 📁 Project Folder Structure

### Backend – `Ecommerce.Api`
Ecommerce.Api/
├── Ecommerce.Api/
│ ├── Controllers/
│ ├── Properties/
│ ├── wwwroot/
│ │ └── Images/
│ ├── appsettings.json
│ ├── Program.cs
│
├── Ecommerce.Common/
│ └── CommonDto/
│
├── Ecommerce.Entity/
│ ├── DTO/
│ ├── Models/
│ ├── Migrations/
│
└── Ecommerce.Service/
└── Repository/


### Frontend – `Ecommerce.Frontend` (Angular)
Ecommerce.Frontend/
└── Ecommerce/
├── src/
│ ├── app/
│ │ ├── admin/
│ │ ├── auth/
│ │ ├── core/
│ │ ├── layout/
│ │ ├── orders/
│ │ ├── pages/
│ │ ├── products/
│ │ ├── shared/
│ │ ├── app.module.ts
│ │ └── app-routing.module.ts
│ ├── assets/
│ ├── environments/
│ ├── index.html
│ ├── main.ts
│ └── styles.css
├── angular.json
├── package.json
└── README.md


---

## 🔐 User Roles

| Role | Permissions |
|-----|------------|
| **Admin** | Dashboard, Products, Categories, Users, Orders |
| **Customer** | Browse Products, Cart, Orders |

---

## ⚙️ How to Run the Project

### Backend Setup
1. Open the solution in **Visual Studio**
2. Update SQL Server connection string in `appsettings.json`
3. Run EF Core migrations
4. Start the API

### Frontend Setup
```bash
cd Ecommerce.Frontend/Ecommerce
npm install
ng serve


